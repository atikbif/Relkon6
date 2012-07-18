/*
 * main.c
 *
 *  Relkon ver 1.0
 *  Author: �����
 *
 */

#include "FreeRTOS.h"
#include "main.h"
#include "htime.h"
#include "lcd.h"
#include "inout.h"
#include "hain.h"
#include "hdac.h"
#include "fc_u.h"
#include "fram.h"
#include "canal.h"

#include "eth_task.h"
#include "sdcard.h"
#include "wifi.h"

#define SYS_STARTUP		0							// ��������� RCC ��� �������� � startup ����� (1/0)
#define FRAM_YEAR		1							// ��� �������� ���� ������������ FRAM
#define ASSERT_EXT		0							// ������ �� assert � head �����

plc_stat _Sys;										// ��������� � ���������� ����������� �����������
volatile unsigned char emu_mode=0;					// ���������� ����� ��������
volatile unsigned long _SysIdleTmr=0,_SysWaitTmr=0;	// ���������� ��� ����������� �������� ����������
volatile unsigned long _SysTmr=0,_SysRealTmr=0;
unsigned long max_load=0;							// ������������ �������� ���������� � ������� ���������
unsigned long cpu=0;
volatile unsigned long adc_sum[8]={0,0,0,0,0,0,0,0};// ������ ��� ���������� ���
extern volatile unsigned short _Sys_ADC[8];

extern unsigned char ssid_name[32];					// ��������� ��� ���� WIFI
extern unsigned char wifi_password[32];
extern unsigned char wifi_type;
extern unsigned char wifi_ip[4];

static unsigned char ip_addr[4]={0,0,0,0};			// IP � MAC ��� ���� ethernet
static unsigned char mac_addr[6]={0,0,0,0,0,0};

unsigned char pult_dis,sd_dis;						// ����� ���������� ������ � sd �����

extern volatile unsigned short canalpu_rx_cnt;
extern volatile unsigned short pu_tmr;

extern const unsigned short S4_max;					// ����������� ���������� ����� ������ ������
extern volatile unsigned long tcp_tmr;

static void prvFlashTask( void *pvParameters );

portTickType FLLastExecutionTime;

#if FRAM_YEAR
	extern tm times;
#endif

