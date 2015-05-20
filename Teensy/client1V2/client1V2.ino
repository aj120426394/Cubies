/**
 * A Mirf example to test the latency between two Ardunio.
 *
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

#define LEDPIN         6
#define NUMPIXELS      2
Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, LEDPIN, NEO_GRB + NEO_KHZ800);


const byte CLIENTNAME = (byte)0xC1;

//Define command button pin
const int buttonIF = 2;
const int buttonTHEN = 3;
const int buttonENTER = 4;
const int buttonNOT = 5;
const int buttonLOOP = 20;
const int buttonOR = 22;

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

// check the command has been pressed
boolean pressTHEN = false;
boolean pressOR = false;
boolean pressLOOP = false;

//byte* test;
//byte test[2];

int currentState = 1;

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

  Serial.println("Beginning ... ");
  
}

void loop(){
  if(!Mirf.isSending() && Mirf.dataReady()){
    byte data[Mirf.payload];
    Mirf.getData(data);
    Serial.print("Ping: ");
    Serial.println(data[0],HEX);
    dataGet(data[0]);
  }
  delay(100);
} 
//initial setting of the button
void bState1(){
  if(inputable){
    if(digitalRead(buttonIF) == HIGH && checkIF){
      IF();
    }else if(digitalRead(buttonTHEN) == HIGH && checkTHEN){
      THEN();
    }else if(digitalRead(buttonENTER) == HIGH && checkENTER){
      ENTER();
    }else if(digitalRead(buttonLOOP) == HIGH && checkLOOP){
      LOOP();
    }else if(digitalRead(buttonOR) == HIGH && checkOR){
      OR();
    }
    delay(500);
  }
}
// color up, NOT up
void bState2(){
  if(inputable){
    if(digitalRead(buttonIF) == HIGH && checkRED){
       RED();
    }else if(digitalRead(buttonTHEN) == HIGH && checkYELLOW){
       YELLOW();
    }else if(digitalRead(buttonENTER) == HIGH && checkGREEN){
       GREEN();
    }else if(digitalRead(buttonOR) == HIGH && checkPURPLE){
       PURPLE();
    }
   
    if(!pressLOOP){
      if(gameStart){
        if(digitalRead(buttonNOT) == HIGH && checkBLUE){
           BLUE();
       }else if(digitalRead(buttonLOOP) == HIGH && checkLOOP){
           LOOP();
       } 
      }else{
        if(digitalRead(buttonNOT) == HIGH && checkNOT){
           NOT();
        }else if(digitalRead(buttonLOOP) == HIGH && checkBLUE){
           BLUE();
        }
      }
       
     }else{
       if(digitalRead(buttonNOT) == HIGH && checkBLUE){
         BLUE();
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
      checkNOT = true;
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
      if(!pressOR){
        checkOR = true;
      }
    }else{
      checkTHEN = true;
      if(pressTHEN){
        checkENTER = true;
      }
    }
  }
}

void buttonChange(int temp){
  delay(500);
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

void IF(){
  Serial.println("IF");
  byte str = (byte)0x01; 
  dataSending(str);
  stateChange(2);
}

void THEN(){
  Serial.println("THEN");
  byte str = (byte)0x02;
  dataSending(str);
  
  pressTHEN = true;
  pressOR = false;
  stateChange(2);
}

void NOT(){
  Serial.println("NOT");
  byte str = (byte)0x03;
  dataSending(str);
  checkNOT = false;
}

void LOOP(){
  Serial.println("LOOP");
  byte str = (byte)0x04;
  dataSending(str);
  if(!gameStart){
    pressLOOP = true;
  }
  checkLOOP = false;
  
  // LED twinkle 
}
void OR(){
  Serial.println("OR");
  byte str = (byte)0x05;  
  dataSending(str);
  pressOR = true;
  stateChange(2);
}
void RED(){
  Serial.println("RED");
  byte str = (byte)0x06;  
  dataSending(str);
  if(!gameStart){
    stateChange(3);
  }else{
    checkReset();
  }
}
void BLUE(){
  Serial.println("BLUE");
  byte str = (byte)0x07;  
  dataSending(str);
  if(!gameStart){
    stateChange(3);
  }else{
    checkReset();
  }
}
void GREEN(){
  Serial.println("GREEN");
  byte str = (byte)0x08;  
  dataSending(str);
  if(!gameStart){
    stateChange(3);
  }else{
    checkReset();
  }
}
void YELLOW(){
  Serial.println("YELLOW");
  byte str = (byte)0x09;  
  dataSending(str);
  if(!gameStart){
    stateChange(3);
  }else{
    checkReset();
  }
}
void PURPLE(){
  Serial.println("PURPLE");
  byte str = (byte)0x0A;  
  dataSending(str);
  if(!gameStart){
    stateChange(3);
  }else{
    checkReset();
  }
}
void ENTER(){
  Serial.println("ENTER");
  byte str = (byte)0x0B;  
  dataSending(str);
  stateChange(1);
  checkENTER = false;
  pressTHEN = false;
  pressOR = false;
  if(pressLOOP){
    //stop twinkle
    pressLOOP = false;
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
    gameStart = true;
    stateChange(2);
  }else if(str == (byte)0xEE){
    inputable = true;
    gameStart = false;
    stateChange(1);
  }else if(str == (byte)0xDD){
    inputable = false;
    gameStart = false;
    stateChange(1);
  }
  
  if(str == (byte)0x06){
    checkRED = true;
    Serial.println("LED:RED");
  }else if(str == (byte)0x07){
    checkBLUE = true;
    Serial.println("LED:BLUE");
  }else if(str == (byte)0x08){
    checkGREEN = true;
    Serial.println("LED:GREEN");
  }else if(str == (byte)0x09){
    checkYELLOW = true;
    Serial.println("LED:YELLOW");
  }else if(str == (byte)0x0A){
    checkPURPLE = true;
    Serial.println("LED:PURPLE");
  }else if(str == (byte)0x04){
    checkLOOP = true;
    Serial.println("LED:LOOP");
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
  if(checkIF){
    pixels.setPixelColor(0, pixels.Color(255,255,255));
  }
  if(checkOR){
    pixels.setPixelColor(1, pixels.Color(255,255,255));
  }
  if(checkTHEN){
    pixels.setPixelColor(2, pixels.Color(255,255,255));
  }
  if(checkENTER){
    pixels.setPixelColor(3, pixels.Color(255,255,255));
  }
  if(checkNOT){
    pixels.setPixelColor(4, pixels.Color(255,255,255));
  }
  if(checkLOOP){
    pixels.setPixelColor(5, pixels.Color(255,255,255));
  }
  
  if(checkRED){
    pixels.setPixelColor(0, pixels.Color(255,0,0));
  }
  if(checkBLUE){
    pixels.setPixelColor(1, pixels.Color(0,255,0));
  }
  if(checkGREEN){
    pixels.setPixelColor(2, pixels.Color(0,0,255));
  }
  if(checkYELLOW){
    pixels.setPixelColor(3, pixels.Color(255,255,0));
  }
  if(checkPURPLE){
    if(checkLOOP){
      pixels.setPixelColor(4, pixels.Color(255,0,255));
    }else if(checkNOT){
      pixels.setPixelColor(5, pixels.Color(255,0,255));
    }
    
  }

  
  
}

