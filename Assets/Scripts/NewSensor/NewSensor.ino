/* This example shows how to use continuous mode to take
range measurements with the VL53L0X. It is based on
vl53l0x_ContinuousRanging_Example.c from the VL53L0X API.

The range readings are in units of mm. */

#include <Wire.h>
#include <VL53L0X.h>

#define XSHUT_pin1 12 
#define XSHUT_pin2 13
#define XSHUT_pin3 11
#define XSHUT_pin4 10
#define XSHUT_pin5 9
#define XSHUT_pin6 8
#define XSHUT_pin7 7
#define XSHUT_pin8 6

#define Sensor2_newAddress 42
#define Sensor3_newAddress 43
#define Sensor4_newAddress 44
#define Sensor5_newAddress 45
#define Sensor6_newAddress 46
#define Sensor7_newAddress 47
#define Sensor8_newAddress 48

const int analogInPin1 = A0;  // Analog input pin that the potentiometer is attached to
const int analogInPin2 = A1;  // Analog input pin that the potentiometer is attached to
const int analogInPin3 = A2;  // Analog input pin that the potentiometer is attached to
const int analogInPin4 = A3;  // Analog input pin that the potentiometer is attached to
const int analogInPin5 = A4;  // Analog input pin that the potentiometer is attached to
const int analogInPin6 = A5;  // Analog input pin that the potentiometer is attached to


int pot1Value = 0;        // value read from the pot
int pot2Value = 0;        // value read from the pot
int pot3Value = 0;        // value read from the pot
int pot4Value = 0;        // value read from the pot
int pot5Value = 0;        // value read from the pot
int pot6Value = 0;        // value read from the pot


VL53L0X sensor1;
VL53L0X sensor2;
VL53L0X sensor3;
VL53L0X sensor4;
VL53L0X sensor5;
VL53L0X sensor6;
VL53L0X sensor7;
VL53L0X sensor8;



void setup()
{
  pinMode(XSHUT_pin1, OUTPUT);
  pinMode(XSHUT_pin2, OUTPUT);
  pinMode(XSHUT_pin3, OUTPUT);
  pinMode(XSHUT_pin4, OUTPUT);
  pinMode(XSHUT_pin5, OUTPUT);
  pinMode(XSHUT_pin6, OUTPUT);
  pinMode(XSHUT_pin7, OUTPUT);
  pinMode(XSHUT_pin8, INPUT);
  
  Serial.begin(9600);
  
  Wire.begin();
  sensor8.setAddress(Sensor8_newAddress);
  pinMode(XSHUT_pin7,INPUT);
  delay(10);
  sensor7.setAddress(Sensor7_newAddress);
  pinMode(XSHUT_pin6,INPUT);
  delay(10);
  sensor6.setAddress(Sensor6_newAddress);
  pinMode(XSHUT_pin5,INPUT);
  delay(10);
  sensor5.setAddress(Sensor5_newAddress);
  pinMode(XSHUT_pin4,INPUT);
  delay(10);
  sensor4.setAddress(Sensor4_newAddress);
  pinMode(XSHUT_pin3,INPUT);
  delay(10);  
  sensor3.setAddress(Sensor3_newAddress);
  pinMode(XSHUT_pin2,INPUT);
  delay(10);
  sensor2.setAddress(Sensor2_newAddress);
  pinMode(XSHUT_pin1,INPUT);
  delay(10);

  sensor1.init();
  sensor2.init();
  sensor3.init();
  sensor4.init();
  sensor5.init();
  sensor6.init();
  sensor7.init();
  sensor8.init();
  
  sensor1.setTimeout(500);
  if (!sensor1.init())
  {
    Serial.println("Failed to detect and initialize sensor1!");
    while (1) {}
  }
  sensor2.setTimeout(500);
  if (!sensor2.init())
  {
    Serial.println("Failed to detect and initialize sensor2!");
    while (1) {}
  }
  sensor3.setTimeout(500);
  if (!sensor3.init())
  {
    Serial.println("Failed to detect and initialize sensor3!");
    while (1) {}
  }
   sensor4.setTimeout(500);
  if (!sensor4.init())
  {
    Serial.println("Failed to detect and initialize sensor4!");
    while (1) {}
  }
  sensor5.setTimeout(500);
  if (!sensor5.init())
  {
    Serial.println("Failed to detect and initialize sensor5!");
    while (1) {}
  }
  sensor6.setTimeout(500);
  if (!sensor6.init())
  {
    Serial.println("Failed to detect and initialize sensor6!");
    while (1) {}
  }
   sensor7.setTimeout(500);
  if (!sensor7.init())
  {
    Serial.println("Failed to detect and initialize sensor7!");
    while (1) {}
  }
  sensor8.setTimeout(500);
  if (!sensor8.init())
  {
    Serial.println("Failed to detect and initialize sensor8!");
    while (1) {}
  }
  // Start continuous back-to-back mode (take readings as
  // fast as possible).  To use continuous timed mode
  // instead, provide a desired inter-measurement period in
  // ms (e.g. sensor.startContinuous(100)).
  sensor1.startContinuous();
  sensor2.startContinuous();
  sensor3.startContinuous();
  sensor4.startContinuous();
  sensor5.startContinuous();
  sensor6.startContinuous();
  sensor7.startContinuous();
  sensor8.startContinuous();
}

