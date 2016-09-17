using UnityEngine;
using System.Collections;

public class DebugRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   this.gameObject.transform.Rotate(Vector3.up*10f*Time.deltaTime);
	}
}
