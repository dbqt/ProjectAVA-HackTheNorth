#!/usr/local/bin/python

# Reading an analogue sensor with
# multiple GPIO pins
# send to firebase

# Author : Julien Levesque
# Distribution : Raspbian
# Python : 3.0
# GPIO   : RPi.GPIO v3.1.0a

import RPi.GPIO as GPIO, time
import pyrebase
import json
import requests

config = {
    "apiKey": "apiKey",
    "authDomain": "projectId.firebaseapp.com",
    "databaseURL": "https://projectava-1de83.firebaseio.com",
    "storageBucket": "projectId.appspot.com"
}

#Initialize database
firebase = pyrebase.initialize_app(config)
db = firebase.database()
    
# Tell the GPIO library to use
# Broadcom GPIO references
GPIO.setmode(GPIO.BCM)

#GPIO
GPIO.setup(16, GPIO.IN)
GPIO.setup(21, GPIO.IN)
GPIO.setup(20, GPIO.IN)

#Variable Init
redFeel = 0
yellowFeel = 0
blueFeel = 0

# Main program loop
while True:
   
    if GPIO.input(16):
      redFeel +=1
      print ("redFeel" + str(redFeel))
      time.sleep(0.1)

    else:
      redFeel =0
    
    if GPIO.input(21):
      yellowFeel +=1
      print ("yellowFeel" + str(yellowFeel))
      time.sleep(0.1)

    else:
      yellowFeel = 0

    if GPIO.input(20):
      blueFeel +=1
      print("blueFeel" + str(blueFeel))
      time.sleep(0.1)

    else:
      blueFeel = 0

    
    #Send JSON formatted data
    data = {"yellowFeel": yellowFeel, "redFeel": redFeel, "blueFeel": blueFeel}

    #Send retrieved
    db.child("data").set(data)
    

    