void loop()
{
  //Serial.print("current dist: ");
  //Serial.print(sensor.readRangeContinuousMillimeters());
  if (sensor1.timeoutOccurred()) { Serial.print("Sensor1 TIMEOUT"); }
  if (sensor2.timeoutOccurred()) { Serial.print("Sensor2 TIMEOUT"); }
  if (sensor3.timeoutOccurred()) { Serial.print("Sensor3 TIMEOUT"); }
  if (sensor4.timeoutOccurred()) { Serial.print("Sensor4 TIMEOUT"); }
  if (sensor5.timeoutOccurred()) { Serial.print("Sensor5 TIMEOUT"); }
  if (sensor6.timeoutOccurred()) { Serial.print("Sensor6 TIMEOUT"); }
  if (sensor7.timeoutOccurred()) { Serial.print("Sensor7 TIMEOUT"); }
  if (sensor8.timeoutOccurred()) { Serial.print("Sensor8 TIMEOUT"); }

//CONTROLLER ONE  
  //Serial.println();
  //Serial.write(sensor1.getAddress());
  checkDistanceDiff(sensor1);
  //POTENTIOMETER CODE 
  // read the analog in value:
  pot1Value = analogRead(analogInPin1);
  // map it to the range of the analog out:
  int outputValue1 = map(pot1Value, 0, 1023, 0, 180);
  // change the analog out value:
  // print the results to the Serial Monitor:
  //Serial.print(" , 1");
  Serial.print(" , ");
  Serial.print(outputValue1);
  Serial.println();

//CONTROLLER TWO
  //Serial.write(sensor2.getAddress());
  checkDistanceDiff(sensor2);
  //POTENTIOMETER CODE for number 2
  // read the analog in value:
  pot2Value = analogRead(analogInPin2);
  // map it to the range of the analog out:
  int outputValue2 = map(pot2Value, 0, 1023, 0, 180);
  // change the analog out value:
  // print the results to the Serial Monitor:
  //Serial.print(" , 2");
  Serial.print(" , ");
  Serial.print(outputValue2);
  Serial.println();

//CONTROLLER THREE  
  checkDistanceDiff(sensor3);
  pot3Value = analogRead(analogInPin3);
  int outputValue3 = map(pot3Value, 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 3");
  Serial.print(" , ");
  Serial.print(outputValue3);
  Serial.println();
  
//CONTROLLER FOUR  
  checkDistanceDiff(sensor4);
  pot4Value = analogRead(analogInPin4);
  int outputValue4 = map(pot4Value, 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 4");
  Serial.print(" , ");
  Serial.print(outputValue4);
  Serial.println();
  
//CONTROLLER FIVE 
  checkDistanceDiff(sensor5);
  pot5Value = analogRead(analogInPin5);
  int outputValue5 = map(pot5Value, 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 5");
  Serial.print(" , ");
  Serial.print(outputValue5);
  Serial.println();

//CONTROLLER SIX  
  checkDistanceDiff(sensor6);
  pot6Value = analogRead(analogInPin6);
  int outputValue6 = map(pot6Value, 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 6");
  Serial.print(" , ");
  Serial.print(outputValue6);
  Serial.println();

//CONTROLLER SEVEN  
//  checkDistanceDiff(sensor7);
//  pot7Value = analogRead(analogInPin7);
//  int outputValue7 = map(pot7Value, 0, 1023, 0, 180);
//  // print the results to the Serial Monitor:
//  //Serial.print(" , 7");
//  Serial.print(" , ");
//  Serial.print(outputValue7);
//  Serial.println();
  
//CONTROLLER EIGHT 
//  checkDistanceDiff(sensor8);
//  pot8Value = analogRead(analogInPin8);
//  int outputValue8 = map(pot8Value, 0, 1023, 0, 180);
//  // print the results to the Serial Monitor:
//  //Serial.print(" , 8");
//  Serial.print(" , ");
//  Serial.print(outputValue8);
//  Serial.println();

  // wait 2 milliseconds before the next loop for the analog-to-digital
  // converter to settle after the last reading:
  //delay(2);
  
  Serial.flush();
  delay(20);
  

}

void checkDistanceDiff(VL53L0X sensor)

{
  static int previousDist = 0;
  int currentDist = sensor.readRangeContinuousMillimeters();
  int diff= currentDist - previousDist;
  previousDist = currentDist;
  int address = sensor.getAddress();
  //Serial.println();
  Serial.print(address);
  Serial.print(" , ");
  Serial.print(diff);


  

  //return sensor.getAddress();
}
