using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManipulation : MonoBehaviour {

    public float ZoomRate;
    public float DebugValue;
    public GameObject[] AllModels;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.anyKey)
        {
            SetZoom(DebugValue);
        }
	
	}

    public void SetZoom(float zoom) {
        // use lerp to zoom gradually
        float curr = this.gameObject.transform.localScale.x;
        float newVal = Mathf.Lerp(curr, zoom, ZoomRate);
        this.gameObject.transform.localScale = new Vector3(newVal, newVal, newVal);
    }

    public void AdjustRotation(Vector3 direction) {
        //look at?
    }

    public void LoadModel(int modelIndex) {
        if(this.gameObject.transform.childCount > 0){ 
        Destroy(this.gameObject.transform.GetChild(0).gameObject);
         }
        GameObject o = Instantiate(AllModels[modelIndex]) as GameObject;
        this.gameObject.transform.parent = o.transform;
    }
}