/**
**===========================================================================
**
**  Abstract: main program
**
**===========================================================================
*/
int main(void)
{
#if SYS_STARTUP == 0
  SystemInit();
#endif

  nvic_init();			// ��������� �������� ����������
  led_init();			// ������������� ����������, ������������� ������ ����������
  init_calendar();		// ������������� ���������
  _SPI_init();			// SPI ��� ������� FRAM

  // ������ ��������� ���������

  read_data(0x7B,0x00,128,(unsigned char*)&_Sys.FR.b1[0]);
  read_data(0x7B,0x80,128,(unsigned char*)&_Sys.FR.b1[0x80]);
  read_data(0x7C,0x00,128,(unsigned char*)&_Sys.FR.b1[0x100]);
  read_data(0x7C,0x80,128,(unsigned char*)&_Sys.FR.b1[0x180]);
  read_data(0x7D,0x00,128,(unsigned char*)&_Sys.FR.b1[0x200]);
  read_data(0x7D,0x80,128,(unsigned char*)&_Sys.FR.b1[0x280]);
  read_data(0x7E,0x00,128,(unsigned char*)&_Sys.FR.b1[0x300]);
  read_data(0x7E,0x80,128,(unsigned char*)&_Sys.FR.b1[0x380]);

  // ������ ��������� �������� �����������

  //read_data(0x7F,0x58,1,&rf_ch);

  read_data(0x7F,0x00,6,(unsigned char*)&_Sys.Mem.b1[0]);

  _Sys.Adr=_Sys.Mem.b1[0];
  if((_Sys.Adr==0)||(_Sys.Adr==0xFF)) _Sys.Adr=0x01;

  _Sys.Can1_Baudrate=_Sys.Mem.b1[1];
  if(_Sys.Can1_Baudrate>5) _Sys.Can1_Baudrate=5;
  _Sys.Can1_Type = _Sys.Mem.b1[2];
  if(_Sys.Can1_Type>1) _Sys.Can1_Type=0;

  _Sys.Can2_Baudrate=_Sys.Mem.b1[3];
  if(_Sys.Can2_Baudrate>5) _Sys.Can2_Baudrate=5;
  _Sys.Can2_Type = _Sys.Mem.b1[4];
  if(_Sys.Can2_Type>1) _Sys.Can2_Type=0;

  emu_mode = _Sys.Mem.b1[5];
  if(emu_mode>2) emu_mode=0;

  read_data(0x7F,0x4C,4,ip_addr);set_ip(ip_addr);
  read_data(0x7F,0x50,6,mac_addr);set_mac(mac_addr);
#if FRAM_YEAR
  read_data(0x7F,0x56,1,(unsigned char*)&times.year);
  if(times.year>99) times.year=0;
#endif

  read_data(0x7F,0x57,1,(unsigned char*)&pult_dis);
  read_data(0x7F,0x59,1,(unsigned char*)&sd_dis);

  read_data(0x7F,0x5A,32,ssid_name);
  read_data(0x7F,0x7A,32,wifi_password);
  read_data(0x7F,0x9A,1,&wifi_type);
  read_data(0x7F,0x9B,4,wifi_ip);

  // ��������� �������� ������� ����� ������
  _Sys.S1=0x00;_Sys.S2=0x00;_Sys.S3=0x00;_Sys.S4=0x00;

  // ��������� �������������� ������
  xTaskCreate( R1Task, ( signed portCHAR * ) "R1", configMINIMAL_STACK_SIZE, NULL, R1_TASK_PRIORITY, NULL );
  xTaskCreate( R5Task, ( signed portCHAR * ) "R5", configMINIMAL_STACK_SIZE, NULL, R5_TASK_PRIORITY, NULL );
  xTaskCreate( R10Task, ( signed portCHAR * ) "R10", configMINIMAL_STACK_SIZE, NULL, R10_TASK_PRIORITY, NULL );
  xTaskCreate( R100Task, ( signed portCHAR * ) "R100", configMINIMAL_STACK_SIZE, NULL, R100_TASK_PRIORITY, NULL );
  xTaskCreate( R1000Task, ( signed portCHAR * ) "R1000", configMINIMAL_STACK_SIZE, NULL, R1000_TASK_PRIORITY, NULL );
  // ��������� ��������������� ������ (���������,������ ���,...)
  xTaskCreate( prvFlashTask, (signed portCHAR *) "Flash", configMINIMAL_STACK_SIZE, NULL, mainFLASH_TASK_PRIORITY, NULL );
  // ����������� ����� ��� ���������������� �����
  if(pult_dis!=0x31) xTaskCreate( LCDTask, ( signed portCHAR * ) "Lcd", configMINIMAL_STACK_SIZE, NULL, LCD_TASK_PRIORITY, NULL );
    else xTaskCreate( PultCanTask, ( signed portCHAR * ) "Lcd", configMINIMAL_STACK_SIZE, NULL, Canal_TASK_PRIORITY, NULL );
  // ������ ��������� ������/������� ������� ������ ����������
  xTaskCreate( InOutTask, ( signed portCHAR * ) "InOut", configMINIMAL_STACK_SIZE, NULL, InOut_TASK_PRIORITY, NULL );
  // ��� �������� ���������������� ������ RS485
  if(_Sys.Can1_Type==0) xTaskCreate( BinCanTask, ( signed portCHAR * ) "Canal", configMINIMAL_STACK_SIZE, NULL, Canal_TASK_PRIORITY, NULL );
  else xTaskCreate( AsciiCanTask, ( signed portCHAR * ) "Canal", configMINIMAL_STACK_SIZE, NULL, Canal_TASK_PRIORITY, NULL );
  if(_Sys.Can2_Type==0) xTaskCreate( BinCan2Task, ( signed portCHAR * ) "Canal2", configMINIMAL_STACK_SIZE, NULL, Canal_TASK_PRIORITY, NULL );
  else xTaskCreate( AsciiCan2Task, ( signed portCHAR * ) "Canal2", configMINIMAL_STACK_SIZE, NULL, Canal_TASK_PRIORITY, NULL );
  // WIFI ������
  if(wifi_ip[0])
  {
    xTaskCreate( WIFITask, ( signed portCHAR * ) "Wifi", configMINIMAL_STACK_SIZE, NULL, WF_TASK_PRIORITY, NULL );
    xTaskCreate( SCAN_WIFITask, ( signed portCHAR * ) "SCANWifi", configMINIMAL_STACK_SIZE, NULL, WF_TASK_PRIORITY+1, NULL );
  }
  // ��������� ethernet
  if(ip_addr[0]) xTaskCreate( EthTask, ( signed portCHAR * ) "Eth", configMINIMAL_STACK_SIZE, NULL, ETH_TASK_PRIORITY, NULL );
  // ������ ������ � sd ������
  if(sd_dis==0x31) xTaskCreate( ArchiveTask, ( signed portCHAR * ) "SD_Task", configMINIMAL_STACK_SIZE, NULL, SD_TASK_PRIORITY, NULL );

  Relkon_init(); // ���� ������������� - ����������� ������� ������� (������ #init �������)

  init_timer();	// ���������� ������ ��� ����������� �������� ����������
  vTaskStartScheduler();  // ����� ������������ �����
  while (1){}
}

