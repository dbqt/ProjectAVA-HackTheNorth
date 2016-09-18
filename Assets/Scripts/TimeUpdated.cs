using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeUpdated : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.GetComponent<TextMesh>().text = System.DateTime.Now.ToShortTimeString();
	
	}
}
