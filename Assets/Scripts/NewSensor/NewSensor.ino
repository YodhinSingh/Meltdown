#include <Wire.h>
#include <VL53L0X.h>

#define XSHUT_pin1 13 
#define XSHUT_pin2 12
#define XSHUT_pin3 11
#define XSHUT_pin4 10
#define XSHUT_pin5 9
#define XSHUT_pin6 8
#define XSHUT_pin7 4
#define XSHUT_pin8 3

#define Sensor2_newAddress 42
#define Sensor3_newAddress 43
#define Sensor4_newAddress 44
#define Sensor5_newAddress 45
#define Sensor6_newAddress 46
#define Sensor7_newAddress 47
#define Sensor8_newAddress 48

const int totalChannels = 8;

int addressA = 5;
int addressB = 6;
int addressC = 7;

int A = 0;
int B = 0;
int C = 0;

int pot1Value = 0;        // value read from the pot
int pot2Value = 0;        // value read from the pot
int pot3Value = 0;        // value read from the pot
int pot4Value = 0;        // value read from the pot
int pot5Value = 0;        // value read from the pot
int pot6Value = 0;        // value read from the pot
int pot7Value = 0;        // value read from the pot
int pot8Value = 0;        // value read from the pot


int potentiometers[8] = {pot1Value, pot2Value, pot3Value, pot4Value,
                            pot5Value, pot6Value, pot7Value, pot8Value};


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

  pinMode(addressA, OUTPUT);
  pinMode(addressB, OUTPUT);
  pinMode(addressC, OUTPUT);

  pinMode(A0, INPUT);

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
  if (!sensor1.init()){
    Serial.println("Failed to detect and initialize sensor1!");
    while (1) {}
  }else{Serial.println("sensor1 initialized");}
  sensor2.setTimeout(500);
  if (!sensor2.init()){
    Serial.println("Failed to detect and initialize sensor2!");
    while (1) {}
  }else{Serial.println("sensor2 initialized");}
  sensor3.setTimeout(500);
  if (!sensor3.init()){
    Serial.println("Failed to detect and initialize sensor3!");
    while (1) {}
  }else{Serial.println("sensor3 initialized");}
   sensor4.setTimeout(500);
  if (!sensor4.init()){
    Serial.println("Failed to detect and initialize sensor4!");
    while (1) {}
  }else{Serial.println("sensor4 initialized");}
  sensor5.setTimeout(500);
  if (!sensor5.init()){
    Serial.println("Failed to detect and initialize sensor5!");
    while (1) {}
  }else{Serial.println("sensor5 initialized");}
  sensor6.setTimeout(500);
  if (!sensor6.init()){
    Serial.println("Failed to detect and initialize sensor6!");
    while (1) {}
  }else{Serial.println("sensor6 initialized");}
   sensor7.setTimeout(500);
  if (!sensor7.init()){
    Serial.println("Failed to detect and initialize sensor7!");
    while (1) {}
  }else{Serial.println("sensor7 initialized");}
   sensor8.setTimeout(500);
  if (!sensor8.init()){
    Serial.println("Failed to detect and initialize sensor8!");
    while (1) {}
  }
  else{Serial.println("sensor8 initialized");}
  
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
  for (int i=0; i<totalChannels; i++){
    A = bitRead(i,0); //take first bit from binary value of i channel. 
    B = bitRead(i,1); //take second bit from binary value of i channel.
    C = bitRead(i,2); //take third bit from value of i channel.

    digitalWrite(addressA, A);
    digitalWrite(addressB, B);
    digitalWrite(addressC, C);    

    potentiometers[i] = analogRead(A0);
<<<<<<< HEAD
    Serial.println();
    //Read and print value
    Serial.print("Channel ");
    Serial.print(i);
    Serial.print(" value: ");
    Serial.println(analogRead(A0));

    }


  if (sensor1.timeoutOccurred()) { Serial.print("Sensor1 TIMEOUT"); }
  if (sensor2.timeoutOccurred()) { Serial.print("Sensor2 TIMEOUT"); }
  if (sensor3.timeoutOccurred()) { Serial.print("Sensor3 TIMEOUT"); }
  if (sensor4.timeoutOccurred()) { Serial.print("Sensor4 TIMEOUT"); }
  if (sensor5.timeoutOccurred()) { Serial.print("Sensor5 TIMEOUT"); }
  if (sensor6.timeoutOccurred()) { Serial.print("Sensor6 TIMEOUT"); }
  if (sensor7.timeoutOccurred()) { Serial.print("Sensor7 TIMEOUT"); }
  if (sensor8.timeoutOccurred()) { Serial.print("Sensor8 TIMEOUT"); }

//CONTROLLER ONE  
  checkDistanceDiff(sensor1);
  //POTENTIOMETER CODE 
  // read the analog in value:
  // map it to the range of the analog out:
  int outputValue1 = map(potentiometers[0], 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 1");
  Serial.print(" , ");
  Serial.print(outputValue1);
  Serial.println();

//CONTROLLER TWO
  checkDistanceDiff(sensor2);
  //POTENTIOMETER CODE for number 2
  // read the analog in value:
  // map it to the range of the analog out:
  int outputValue2 = map(potentiometers[1], 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 2");
  Serial.print(" , ");
  Serial.print(outputValue2);
  Serial.println();

//CONTROLLER THREE  
  checkDistanceDiff(sensor3);
  int outputValue3 = map(potentiometers[2], 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 3");
  Serial.print(" , ");
  Serial.print(outputValue3);
  Serial.println();
  
//CONTROLLER FOUR  
  checkDistanceDiff(sensor4);
  int outputValue4 = map(potentiometers[3], 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 4");
  Serial.print(" , ");
  Serial.print(outputValue4);
  Serial.println();
  
//CONTROLLER FIVE 
  checkDistanceDiff(sensor5);
  int outputValue5 = map(potentiometers[4], 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 5");
  Serial.print(" , ");
  Serial.print(outputValue5);
  Serial.println();

//CONTROLLER SIX  
  checkDistanceDiff(sensor6);
  int outputValue6 = map(potentiometers[5], 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 6");
  Serial.print(" , ");
  Serial.print(outputValue6);
  Serial.println();

//CONTROLLER SEVEN  
  checkDistanceDiff(sensor7);
  int outputValue7 = map(potentiometers[6], 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 7");
  Serial.print(" , ");
  Serial.print(outputValue7);
  Serial.println();
  
//CONTROLLER EIGHT 
  checkDistanceDiff(sensor8);
  int outputValue8 = map(potentiometers[7], 0, 1023, 0, 180);
  // print the results to the Serial Monitor:
  //Serial.print(" , 8");
  Serial.print(" , ");
  Serial.print(outputValue8);
  Serial.println();
  
  Serial.flush();
  delay(20);
  

}

void checkDistanceDiff(VL53L0X sensor){
  static int previousDist = 0;
  int currentDist = sensor.readRangeContinuousMillimeters();
  int diff= currentDist - previousDist;
  previousDist = currentDist;
  int address = sensor.getAddress();
  //Serial.println();
  Serial.print(address);
  Serial.print(" , ");
  Serial.print(diff);

}