void vApplicationTickHook( void )	// ���������� ������ ������������
{
	_SysTmr++;tcp_tmr++;
	if (_Sys.S4 >= S4_max) _Sys.S4 = S4_max - 1;	// ����������� ������ ������ ������
	if(canalpu_rx_cnt) pu_tmr++;else pu_tmr=0;
	if(_SysTmr==1000)								// �������� �������� ����������
	{
	    _SysTmr=0;
	    if(_SysWaitTmr>100000) _SysWaitTmr=100000;
	    _SysRealTmr=1000-_SysWaitTmr/100;
	    if(_SysRealTmr>999) _SysRealTmr=999;
	    if(_SysRealTmr>max_load) max_load=_SysRealTmr;
	    _SysWaitTmr=0;
	}
}

void vApplicationIdleHook(void)	// ���������� � ����������� �������� �����
{
	_SysIdleTmr=get_idle_tmr();
}

// ��������������� ��������� ������
static void prvFlashTask( void *pvParameters )
{
    static unsigned long s_tmr=0;
    unsigned char tmp;
    init_adc();init_dac();		// ������������� ��� � ���
    update_code();				// ���������� �������� ����� � FRAM
    FLLastExecutionTime = xTaskGetTickCount();
    for( ;; )
	{
    	// ������ ���
    	if(emu_mode==0){for(tmp=0;tmp<8;tmp++) adc_sum[tmp]+=get_adc(tmp);
        if(s_tmr % 16 == 0) for(tmp=0;tmp<8;tmp++) {_Sys_ADC[tmp]=adc_sum[tmp];adc_sum[tmp]=0;}}
    	s_tmr++;
    	if((s_tmr%64)==0) {get_time();toggle_led();}	// ���������� �������� ������� � ������� ����������
        vTaskDelayUntil( &FLLastExecutionTime, mainFLASH_DELAY );
	}
}

#if ASSERT_EXT == 0
/* Minimal __assert_func used by the assert() macro */
void __assert_func(const char *file, int line, const char *func, const char *failedexpr)
{
  while(1)
  {}
}

/* Minimal __assert() uses __assert__func() */
void __assert(const char *file, int line, const char *failedexpr)
{
   __assert_func (file, line, NULL, failedexpr);
}
#endif
