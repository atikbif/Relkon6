#ifndef MASTER_H_
#define MASTER_H_

#define CAN_MB		3
#define CAN_PR		1
#define CAN_PC		2

#define RD_RAM 		0
#define RD_US		1
#define RD_XRAM		2
#define RD_IO		3
#define WR_RAM 		4
#define WR_US		5
#define WR_XRAM		6

typedef struct
{
	unsigned char canal;
	unsigned char cmd;
	unsigned char plc_addr;
	unsigned short mem_addr;
	unsigned short amount;
	unsigned char* rx;
	unsigned char* tx;
}request;

char test_user(unsigned char* ptr,unsigned char cnt);
void read_user(unsigned char plc_addr,unsigned char addr,unsigned char cnt);
char test_xram(unsigned char* ptr,unsigned short cnt);
void read_xram(unsigned char plc_addr,unsigned short xram_addr,unsigned short cnt);
char test_xram_51(unsigned char* ptr);
void read_xram_51(unsigned char plc_addr,unsigned short xram_addr);
void can_cmd(request* r);
char can_check(request* r);
unsigned char get_disp_num(void);
void set_disp_num(unsigned char id);

#endif /* MASTER_H_ */
