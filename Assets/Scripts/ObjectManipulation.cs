using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManipulation : MonoBehaviour {

    public float ZoomRate;
    public float RotationSpeed;
    public float DebugValue;
    public Animator MainAnimator;
    public Animator AnaAnimator;
    public GameObject[] AllModels;

    private int modelIndex;
    private bool isRotating;

	// Use this for initialization
	void Start () {
	   modelIndex = 0;
       isRotating = false;
       LoadModel();
       //ChangeModel(0);
	}
	
	// Update is called once per frame
	void Update () {

        if(isRotating) {
            this.gameObject.transform.Rotate(Vector3.up*RotationSpeed*Time.deltaTime);
        }
	
	}

    public void SetZoom(float zoom) {
        // use lerp to zoom gradually
        float curr = this.gameObject.transform.localScale.x;
        float newVal = Mathf.Lerp(curr, zoom, ZoomRate);
        this.gameObject.transform.localScale = new Vector3(newVal, newVal, newVal);
    }

    public void SetRotation(bool newIsRotating) {
        isRotating = newIsRotating;
    }

    public void AdjustRotation(Vector3 direction) {
        //look at?
    }

    public void Hide() {
        MainAnimator.SetTrigger("HideTrigger");
        Invoke("LoadModel", 2f);
    }

    public void Show() {

        MainAnimator.SetTrigger("ShowTrigger");
    }

    public void SetAnaAnimation(string trigger) {
        Debug.Log("set animation " + trigger);
        switch (trigger) {
            case "greet":
            Debug.Log("case greet");
                AnaAnimator.SetTrigger("GreetTrigger");
                break;
            case "talk":
            Debug.Log("case talk");
                AnaAnimator.SetTrigger("TalkTrigger");
                break;
            case "idle":
                //MainAnimator.SetTrigger("GreetTrigger");
                break;
            case "search":
            Debug.Log("case search");
                AnaAnimator.SetTrigger("SearchTrigger");
                break;
            case "dance":
            Debug.Log("case dance");
                //MainAnimator.SetTrigger("HideTrigger");
                AnaAnimator.SetTrigger("DanceTrigger");
                break;
            case "no":
            Debug.Log("case no");
                AnaAnimator.SetTrigger("NoTrigger");
                break;

            default:
                //MainAnimator.SetTrigger("IdleTrigger");
                break;
        }
    }

    public void ChangeModel(int newModelIndex) {
        modelIndex = newModelIndex;
        Hide();
    }

    public void LoadModel() {
        foreach(var ob in AllModels) {
            ob.SetActive(false);
        }
        AllModels[modelIndex].SetActive(true);
        /*if(this.gameObject.transform.childCount > 0){ 
            Destroy(this.gameObject.transform.GetChild(0).gameObject);
        }

        if(modelIndex >= 0) {
            GameObject o = Instantiate(AllModels[modelIndex], new Vector3(0f, (modelIndex == 0) ? this.transform.position.y-8.5f:this.transform.position.y, 0f), Quaternion.identity) as GameObject;
            o.transform.parent = this.gameObject.transform;
            if(modelIndex == 0) {
                AnaAnimator = o.GetComponent<Animator>();
            }
        }*/
        //show nothing otherwise
        Show();
    }
}
