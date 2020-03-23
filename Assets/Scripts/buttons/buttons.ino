int LED1 = 13;
int BUTTON1 = 12;
int LED2 = 11;
int BUTTON2 = 10;
int LED3 = 9;
int BUTTON3 = 8;
int LED4 = 7;
int BUTTON4 = 6;
int LED5 = 5;
int BUTTON5 = 4;
int LED6 = 3;
int BUTTON6 = 2;


void setup()
{
  Serial.begin(9600);
  pinMode(LED1,OUTPUT); 
  pinMode(BUTTON1,INPUT); 
  pinMode(LED2,OUTPUT); 
  pinMode(BUTTON2,INPUT);
  pinMode(LED3,OUTPUT); 
  pinMode(BUTTON3,INPUT);
  pinMode(LED4,OUTPUT); 
  pinMode(BUTTON4,INPUT);
  pinMode(LED5,OUTPUT); 
  pinMode(BUTTON5,INPUT);
  pinMode(LED6,OUTPUT); 
  pinMode(BUTTON6,INPUT);
}

void loop()
{
// SNOWBALL ONE
  static unsigned char led1State = LOW;
  static unsigned char button1State = LOW;
  static unsigned char lastButton1State = LOW;
  static unsigned long led1CameOn = 0;
 
  // If the LED has been on for at least 3 seconds then turn it off.
  if(led1State == HIGH)
  {
    if(millis()-led1CameOn > 3000)
    {
      digitalWrite(LED1,LOW);
      led1State = LOW;
    }
  }

  // If the button's state has changed, then turn the LED on IF it is not on already.
  button1State = digitalRead(BUTTON1);
  if(button1State != lastButton1State)
  {
    lastButton1State = button1State;
    if((button1State == HIGH) && (led1State == LOW))
    {
      digitalWrite(LED1,HIGH);
      led1State = HIGH;
      led1CameOn = millis();
      Serial.println("SNOWBALL1 THROWN");
      Serial.flush();
      delay(2);
    }
  }

// SNOWBALL TWO
  static unsigned char led2State = LOW;
  static unsigned char button2State = LOW;
  static unsigned char lastButton2State = LOW;
  static unsigned long led2CameOn = 0;
 
  // If the LED has been on for at least 3 seconds then turn it off.
  if(led2State == HIGH)
  {
    if(millis()-led2CameOn > 3000)
    {
      digitalWrite(LED2,LOW);
      led2State = LOW;
    }
  }

  // If the button's state has changed, then turn the LED on IF it is not on already.
  button2State = digitalRead(BUTTON2);
  if(button2State != lastButton2State)
  {
    lastButton2State = button2State;
    if((button2State == HIGH) && (led2State == LOW))
    {
      digitalWrite(LED2,HIGH);
      led2State = HIGH;
      led2CameOn = millis();
      Serial.println("SNOWBALL2 THROWN");
      Serial.flush();
      delay(2);
    }
  }

 // SNOWBALL THREE 
  static unsigned char led3State = LOW;
  static unsigned char button3State = LOW;
  static unsigned char lastButton3State = LOW;
  static unsigned long led3CameOn = 0;
 
  // If the LED has been on for at least 3 seconds then turn it off.
  if(led3State == HIGH)
  {
    if(millis()-led3CameOn > 3000)
    {
      digitalWrite(LED3,LOW);
      led3State = LOW;
    }
  }

  // If the button's state has changed, then turn the LED on IF it is not on already.
  button3State = digitalRead(BUTTON3);
  if(button3State != lastButton3State)
  {
    lastButton3State = button3State;
    if((button3State == HIGH) && (led3State == LOW))
    {
      digitalWrite(LED3,HIGH);
      led3State = HIGH;
      led3CameOn = millis();
      Serial.println("SNOWBALL3 THROWN");
      Serial.flush();
      delay(2);
    }
  }

// SNOWBALL FOUR
  static unsigned char led4State = LOW;
  static unsigned char button4State = LOW;
  static unsigned char lastButton4State = LOW;
  static unsigned long led4CameOn = 0;
 
  // If the LED has been on for at least 3 seconds then turn it off.
  if(led4State == HIGH)
  {
    if(millis()-led4CameOn > 3000)
    {
      digitalWrite(LED4,LOW);
      led4State = LOW;
    }
  }

  // If the button's state has changed, then turn the LED on IF it is not on already.
  button4State = digitalRead(BUTTON4);
  if(button4State != lastButton4State)
  {
    lastButton4State = button4State;
    if((button4State == HIGH) && (led4State == LOW))
    {
      digitalWrite(LED4,HIGH);
      led4State = HIGH;
      led4CameOn = millis();
      Serial.println("SNOWBALL4 THROWN");
      Serial.flush();
      delay(2);
    }
  }

// SNOWBALL FIVE
  static unsigned char led5State = LOW;
  static unsigned char button5State = LOW;
  static unsigned char lastButton5State = LOW;
  static unsigned long led5CameOn = 0;
 
  // If the LED has been on for at least 3 seconds then turn it off.
  if(led5State == HIGH)
  {
    if(millis()-led5CameOn > 3000)
    {
      digitalWrite(LED5,LOW);
      led5State = LOW;
    }
  }

  // If the button's state has changed, then turn the LED on IF it is not on already.
  button5State = digitalRead(BUTTON5);
  if(button5State != lastButton5State)
  {
    lastButton5State = button5State;
    if((button5State == HIGH) && (led5State == LOW))
    {
      digitalWrite(LED5,HIGH);
      led5State = HIGH;
      led5CameOn = millis();
      Serial.println("SNOWBALL5 THROWN");
      Serial.flush();
      delay(2);
    }
  }

//SNOWBALL SIX
  static unsigned char led6State = LOW;
  static unsigned char button6State = LOW;
  static unsigned char lastButton6State = LOW;
  static unsigned long led6CameOn = 0;
 
  // If the LED has been on for at least 5 seconds then turn it off.
  if(led6State == HIGH)
  {
    if(millis()-led6CameOn > 3000)
    {
      digitalWrite(LED6,LOW);
      led6State = LOW;
    }
  }

  // If the button's state has changed, then turn the LED on IF it is not on already.
  button6State = digitalRead(BUTTON6);
  if(button6State != lastButton6State)
  {
    lastButton6State = button6State;
    if((button6State == HIGH) && (led6State == LOW))
    {
      digitalWrite(LED6,HIGH);
      led6State = HIGH;
      led6CameOn = millis();
      Serial.println("SNOWBALL6 THROWN");
      Serial.flush();
      delay(2);
    }
  }

}
