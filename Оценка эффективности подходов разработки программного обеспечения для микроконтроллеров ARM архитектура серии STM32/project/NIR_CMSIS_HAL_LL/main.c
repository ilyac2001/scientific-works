#include <stm32f072xb.h>
int main (void)
{
	RCC->AHBENR |=  RCC_AHBENR_GPIOBEN;	
	GPIOB->MODER |= GPIO_MODER_MODER4_0;
	unsigned short counter = 10000;
	while(counter--){
		GPIOB->ODR ^= GPIO_ODR_4;
	}
	while(1) { }
}
