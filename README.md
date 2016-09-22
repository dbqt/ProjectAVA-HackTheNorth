# ProjectAVA :  Augmented Virtual Assistant

[![IMAGE ALT TEXT](http://s17.postimg.org/p6f6jj64f/IMG_20160918_083939play.jpg)](https://www.youtube.com/watch?v=ghabkmMT5IA "ProjectAVA")

##Inspiration
Our inspiration for this idea came from various sources, mainly from Cortana originating from the video game Halo as well as a futuristic ideal of having artificial intelligence-endowed companions for your everyday life.

##What it does
AVA, also known as Augmented Virtual Assistant, is a portable companion utilizing the Pepper's Ghost Illusion concept from more than a hundred years ago.

##How we built it
This assistant makes use of three main components: a Raspberry Pi connected with three pressure sensors, an Android application for speech-to-text recognition to communicate with AVA as well as the screen and recycled plastic duo to give a three-dimensional component to our project.

The Raspberry Pi is programmed in Python to read the sensors' pressure values; these are evaluated on a variable scale of values to give the user a more intuitive experience. The accumulated data is pushed onto a real-time database provided by Firebase every second to simulate real-time transferring and updating.

The Android application utilizes the Google speech-to-text API to recognize the user's commands to AVA. The text received is later passed to be analyzed: if there are occurrences of any known commands, AVA will execute that command. Every time a command is chosen, it is pushed onto the Firebase database. The main logic for all assistant tasks is processed in this application.

The screen used to display the holographic-like projection of AVA was created in Unity. The 3D models as well as animations are distributed for free by mixamo as well as other sites such as turbosquid and Autodesk. As the viewer of our project, the program behind the screen projection verifies every second for any updated values or commands to understand which animation and scene to display.

##Challenges we ran into
For the Raspberry Pi, we originally planned on having infrared position location by aligning multiple infrared sensors in a circle around the projection. Unfortunately, the infrared sensors were too efficient and could detect around them no matter the direction the infrared signals were coming from. Furthermore, we had planned on using a USB-port webcam with an integrated microphone to process the speech-to-text function. Sadly, it was impossible for us to detect the webcam on the Raspberry Pi, and we were left with no microphones to implement this feature. After some thinking, we decided that using an Android phone's built-in microphone as well as native text-to-speech features was a more simple and efficient solution. Another problem we encountered during the last night of the hackathon; we could not display the animations via the script, and we could not figure out why until a very long time of searching and understanding that this was due to an error that had not been raised by the compiler.

##Accomplishments that we're proud of
As a team of two software engineering students and one electrical engineering student, we are very amazed at how equal the percentage* of efforts we have put into this project. All three components melded extremely well together, and we can't imagine this project being as amazing as it is right now without any of the team members. We are especially proud of how interactive AVA feels when used by someone. We feel that this assistant prototype inspires us to look forward to a future where handier tools and companions will be available, and where holographic projections are omnipresent.

##What we learned
One of the biggest aspects we have learned from this project is the usage of databases and HTTP requests. Of course, two of us had taken courses of networks beforehand, but we had never gotten the chance to have this much hands-on experience. We were the creators, editors and maintainers of our database, and we had to coordinate each other regarding how information would have to be passed and understood between the different components. For the Android application, the usage of Google speech-to-text as well as the translate API was simple yet rewarding; this is due to the fact that we would never have had to learn them in such a short amount of time, and this pushed us to be more adaptive and open to learning about new technologies.

##What's next for ProjectAVA
We believe we are onto something revolutionary; a future where humans can be accompanied and helped by an omnipresent companion doesn't seem so far-fetched and idealistic anymore. The prototype we currently have is a first but solid of future increments. This design can be further solidified, improved in many ways, such as the presentation of the plastic pyramid. Furthermore, in a setting where time isn't so fleeting, it would be easier for us to breathe a little and stir up even more ideas to make the experience for users even more interactive and intuitive.

http://devpost.com/software/projectava
