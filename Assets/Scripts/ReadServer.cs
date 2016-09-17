using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadServer : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
	   WWW w = new WWW("https://projectava-1de83.firebaseio.com/test.json");

       yield return w;
      
      Debug.Log(w.text);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
