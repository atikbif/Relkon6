#ifndef PULT_DEF_H_
#define PULT_DEF_H_

#define st1	_Sys.S1
#define st2	_Sys.S2
#define st3	_Sys.S3
#define st4	_Sys.S4

#define Z30	_Sys.S1
#define Z31	_Sys.S2
#define Z32	_Sys.S3
#define Z33	_Sys.S4

#define     Z50     key
#define     Z40     led

#define led_1   (*(led_ptr+0))
#define led_2   (*(led_ptr+1))
#define led_3   (*(led_ptr+2))
#define led_4   (*(led_ptr+3))
#define led_5   (*(led_ptr+4))
#define led_6   (*(led_ptr+5))
#define led_7   (*(led_ptr+6))
#define led_8   (*(led_ptr+7))

#define key_1   ((unsigned char)(*(key_ptr+0)))
#define key_2   ((unsigned char)(*(key_ptr+1)))
#define key_3   ((unsigned char)(*(key_ptr+2)))
#define key_4   ((unsigned char)(*(key_ptr+3)))
#define key_5   ((unsigned char)(*(key_ptr+4)))
#define key_6   ((unsigned char)(*(key_ptr+5)))
#define key_7   ((unsigned char)(*(key_ptr+6)))
#define key_8   ((unsigned char)(*(key_ptr+7)))

#endif /* PULT_DEF_H_ */
