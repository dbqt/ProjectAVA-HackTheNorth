using UnityEngine;

using UnityEngine.Networking;

using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

public class ReadServer : MonoBehaviour {

	private float internalZoom;
	private string lastCommand;
	private string lastExpression;
	int counter = 0;

	// Use this for initialization
	void Start () {
		internalZoom = 0f;
		lastExpression = "";
		lastCommand = "";
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(counter >= 30){
			StartCoroutine("ReadPi");
			StartCoroutine("ReadVoice");
			StartCoroutine("ReadExpression");

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

	IEnumerator ReadExpression() {
		 WWW exp = new WWW("https://projectava-1de83.firebaseio.com/expression.json");
		 yield return exp;
		 Debug.Log("RV exp: " + exp.text);
		 if(lastExpression != exp.text){
		 	Debug.Log("last exp not equal to exp text");
		 	lastExpression = exp.text;
			ExecuteExpression(exp.text);
		 } 
		 
		//ExecuteExpression(exp.text); 
	}

	private void ExecuteExpression(string expression) {
		this.gameObject.GetComponent<ObjectManipulation>().SetAnaAnimation(expression.ToString());
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
		else if(command.ToLower().Contains("time")) {
			Debug.Log("time????");
			this.gameObject.GetComponent<ObjectManipulation>().ChangeModel(4);
			//show time
		}
		else {
			this.gameObject.GetComponent<ObjectManipulation>().ChangeModel(0);
		}	
	}
}
