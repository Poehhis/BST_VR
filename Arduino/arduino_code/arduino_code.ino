
#include <SoftwareSerial.h>
#include <SerialCommand.h>

SerialCommand sCmd;

int relay = 5;
int pressPin = A0;
float pressure = 0.0f;
float bar= 0.0f;
float maxpress = 0.5f;
int count = 0;
float avrgPress;

void setup() 
{ 
  pinMode(relay, OUTPUT);
  pinMode(pressPin, INPUT);
  
  Serial.begin(9600);
  while (!Serial);
  
  //What happens when "ECHO X" command is received
  sCmd.addCommand("ECHO", echoHandler);
  sCmd.addCommand("PING", grabHandler);
}

void loop() 
{    
  pressure = pressure + analogRead(pressPin);
  count ++;
  if(count >= 10)
  {
     avrgPress = pressure / 10.0f;
     pressure = 0.0f;
     count = 0; 
  }
      
  bar = -1.42307 + avrgPress * 0.01499;
  //Serial.println(bar);

  //Read serial if theres input
  if (Serial.available() > 0) sCmd.readSerial();
  
  //if pressure goes over wanted value then valve closes
  if (bar > maxpress)
  {
    digitalWrite(relay, HIGH);
  }  

  if(bar < 0.1f)
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
      maxpress=0.25f;
    }
    if(String(arg) == "2")
    {
      maxpress=0.35f;
    }
    if(String(arg) == "3")
    {
      maxpress=0.5f;
    }
    
    //send value of max pressure and arg back to unity when max pressure is assigned
    String msgdata = String(maxpress) + " I " + arg + " I ";
    Serial.println( msgdata );
  }
  else
    Serial.println("nothing to echo");
}

//if PING message is sent and pressure is higher than max pressure (object grabbed)
void grabHandler () 
{
  if(bar > (maxpress + 0.1f))
    {
     Serial.println("squeezed");  
    }
}
