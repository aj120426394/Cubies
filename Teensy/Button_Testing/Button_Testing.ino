/*
  Button
 
 Turns on and off a light emitting diode(LED) connected to digital  
 pin 13, when pressing a pushbutton attached to pin 2. 
 
 
 The circuit:
 * LED attached from pin 13 to ground 
 * pushbutton attached to pin 2 from +5V
 * 10K resistor attached to pin 2 from ground
 
 * Note: on most Arduinos there is already an LED on the board
 attached to pin 13.
 
 
 created 2005
 by DojoDave <http://www.0j0.org>
 modified 30 Aug 2011
 by Tom Igoe
 
 This example code is in the public domain.
 
 http://www.arduino.cc/en/Tutorial/Button
 */
#include <Bounce2.h>
Bounce debouncer = Bounce(); 
// constants won't change. They're used here to 
// set pin numbers:
const int buttonPin = 22;     // the number of the pushbutton pin
const int buttonPin2 = 20;
const int buttonPin3 = 21;
const int buttonPin4 = 23;
const int buttonPin5 = 19;
const int buttonPin6 = 18;

const int buttonIF = 18;
const int buttonTHEN = 19;
const int buttonENTER = 23;
const int buttonNOT = 21;
const int buttonLOOP = 22;
const int buttonOR = 20;


const int ledPin =  13;      // the number of the LED pin
  // Pin 13: Arduino has an LED connected on pin 13
  // Pin 11: Teensy 2.0 has the LED on pin 11
  // Pin  6: Teensy++ 2.0 has the LED on pin 6
  // Pin 13: Teensy 3.0 has the LED on pin 13

// variables will change:
int buttonState = 0;         // variable for reading the pushbutton status
int buttonState2 = 0;
int buttonState3 = 0;
int buttonState4 = 0;
int buttonState5 = 0;
int buttonState6 = 0;

unsigned long buttonPressTimeStamp;

void setup() {
  // initialize the LED pin as an output:
  pinMode(ledPin, OUTPUT);      
  // initialize the pushbutton pin as an input:
  pinMode(buttonPin, INPUT);
  pinMode(buttonPin2, INPUT);
  pinMode(buttonPin3, INPUT);
  pinMode(buttonPin4, INPUT);
  pinMode(buttonPin5, INPUT);
  pinMode(buttonPin6, INPUT);
}

void loop(){
  // read the state of the pushbutton value:
  buttonState = digitalRead(buttonIF);
  buttonState2 = digitalRead(buttonTHEN);
  buttonState3 = digitalRead(buttonENTER);
  buttonState4 = digitalRead(buttonNOT);
  buttonState5 = digitalRead(buttonLOOP);
  buttonState6 = digitalRead(buttonOR);


  // check if the pushbutton is pressed.
  // if it is, the buttonState is HIGH:
  if(buttonState == HIGH && millis() - buttonPressTimeStamp >= 500){
    Serial.println("IF");
    buttonPressTimeStamp = millis();
  }else if(buttonState2 == HIGH && millis() - buttonPressTimeStamp >= 500){
    Serial.println("THEN");
    buttonPressTimeStamp = millis();
  }else if(buttonState3 == HIGH && millis() - buttonPressTimeStamp >= 500){
    Serial.println("ENTER");
    buttonPressTimeStamp = millis();
  }else if(buttonState4 == HIGH && millis() - buttonPressTimeStamp >= 500){
    Serial.println("NOT");
    buttonPressTimeStamp = millis();
  }else if(buttonState5 == HIGH && millis() - buttonPressTimeStamp >= 500){
    Serial.println("LOOP");
    buttonPressTimeStamp = millis();
  }else if(buttonState6 == HIGH && millis() - buttonPressTimeStamp >= 500){
    Serial.println("OR");
    buttonPressTimeStamp = millis();
  }
  
  if (buttonState == HIGH) {     
    // turn LED on:    
    digitalWrite(ledPin, HIGH);  
  } 
  else {
    // turn LED off:
    digitalWrite(ledPin, LOW); 
  }
}
