
#include <SoftwareSerial.h>
#include <SerialCommand.h>

SerialCommand sCmd;

int relay = 5;
int pressPin = A0;
float pressure = 0.0f;
float bar= 0.0f;
float maxpress = 0.5f;

void setup() 
{ 
  pinMode(relay, OUTPUT);
  pinMode(pressPin, INPUT);
  
  Serial.begin(9600);
  while (!Serial);
  
  //What happens when "ECHO X" command is received
  sCmd.addCommand("ECHO", echoHandler);
}

void loop() 
{    
  pressure = analogRead(pressPin);
  bar = -1.42307 + pressure * 0.01499;
  //Serial.println(bar);

  //Read serial if theres input
  if (Serial.available() > 0) sCmd.readSerial();
  
  //if pressure goes over wanted value then valve closes
  if (bar > maxpress)
  {
    digitalWrite(relay, HIGH);
  }  

  if(bar < 0.2f)
  {
    digitalWrite(relay, LOW); //valve is open  
  }
}

//how does arduino handle different states
void echoHandler () {
  char *arg;
  arg = sCmd.next();
  if (arg != NULL)
  {
    if(String(arg) == "1")
    {
      maxpress=0.5f;
    }
    if(String(arg) == "2")
    {
      maxpress=0.75f;
    }
    if(String(arg) == "3")
    {
      maxpress=1.0f;
    }
    
    //send value of max pressure and arg back to unity when max pressure is assigned and pressure is higher that max pressure (object grabbed)
    //if(bar > (maxpress+ 0.1f))
    String msgdata = String(maxpress) + " I " + arg + " I ";
    Serial.println( msgdata );
  }
  else
    Serial.println("nothing to echo");
}
