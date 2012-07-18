/*
 * k_main.c
 *
 *  Created on: Mar 6, 2012
 *      Author: �����
 */

#include "k_main.h"
#include "heth.h"

#include "hcanal.h"

#include "k_icmp.h"
#include "k_arp.h"
#include "k_udp.h"
#include "k_ip.h"
#include "k_mac.h"
#include "k_tcp.h"

extern unsigned char canal_tx_buf[512];

static mac_pkt* m_pkt;
static arp_pkt* arp_p;
static ip_pkt* ip_p;
static icmp_pkt* icmp_p;
static udp_pkt* udp_p;
static tcp_pkt* tcp_p;

eth_buf* buf;

static unsigned short k_tmr=0;

extern unsigned short eth_pkt_cnt;

void konlite_call(void)
{
	unsigned short tmp;
	k_tmr++;
	for(tmp=0;tmp<ETH_RXBUFNB;tmp++)
	{
		buf = get_mac_data(tmp); // ����� �������� ������
		if(buf->len)
		{
			m_pkt=get_mac_pkt(buf); // ������������ ��������� ��� ������
			if(check_mac(m_pkt)) // �������� ��� ������ ����������
			{

				switch(get_eth_type(m_pkt))
				{
					case IP_TYPE:
						ip_p=ip_rcv(m_pkt);
						if(check_ip_req(ip_p))
						{
							if(check_mask(ip_p->ip_s)==0) {set_gate_cmd();} // ������ ������� �� ����� �������
							else{clr_gate_cmd();add_arp_tab(ip_p->ip_s,m_pkt->mac_s);} // ������ � �������� �������
							switch(get_ip_type(ip_p))
							{
								case ICMP_TYPE:
									icmp_p=icmp_rcv(ip_p);
									if(check_ping(icmp_p)) {ping_echo(icmp_p,ip_p);}
									break;
								case UDP_TYPE:
									udp_p=udp_rcv(ip_p);
									portudp_scan(udp_p,ip_p);
									break;
								case TCP_TYPE:
									tcp_p=tcp_rcv(ip_p);
									porttcp_scan(tcp_p,ip_p);
									break;
							}
						}
						break;
					case ARP_TYPE:
						arp_p=arp_rcv(m_pkt); // ������������ ��������� arp ������
						if(check_arp_req(arp_p))	// arp ������
						{
							add_arp_tab(arp_p->ip_s,arp_p->mac_s);
							arp_answer(arp_p);
						}else
						if(check_arp_answer_gate(arp_p))	// arp ����� �� �����
						{
							set_gate_mac(arp_p->mac_s);
						}
						break;
					case UNDEF_TYPE:
						break;
				}
			}
			unlock_mac_rx(tmp);
		}
	}
	if(k_tmr % GATE_PER == 0) get_gate_mac();	// ������������� ������ mac �����
}
