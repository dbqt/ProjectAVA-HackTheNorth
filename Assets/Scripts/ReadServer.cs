using UnityEngine;

using UnityEngine.Networking;

using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

public class ReadServer : MonoBehaviour {

	private float internalZoom;
	private string lastCommand;
	int counter = 0;

	// Use this for initialization
	void Start () {
		internalZoom = 0f;
		lastCommand = "";
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(counter >= 200){
			StartCoroutine("ReadPi");
			StartCoroutine("ReadVoice");

			if(internalZoom == 0f) {
			this.GetComponent<ObjectManipulation>().SetZoom(0.75f);

			}
			else {
				this.GetComponent<ObjectManipulation>().SetZoom(1.75f);

			}
			counter = 0;
		} else {
			counter++;
		}

	}

	IEnumerator ReadPi() {
		WWW pi = new WWW("https://projectava-1de83.firebaseio.com/data.json");
		yield return pi;
		Debug.Log("PI" + pi.text);
		var jsonPi = JSON.Parse(pi.text);

		internalZoom = float.Parse(jsonPi["yellowFeel"]);
		this.GetComponent<ObjectManipulation>().SetRotation(int.Parse(jsonPi["redFeel"]) != 0);//if != 0, true


		//this.GetComponent<ObjectManipulation>().SetZoom(2f);
	}


	IEnumerator ReadVoice() {
		 WWW voice = new WWW("https://projectava-1de83.firebaseio.com/command.json");
		 yield return voice;
		 Debug.Log("RV Voice: " + voice.text);
		 Debug.Log("RV Last command: " + lastCommand);
		 if(lastCommand != voice.text){
		 	Debug.Log("last command not equal to voice text");
		 	lastCommand = voice.text;
			ExecuteCommand(voice.text);
		 } 

	}

	private void ExecuteCommand(string command) {

			Debug.Log("EC new command "+ command);
			//new command
			if(command.ToLower().Contains("weather")){
				if(command.ToLower().Contains("sun") || command.ToLower().Contains("clear")) {
		            //sun
		            this.gameObject.GetComponent<ObjectManipulation>().ChangeModel(1);
			    }
			    else if(command.ToLower().Contains("cloud")) {
		            //cloud
		            this.gameObject.GetComponent<ObjectManipulation>().ChangeModel(2);
			    }
			    else {
		            //rain
		            this.gameObject.GetComponent<ObjectManipulation>().ChangeModel(3);
	       		}
			}
			else if(command.ToLower() == "time") {
				this.gameObject.GetComponent<ObjectManipulation>().ChangeModel(-1);
				//show time
			}
			else {
				this.gameObject.GetComponent<ObjectManipulation>().ChangeModel(0);
			}
		
	}
}
