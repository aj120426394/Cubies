/**
 * An Mirf example which copies back the data it recives.
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
 */

#include <SPI.h>
#include <Mirf.h>
#include <nRF24L01.h>
#include <MirfHardwareSpiDriver.h>

const int numOfClient = 3;

char serialData[20];
char* clients[numOfClient] = {"clie1", "clie2", "clie3"};

void setup(){
  Serial.begin(9600);
  Mirf.spi = &MirfHardwareSpi;
  Mirf.init();
  Mirf.setRADDR((byte *)"serv1");
  Mirf.payload = 32;
  Mirf.config();
}

void loop(){
  if(!Mirf.isSending() && Mirf.dataReady()){
    //Serial.println("Get data");
    byte data[Mirf.payload];
    Mirf.getData(data);
    char* from = "";
    char* str = "";
    if(data[0] == (byte)0xC1){
      from = "C1";
    }else if(data[0] == (byte)0xC2){
      from = "C2";
    }else if(data[0] == (byte)0xC3){
      from = "C3";
    }else if(data[0] == (byte)0xC4){
      from = "C4";
    }
    
    if(data[1] == (byte)0x01){
      str = "I"; //IF
    }else if(data[1] == (byte)0x02){
      str = "T"; //THEN
    }else if(data[1] == (byte)0x03){
      str = "N"; //NOT
    }else if(data[1] == (byte)0x04){
      str = "L"; //LOOP
    }else if(data[1] == (byte)0x05){
      str = "O"; //OR
    }else if(data[1] == (byte)0x06){
      str = "R"; //RED
    }else if(data[1] == (byte)0x07){
      str = "B"; //BLUE
    }else if(data[1] == (byte)0x08){
      str = "G"; //GREEN
    }else if(data[1] == (byte)0x09){
      str = "Y"; //YELLOW
    }else if(data[1] == (byte)0x0A){
      str = "P"; //PURPLE
    }else if(data[1] == (byte)0x0B){
      str = "E"; //ENTER
    }
    Serial.println((String)from + ":" + (String)str);
    feedback(from, str);
    
    //S erial.println(data[1], HEX);
    Serial.flush();
  }
  
  // Read from Serial
  if(Serial.available() > 0){
    int lf = 10;
    Serial.readBytesUntil(lf,serialData,15);
    byte client;
    if(strstr(serialData, "C1") != NULL){
      client = (byte)0xC1;
    }else if(strstr(serialData, "C2") != NULL){
      client = (byte)0xC2;
    }else if(strstr(serialData, "C3") != NULL){
      client = (byte)0xC3;
    }else if(strstr(serialData, "C4") != NULL){
      client = (byte)0xC4;
    }
    
    sendData(client, serialData);
    Serial.flush();
  }
}

void feedback(char* from, char* data){
   byte sendData[1];
   
   char* client;
    if(strstr(from, "C1") != NULL){
      client = "clie1";
    }else if(strstr(from, "C2") != NULL){
      client = "clie2";
    }else if(strstr(from, "C3") != NULL){
      client = "clie3";
    }else if(strstr(from, "C4") != NULL){
      client = "clie4";
    }

  if(strstr(data, "I") != NULL){
    sendData[0] = (byte)0xB1;
  }else if(strstr(data, "T") != NULL){
    sendData[0] = (byte)0xB2;
  }else if(strstr(data, "N") != NULL){
    sendData[0] = (byte)0xB3;
  }else if(strstr(data, "O") != NULL){
    sendData[0] = (byte)0xB5;
  }else if(strstr(data, "L") != NULL){
    sendData[0] = (byte)0xB4;
  }else if(strstr(data, "E") != NULL){
    sendData[0] = (byte)0xBB;
  }else if(strstr(data, "R") != NULL){
    sendData[0] = (byte)0xB6;
  }else if(strstr(data, "B") != NULL){
    sendData[0] = (byte)0xB7;
  }else if(strstr(data, "G") != NULL){
    sendData[0] = (byte)0xB8;
  }else if(strstr(data, "Y") != NULL){
    sendData[0] = (byte)0xB9;
  }else if(strstr(data, "P") != NULL){
    sendData[0] = (byte)0xBA;
  }
  Mirf.setTADDR((byte *)client);
  Mirf.send(sendData);
  while(Mirf.isSending()){
  }
}
void sendData(byte client, char* sendingData){
  byte sendData[2];
  
  if(strstr(serialData, "clie1") != NULL){
      client = "clie1";
    }else if(strstr(serialData, "C2") != NULL){
      client = "clie2";
    }else if(strstr(serialData, "C3") != NULL){
      client = "clie3";
    }else if(strstr(serialData, "C4") != NULL){
      client = "clie4";
    }
  
  if(strstr(sendingData, "GameStart") != NULL){
    sendData[1] = (byte)0xFF;
  }else if(strstr(sendingData, "StartInput") != NULL){
    sendData[1] = (byte)0xEE;
  }else if(strstr(sendingData, "GameFin") != NULL){
    sendData[1] = (byte)0xDD;
  }else if(strstr(sendingData, "ShipColor") != NULL){
    sendData[1] = (byte)0xCC;
  }else if(strstr(sendingData, "R") != NULL){
    sendData[1] = (byte)0x06;
  }else if(strstr(sendingData, "B") != NULL){
    sendData[1] = (byte)0x07;
  }else if(strstr(sendingData, "G") != NULL){
    sendData[1] = (byte)0x08;
  }else if(strstr(sendingData, "Y") != NULL){
    sendData[1] = (byte)0x09;
  }else if(strstr(sendingData, "P") != NULL){
    sendData[1] = (byte)0x0A;
  }else if(strstr(sendingData, "L") != NULL){
    sendData[1] = (byte)0x04;
  }
  Mirf.setTADDR((byte *)"clie1");
  Mirf.send(sendData);
  while(Mirf.isSending()){
  }
}
