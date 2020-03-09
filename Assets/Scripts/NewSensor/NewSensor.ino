/* This example shows how to use continuous mode to take
range measurements with the VL53L0X. It is based on
vl53l0x_ContinuousRanging_Example.c from the VL53L0X API.

The range readings are in units of mm. */

#include <Wire.h>
#include <VL53L0X.h>

#define XSHUT_pin1 12 
#define XSHUT_pin2 13

#define Sensor2_newAddress 42

const int analogInPin1 = A0;  // Analog input pin that the potentiometer is attached to
const int analogInPin2 = A1;  // Analog input pin that the potentiometer is attached to

int pot1Value = 0;        // value read from the pot
int pot2Value = 0;        // value read from the pot



VL53L0X sensor1;
VL53L0X sensor2;


void setup()
{
  pinMode(XSHUT_pin1, OUTPUT);
  
  Serial.begin(9600);
  Wire.begin();

  sensor2.setAddress(Sensor2_newAddress);
  pinMode(XSHUT_pin1,INPUT);
  //delay(10);

  sensor1.init();
  sensor2.init();
  
  sensor1.setTimeout(500);
  if (!sensor1.init())
  {
    Serial.println("Failed to detect and initialize sensor!");
    while (1) {}
  }


  sensor2.setTimeout(500);
  if (!sensor2.init())
  {
    Serial.println("Failed to detect and initialize sensor!");
    while (1) {}
  }
  // Start continuous back-to-back mode (take readings as
  // fast as possible).  To use continuous timed mode
  // instead, provide a desired inter-measurement period in
  // ms (e.g. sensor.startContinuous(100)).
  sensor1.startContinuous();
  sensor2.startContinuous();
}

void loop()
{
  //Serial.print("current dist: ");
  //Serial.print(sensor.readRangeContinuousMillimeters());
  if (sensor1.timeoutOccurred()) { Serial.print("Sensor1 TIMEOUT"); }
  if (sensor2.timeoutOccurred()) { Serial.print("Sensor2 TIMEOUT"); }

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

  // wait 2 milliseconds before the next loop for the analog-to-digital
  // converter to settle after the last reading:
  //delay(2);
  //Serial.flush();
  //delay(20);  
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
