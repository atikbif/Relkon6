/*
 * master.c
 *
 *  Relkon ver 1.0
 *  Author: Роман
 *
 */

#include "master.h"
#include "crc.h"
#include "canal.h"
#include "hcanal.h"
#include "hinout.h"

extern volatile unsigned short rx_mod_cnt;
extern volatile unsigned char obj_name[20];

char test_user(unsigned char* ptr,unsigned char cnt)
{
    unsigned char tmp;
    unsigned char* rx_buf;
    rx_buf = get_can_rx_ptr(3);
    if(rx_buf==0) return 0;
    if(rx_mod_cnt==((unsigned short)cnt)+3)
	{
		if(GetCRC16(rx_buf,((unsigned short)cnt)+3)==0)
		{
			for(tmp=0;tmp<cnt;tmp++) ptr[tmp]=rx_buf[tmp+1];
            return 1;
		}
	}
    return 0;
}

void read_user(unsigned char plc_addr,unsigned char addr,unsigned char cnt)
{
    unsigned short crc_val;
    unsigned char* tx_buf;
    if((unsigned short)addr+(unsigned short)cnt > 256) {rx_mod_cnt=0;return;}
    tx_buf=get_can_tx_ptr(3);
    tx_buf[0]=plc_addr;
	tx_buf[1]=0xD0;
	tx_buf[2]=addr;
    tx_buf[3]=cnt;
    crc_val=GetCRC16(tx_buf,4);
    tx_buf[4]=crc_val>>8;
    tx_buf[5]=crc_val&0xFF;
    write_module(6);
}

char test_xram(unsigned char* ptr,unsigned short cnt)
{
    unsigned short tmp;
    unsigned char* rx_buf;
    rx_buf = get_can_rx_ptr(3);
    if(rx_buf==0) return 0;
    if(rx_mod_cnt==cnt+3)
	{
		if(GetCRC16(rx_buf,cnt+3)==0)
		{
			for(tmp=0;tmp<cnt;tmp++) ptr[tmp]=rx_buf[tmp+1];
            return 1;
		}
	}
    return 0;
}

void read_xram(unsigned char plc_addr,unsigned short xram_addr,unsigned short cnt)
{
    unsigned short crc_val;
    unsigned char* tx_buf;
    tx_buf = get_can_tx_ptr(3);
    if(tx_buf==0) return;
    tx_buf[0]=plc_addr;
	tx_buf[1]=0xD4;
	tx_buf[2]=xram_addr >> 8;
	tx_buf[3]=xram_addr & 0xFF;
    tx_buf[4]=cnt >> 8;
	tx_buf[5]=cnt & 0xFF;
    crc_val=GetCRC16(tx_buf,6);
    tx_buf[6]=crc_val>>8;
    tx_buf[7]=crc_val&0xFF;
    write_module(8);
}

char test_xram_51(unsigned char* ptr)
{
    unsigned char tmp;
    unsigned char* rx_buf;
	rx_buf = get_can_rx_ptr(3);
	if(rx_buf==0) return 0;
    if(rx_mod_cnt==11)
	{
		if(GetCRC16(rx_buf,11)==0)
		{
			for(tmp=0;tmp<8;tmp++) ptr[tmp]=rx_buf[tmp+1];
            return 1;
		}
	}
    return 0;
}

void read_xram_51(unsigned char plc_addr,unsigned short xram_addr)
{
    unsigned short crc_val;
    unsigned char* tx_buf;
	tx_buf = get_can_tx_ptr(3);
	if(tx_buf==0) return;
    tx_buf[0]=plc_addr;
	tx_buf[1]=0xD4;
	tx_buf[2]=xram_addr >> 8;
	tx_buf[3]=xram_addr & 0xFF;
    crc_val=GetCRC16(tx_buf,4);
    tx_buf[4]=crc_val>>8;
    tx_buf[5]=crc_val&0xFF;
    write_module(6);
}

