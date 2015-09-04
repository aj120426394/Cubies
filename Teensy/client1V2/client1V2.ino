/**
 * A Mirf example to test the latency between two Ardunio.
 *`1234567898765432456789
 * Pins:
 * Hardware SPI:
 * MISO -> 12
 * MOSI -> 11
 * SCK -> 13
 *
 * Configurable:
 * CE -> 8
 * CSN -> 7
 *
 * Note: To see best case latency comment out all Serial.println
 * statements not displaying the result and load
 * 'ping_server_interupt' on the server.
 */
#include <SPI.h>
#include <Mirf.h>
#include <nRF24L01.h>
#include <MirfHardwareSpiDriver.h>
#include <Adafruit_NeoPixel.h>
#include <avr/power.h>

#define LEDPIN         2
#define NUMPIXELS      5
Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, LEDPIN, NEO_GRB + NEO_KHZ800);


const byte CLIENTNAME = (byte)0xC1;

//Define command button pin
const int buttonIF = 18;
const int buttonTHEN = 19;
const int buttonENTER = 20;
const int buttonNOT = 21;
const int buttonLOOP = 22;
const int buttonOR = 23;

// check the command is available to press
boolean checkIF = true;
boolean checkENTER = false;
boolean checkTHEN = false;
boolean checkNOT = false;
boolean checkOR = false;
boolean checkLOOP = true;

// check the color is available to press
boolean checkRED = false;
boolean checkBLUE = false;
boolean checkGREEN = false;
boolean checkYELLOW = false;
boolean checkPURPLE = false;

boolean gameStart = false;
boolean inputable = false;
boolean shipColor = false;

// check the command has been pressed
boolean pressTHEN = false;
boolean pressOR = false;
boolean pressLOOP = false;

//byte* test;
//byte test[2];

int currentState = 1;

unsigned long buttonPressTimeStamp;

void setup(){
  pinMode(buttonIF, INPUT);
  pinMode(buttonTHEN, INPUT);
  pinMode(buttonENTER, INPUT);
  pinMode(buttonNOT, INPUT);
  pinMode(buttonLOOP, INPUT);
  pinMode(buttonOR, INPUT);

  //test[0] = (byte)0xD5;
  //test[1] = (byte)0xFF;

  attachInterrupt(buttonIF, bState1, HIGH);
  attachInterrupt(buttonTHEN, bState1, HIGH);
  attachInterrupt(buttonENTER, bState1, HIGH);
  attachInterrupt(buttonNOT, bState2, HIGH);
  attachInterrupt(buttonLOOP, bState1, HIGH);
  attachInterrupt(buttonOR, bState1, HIGH);

  Serial.begin(9600);
  pixels.begin();

  Mirf.spi = &MirfHardwareSpi;
  Mirf.init();
  Mirf.setRADDR((byte *)"clie1");
  Mirf.payload = 32;
  Mirf.config();
  checkIF = true;
  Serial.println("Beginning ... ");

}

void loop(){
  LEDcontroller();

  if(!Mirf.isSending() && Mirf.dataReady()){
    byte data[Mirf.payload];
    Mirf.getData(data);
    if(data[0] == CLIENTNAME){
      Serial.print("From Server: ");
      Serial.println(data[1],HEX);
      dataGet(data[1]);
    }
  }
  //pixels.setPixelColor(5, pixels.Color(0,255,0));
  //pixels.show();
}
//initial setting of the button
void bState1(){
  if(inputable && millis() - buttonPressTimeStamp >= 500){
    buttonPressTimeStamp = millis();
    if(digitalRead(buttonIF) == HIGH && checkIF){
      IF(false);
    }else if(digitalRead(buttonTHEN) == HIGH && checkTHEN){
      THEN(false);
    }else if(digitalRead(buttonENTER) == HIGH && checkENTER){
      ENTER(false);
    }else if(digitalRead(buttonLOOP) == HIGH && checkLOOP){
      LOOP(false);
    }else if(digitalRead(buttonOR) == HIGH && checkOR){
      OR(false);
    }
  }
}
// color up, NOT up
void bState2(){
  if(inputable && millis() - buttonPressTimeStamp >= 500){
    buttonPressTimeStamp = millis();
    if(digitalRead(buttonIF) == HIGH && checkBLUE){
       BLUE(false);
    }else if(digitalRead(buttonTHEN) == HIGH && checkRED){
       RED(false);
    }else if(digitalRead(buttonOR) == HIGH && checkYELLOW){
       YELLOW(false);
    }

    if(!pressLOOP){
      if(gameStart){
        if(digitalRead(buttonNOT) == HIGH && checkGREEN){
           GREEN(false);
       }else if(digitalRead(buttonLOOP) == HIGH && checkLOOP){
           LOOP(false);
       }
      }else{
        if(digitalRead(buttonNOT) == HIGH && checkNOT){
           NOT(false);
        }else if(digitalRead(buttonLOOP) == HIGH && checkGREEN){
           GREEN(false);
        }
      }

     }else{
       if(digitalRead(buttonNOT) == HIGH && checkBLUE){
         GREEN(false);
       }
     }
  }
}

