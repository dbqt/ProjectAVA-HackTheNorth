using UnityEngine;
using System.Collections;

public class ObjectManipulation : MonoBehaviour {

    public float ZoomRate;
    public float DebugValue;

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
}
