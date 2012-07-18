#ifndef INOUT_DEF_H_
#define INOUT_DEF_H_

#ifndef BIT_STRUCT
	#define BIT_STRUCT
	typedef struct IO_DEF
	{
		unsigned bit0:1;
		unsigned bit1:1;
		unsigned bit2:1;
		unsigned bit3:1;
		unsigned bit4:1;
		unsigned bit5:1;
		unsigned bit6:1;
		unsigned bit7:1;
	} __attribute__((__may_alias__)) __attribute__((aligned(4))) iostruct;
#endif

#define IN0 				_Sys_IN[0]
#define IN1 				_Sys_IN[1]
#define IN2 				_Sys_IN[2]
#define IN3 				_Sys_IN[3]
#define DIN4 				_Sys_IN[4]
#define DIN5 				_Sys_IN[5]

#define OUT0 				_Sys_OUT[0]
#define OUT1 				_Sys_OUT[1]
#define OUT2 				_Sys_OUT[2]
#define OUT3 				_Sys_OUT[3]
#define DOUT4 				_Sys_OUT[4]
#define DOUT5 				_Sys_OUT[5]

#define ADH1				(_Sys_ADC[0]>>8)
#define ADH2				(_Sys_ADC[1]>>8)
#define ADH3				(_Sys_ADC[2]>>8)

#define ADC4 				_Sys_ADC[3]
#define ADH4				(_Sys_ADC[3]>>8)
#define ADC5  				_Sys_ADC[4]
#define ADH5				(_Sys_ADC[4]>>8)
#define ADC6				_Sys_ADC[5]
#define ADH6				(_Sys_ADC[5]>>8)
#define ADC7				_Sys_ADC[6]
#define ADH7				(_Sys_ADC[6]>>8)
#define ADC8				_Sys_ADC[7]
#define ADH8				(_Sys_ADC[7]>>8)

#define AI1                (_Sys_ADC[0]>>4)
#define AI2                (_Sys_ADC[1]>>4)
#define AI3                (_Sys_ADC[2]>>4)
#define AI4                (_Sys_ADC[3]>>4)
#define AI5                (_Sys_ADC[4]>>4)
#define AI6                (_Sys_ADC[5]>>4)
#define AI7                (_Sys_ADC[6]>>4)
#define AI8                (_Sys_ADC[7]>>4)

#define DAC1 				_Sys_DAC[0]
#define DAH1				(_Sys_DAC[0]>>8)
#define DAC2  				_Sys_DAC[1]
#define DAH2				(_Sys_DAC[1]>>8)
#define DAC3				_Sys_DAC[2]
#define DAH3				(_Sys_DAC[2]>>8)
#define DAC4				_Sys_DAC[3]
#define DAH4				(_Sys_DAC[3]>>8)

#endif /* INOUT_DEF_H_ */
