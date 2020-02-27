/* This example shows how to use continuous mode to take
range measurements with the VL53L0X. It is based on
vl53l0x_ContinuousRanging_Example.c from the VL53L0X API.

The range readings are in units of mm. */

#include <Wire.h>
#include <VL53L0X.h>

#define XSHUT_pin1 12 
#define XSHUT_pin2 13

#define Sensor2_newAddress 42


VL53L0X sensor1;
VL53L0X sensor2;


void setup()
{
  pinMode(XSHUT_pin1, OUTPUT);
  
  Serial.begin(9600);
  Wire.begin();

  sensor2.setAddress(Sensor2_newAddress);
  pinMode(XSHUT_pin1,INPUT);
  delay(10);

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
  //Serial.flush();
  //delay(20);  
  //Serial.write(sensor2.getAddress());
  checkDistanceDiff(sensor2);

}

void checkDistanceDiff(VL53L0X sensor)

{
  static int previousDist = 0;
  int currentDist = sensor.readRangeContinuousMillimeters();
  int diff= currentDist - previousDist;
  previousDist = currentDist;
  int address = sensor.getAddress();
  Serial.println();
  Serial.print(address);
  Serial.print(" , ");
  Serial.print(diff);

  Serial.flush();
  delay(20);
  

  //return sensor.getAddress();
}
