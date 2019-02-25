
#include <SoftwareSerial.h>
#include <SerialCommand.h>

SerialCommand sCmd;

//relays pin number
int relay = 5;

// analog pin number for pressure metering
int pressPin = A0;

//next 4 initialized variables are for pressure handling
float pressure = 0.0f;
float bar= 0.0f;
float maxpress = 0.5f;
float avrgPress;

//count variable is for average pressure calculating
int count = 0;

float avrgPress;
void setup() 
{ 
  pinMode(relay, OUTPUT);
  pinMode(pressPin, INPUT);
  
  Serial.begin(9600);
  while (!Serial);
  
  //What happens when "ECHO" or "PING" commands are received
  sCmd.addCommand("ECHO", echoHandler);
  sCmd.addCommand("PING", grabHandler);
}

void loop() 
{    
  //Add pressure value on top of previous each loop count
  pressure = pressure + analogRead(pressPin);
  count ++;

  //when there is 10 pressure values stacked, 
  //calculate average pressure and reset state
  if(count >= 10)
  {
     avrgPress = pressure / 10.0f;
     pressure = 0.0f;
     count = 0; 
  }

  //transform value to bars
  bar = -1.42307 + avrgPress * 0.01499;

  //Read serial if there is an input
  if (Serial.available() > 0) sCmd.readSerial();
  
  //if pressure goes over wanted value then valve closes
  if (bar > maxpress)
  {
    digitalWrite(relay, HIGH);
  }  

  //open valve and let pressure flow in if 
  //pressure inside is less than 0.1 bar
  if(bar < 0.1f)
  {
    digitalWrite(relay, LOW); //valve is open  
  }
}

//how does arduino handle different different "ECHO arg" commands
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
    
    //send value of max pressure and arg back to Unity as "confirmation"
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