void can_cmd(request* r)
{
	unsigned short crc_val,tmp;
	unsigned char* tx_buf;
	tx_buf = get_can_tx_ptr(r->canal);
	if(tx_buf==0) return;
	switch(r->cmd)
	{
		case WR_RAM:
			tx_buf[0]=r->plc_addr;
			tx_buf[1]=0xE4;
			tx_buf[2]=r->mem_addr >> 8;
			tx_buf[3]=r->mem_addr & 0xFF;
			tx_buf[4]=r->amount >> 8;
			tx_buf[5]=r->amount & 0xFF;
			for(tmp=0;tmp < r->amount;tmp++) tx_buf[6+tmp]=r->tx[tmp];
			crc_val=GetCRC16(tx_buf,6+tmp);
			tx_buf[6+tmp]=crc_val>>8;
			tx_buf[7+tmp]=crc_val&0xFF;
			clear_rx_cnt(r->canal);
			switch(r->canal)
			{
				case 1:write_canal2(8+tmp);break;
				case 2:write_canal(8+tmp);break;
				case 3:write_module(8+tmp);break;
			}
			break;
		case WR_US:
			tx_buf[0]=r->plc_addr;
			tx_buf[1]=0xE0;
			tx_buf[2]=r->mem_addr & 0xFF;
			tx_buf[3]=r->amount & 0xFF;
			for(tmp=0;tmp < r->amount;tmp++) tx_buf[4+tmp]=r->tx[tmp];
			crc_val=GetCRC16(tx_buf,4+tmp);
			tx_buf[4+tmp]=crc_val>>8;
			tx_buf[5+tmp]=crc_val&0xFF;
			clear_rx_cnt(r->canal);
			switch(r->canal)
			{
				case 1:write_canal2(6+tmp);break;
				case 2:write_canal(6+tmp);break;
				case 3:write_module(6+tmp);break;
			}
			break;
		case WR_XRAM:
			tx_buf[0]=r->plc_addr;
			tx_buf[1]=0x64;
			tx_buf[2]=r->mem_addr >> 8;
			tx_buf[3]=r->mem_addr & 0xFF;
			for(tmp=0;tmp < r->amount;tmp++) tx_buf[4+tmp]=r->tx[tmp];
			crc_val=GetCRC16(tx_buf,4+tmp);
			tx_buf[4+tmp]=crc_val>>8;
			tx_buf[5+tmp]=crc_val&0xFF;
			clear_rx_cnt(r->canal);
			switch(r->canal)
			{
				case 1:write_canal2(6+tmp);break;
				case 2:write_canal(6+tmp);break;
				case 3:write_module(6+tmp);break;
			}
			break;
		case RD_RAM:
			tx_buf[0]=r->plc_addr;
			tx_buf[1]=0xD4;
			tx_buf[2]=r->mem_addr >> 8;
			tx_buf[3]=r->mem_addr & 0xFF;
			tx_buf[4]=r->amount >> 8;
			tx_buf[5]=r->amount & 0xFF;
			crc_val=GetCRC16(tx_buf,6);
			tx_buf[6]=crc_val>>8;
			tx_buf[7]=crc_val&0xFF;
			clear_rx_cnt(r->canal);
			switch(r->canal)
			{
				case 1:write_canal2(8);break;
				case 2:write_canal(8);break;
				case 3:write_module(8);break;
			}
			break;
		case RD_US:
			tx_buf[0]=r->plc_addr;
			tx_buf[1]=0xD0;
			tx_buf[2]=r->mem_addr & 0xFF;
			tx_buf[3]=r->amount & 0xFF;
			crc_val=GetCRC16(tx_buf,4);
			tx_buf[4]=crc_val>>8;
			tx_buf[5]=crc_val&0xFF;
			clear_rx_cnt(r->canal);
			switch(r->canal)
			{
				case 1:write_canal2(6);break;
				case 2:write_canal(6);break;
				case 3:write_module(6);break;
			}
			break;
		case RD_XRAM:
			tx_buf[0]=r->plc_addr;
			tx_buf[1]=0x54;
			tx_buf[2]=r->mem_addr >> 8;
			tx_buf[3]=r->mem_addr & 0xFF;
			crc_val=GetCRC16(tx_buf,4);
			tx_buf[4]=crc_val>>8;
			tx_buf[5]=crc_val&0xFF;
			clear_rx_cnt(r->canal);
			switch(r->canal)
			{
				case 1:write_canal2(6);break;
				case 2:write_canal(6);break;
				case 3:write_module(6);break;
			}
			break;
		case RD_IO:
			tx_buf[0]=r->plc_addr;
			tx_buf[1]=0xB0;
			tx_buf[2]=r->mem_addr >> 8;
			tx_buf[3]=r->mem_addr & 0xFF;
			tx_buf[4]=r->amount >> 8;
			tx_buf[5]=r->amount & 0xFF;
			crc_val=GetCRC16(tx_buf,6);
			tx_buf[6]=crc_val>>8;
			tx_buf[7]=crc_val&0xFF;
			clear_rx_cnt(r->canal);
			switch(r->canal)
			{
				case 1:write_canal2(8);break;
				case 2:write_canal(8);break;
				case 3:write_module(8);break;
			}
			break;
	}
}

char can_check(request* r)
{
	unsigned char* rx_buf;
	rx_buf = get_can_rx_ptr(r->canal);
	if(rx_buf==0) return -1;
	switch(r->cmd)
	{
		case RD_RAM:
			if(get_rx_cnt(r->canal)==1+r->amount+2)
			{
				if(GetCRC16(rx_buf,3+r->amount)==0)	{r->rx = &rx_buf[1];return 1;}
			}
			break;
		case RD_US:
			if(get_rx_cnt(r->canal)==1+r->amount+2)
			{
				if(GetCRC16(rx_buf,3+r->amount)==0)	{r->rx = &rx_buf[1];return 1;}
			}
			break;
		case RD_XRAM:
			if(get_rx_cnt(r->canal)==1+8+2)
			{
				if(GetCRC16(rx_buf,11)==0) {r->rx = &rx_buf[1];return 1;}
			}
			break;
		case RD_IO:
			if(get_rx_cnt(r->canal)==1+r->amount+2)
			{
				if(GetCRC16(rx_buf,3+r->amount)==0)	{r->rx = &rx_buf[1];return 1;}
			}
			break;
	}
	return 0;
}

void set_disp_num(unsigned char id)
{
	unsigned char n100,n10,tmp;
	for(tmp=0;tmp<20;tmp++) obj_name[tmp]=' ';
	n100 = id/100;id-=n100*100;n10=id/10;id-=n10*10;
	obj_name[0]=n100 + '0';
	obj_name[1]=n10+'0';
	obj_name[2]=id+'0';
}

unsigned char get_disp_num(void)
{
	unsigned char n;
	n = (obj_name[0]-'0')*100 + (obj_name[1]-'0')*10 + obj_name[2];
	return n;
}