void stateChange(int toState){
  checkReset();

  if (toState == 1){
    // IF / LOOP
    buttonChange(1);
    checkIF = true;
    checkLOOP = true;
  }else if(toState == 2){
    // COLOR / NOT
    buttonChange(2);
    if(!gameStart){
      if(pressLOOP){
        checkNOT = false;
      }else{
        checkNOT = true;
      }
      checkRED = true;
      checkBLUE = true;
      checkGREEN = true;
      checkYELLOW = true;
      checkPURPLE = true;
    }
  }else if(toState == 3){
    // THEN / OR / ENTER
    buttonChange(1);
    if(!pressLOOP){
      if(!pressTHEN){
        checkTHEN = true;
      }else{
        checkENTER = true;
      }
       checkOR = true;
      /*
      if(!pressOR){
        checkOR = true;
      }
      */
    }else{
      checkTHEN = true;
      if(pressTHEN){
        checkENTER = true;
      }
    }
  }
}

void buttonChange(int temp){
  if(temp == 1){
    attachInterrupt(buttonIF, bState1, HIGH);
    attachInterrupt(buttonTHEN, bState1, HIGH);
    attachInterrupt(buttonENTER, bState1, HIGH);
    attachInterrupt(buttonLOOP, bState1, HIGH);
    attachInterrupt(buttonOR, bState1, HIGH);
  }else if(temp == 2){
    attachInterrupt(buttonIF, bState2, HIGH);
    attachInterrupt(buttonTHEN, bState2, HIGH);
    attachInterrupt(buttonENTER, bState2, HIGH);
    attachInterrupt(buttonLOOP, bState2, HIGH);
    attachInterrupt(buttonOR, bState2, HIGH);
  }
}

void IF(bool flag){
  if(!flag){
    Serial.println("IF");
    byte str = (byte)0x01;
    dataSending(str);
  }else{
    stateChange(2);
  }
}

void THEN(bool flag){
  if(!flag){
    Serial.println("THEN");
    byte str = (byte)0x02;
    dataSending(str);
  }else{
    pressTHEN = true;
    pressOR = false;
    stateChange(2);
  }
}

void NOT(bool flag){
  if(!flag){
    Serial.println("NOT");
    byte str = (byte)0x03;
    dataSending(str);
  }else{
    checkNOT = false;
  }
}

void LOOP(bool flag){
  if(!flag){
    Serial.println("LOOP");
    byte str = (byte)0x04;
    dataSending(str);
  }else{
    if(!gameStart){
      pressLOOP = true;
    }
    checkLOOP = false;
  }



  // LED twinkle
}
void OR(bool flag){
  if(!flag){
    Serial.println("OR");
    byte str = (byte)0x05;
    dataSending(str);
  }else{
    pressOR = true;
    stateChange(2);
  }
}
void RED(bool flag){
  if(!flag){
    Serial.println("RED");
    byte str = (byte)0x06;
    dataSending(str);
  }else{
    if(!gameStart){
      stateChange(3);
    }else{
      checkReset();
    }
  }
}
void BLUE(bool flag){
  if(!flag){
    Serial.println("BLUE");
    byte str = (byte)0x07;
    dataSending(str);
  }else{
    if(!gameStart){
      stateChange(3);
    }else{
      checkReset();
    }
  }
}
void GREEN(bool flag){
  if(!flag){
    Serial.println("GREEN");
    byte str = (byte)0x08;
    dataSending(str);
  }else{
    if(!gameStart){
      stateChange(3);
    }else{
      checkReset();
    }
  }
}
void YELLOW(bool flag){
  if(!flag){
    Serial.println("YELLOW");
    byte str = (byte)0x09;
    dataSending(str);
  }else{
    if(!gameStart){
      stateChange(3);
    }else{
      checkReset();
    }
  }
}
void PURPLE(bool flag){
  if(!flag){
    Serial.println("PURPLE");
    byte str = (byte)0x0A;
    dataSending(str);
  }else{
    if(!gameStart){
      stateChange(3);
    }else{
      checkReset();
    }
  }
}
void ENTER(bool flag){
  if(!flag){
    Serial.println("ENTER");
    byte str = (byte)0x0B;
    dataSending(str);
  }else{
    stateChange(1);
    checkENTER = false;
    pressTHEN = false;
    pressOR = false;
    if(pressLOOP){
      //stop twinkle
      pressLOOP = false;
    }
  }
}

// data sending through radio
void dataSending(byte str){
  byte data[2] = {CLIENTNAME, str};
  Mirf.setTADDR((byte *)"serv1");
  Mirf.send(data);
  while(Mirf.isSending()){
  }
  Serial.println("Data Sent");
}

void dataGet(byte str){
  if(str == (byte)0xFF){
    //game start
    gameStart = true;
    inputable = true;
    shipColor = false;
    stateChange(2);
  }else if(str == (byte)0xEE){
    //start input
    inputable = true;
    gameStart = false;
    shipColor = false;
    stateChange(1);
  }else if(str == (byte)0xDD){
    //Game finish
    inputable = false;
    gameStart = false;
    shipColor = false;
    stateChange(1);
  }else if(str == (byte)0xCC){
    //select ship color
    inputable = false;
    gameStart = false;
    shipColor = true;
    stateChange(2);
  }

  if(str == (byte)0x06){
    if(shipColor){
      checkRED = false;
    }else{
      checkRED = true;
      Serial.println("LED:RED");
    }
  }else if(str == (byte)0x07){
    if(shipColor){
      checkBLUE = false;
    }else{
      checkBLUE = true;
      Serial.println("LED:BLUE");
    }
  }else if(str == (byte)0x08){
    if(shipColor){
      checkGREEN = false;
    }else{
      checkGREEN = true;
      Serial.println("LED:GREEN");
    }
  }else if(str == (byte)0x09){
    if(shipColor){
      checkYELLOW = false;
    }else{
      checkYELLOW = true;
      Serial.println("LED:YELLOW");
    }
  }else if(str == (byte)0x0A){
    if(shipColor){
      checkPURPLE = false;
    }else{
      checkPURPLE = true;
      Serial.println("LED:PURPLE");
    }
  }else if(str == (byte)0x04){
    checkLOOP = true;
    Serial.println("LED:LOOP");
  }

  if(str == (byte)0xB1){
    IF(true);//I
  }else if(str == (byte)0xB2){
    THEN(true);//T
  }else if(str == (byte)0xB3){
    NOT(true);//N
  }else if(str == (byte)0xB4){
    LOOP(true);//L
  }else if(str == (byte)0xB5){
    OR(true);//O
  }else if(str == (byte)0xB6){
    RED(true);//R
  }else if(str == (byte)0xB7){
    BLUE(true);//B
  }else if(str == (byte)0xB8){
    GREEN(true);//G
  }else if(str == (byte)0xB9){
    YELLOW(true);//Y
  }else if(str == (byte)0xBA){
    PURPLE(true);//P
  }else if(str == (byte)0xBB){
    ENTER(true);//E
  }
}

void checkReset(){
  checkIF = false;
  checkENTER = false;
  checkTHEN = false;
  checkNOT = false;
  checkOR = false;
  checkLOOP = false;

  checkRED = false;
  checkBLUE = false;
  checkGREEN = false;
  checkYELLOW = false;
  checkPURPLE = false;
}

void LEDcontroller(){
  if(checkIF || checkBLUE){
    if(checkIF){
      pixels.setPixelColor(0, pixels.Color(255,255,255));
    }else{
      pixels.setPixelColor(0, pixels.Color(0,0,255));
    }
  }else{
    pixels.setPixelColor(0, pixels.Color(0,0,0));
  }

  if(checkOR || checkYELLOW){
    if(checkOR){
      pixels.setPixelColor(3, pixels.Color(255,255,255));
    }else{
      pixels.setPixelColor(3, pixels.Color(255,255,0));
    }
  }else{
    pixels.setPixelColor(3, pixels.Color(0,0,0));
  }

  if(checkTHEN || checkRED){
    if(checkTHEN){
      pixels.setPixelColor(2, pixels.Color(255,255,255));
    }else{
      pixels.setPixelColor(2, pixels.Color(255,0,0));
    }
  }else{
    pixels.setPixelColor(2, pixels.Color(0,0,0));
  }

  /*
  if(checkENTER){
    pixels.setPixelColor(2, pixels.Color(255,255,255));
  }else{
    pixels.setPixelColor(2, pixels.Color(0,0,0));
  }
  */

  if(checkNOT || checkGREEN  && pressLOOP){
    if(checkNOT){
      pixels.setPixelColor(1, pixels.Color(255,255,255));
    }else{
      pixels.setPixelColor(1, pixels.Color(0,255,0));
    }
  }else{
    pixels.setPixelColor(1, pixels.Color(0,0,0));
  }

  if(checkLOOP || checkGREEN && !gameStart && !pressLOOP){
    if(checkLOOP){
      pixels.setPixelColor(4, pixels.Color(255,255,255));
    }else{
      pixels.setPixelColor(4, pixels.Color(0,255,0));
    }
  }else{
    pixels.setPixelColor(4, pixels.Color(0,0,0));
  }
  pixels.show();
}

