using UnityEngine;
using SimpleJSON;
using System.Collections;

public class WeatherService : MonoBehaviour 
{
    private string apikey = "efe27d29b9904894ab6150619161709";

    public GameObject ava;

	// Use this for initialization
	IEnumerator Start () {
        //N2L3G1 waterloo
	   WWW weather = new WWW("http://api.apixu.com/v1/current.json?key="+apikey+"&q=waterloo");//uwaterloo postal code
	   yield return weather;
       var jsonWeather = JSON.Parse(weather.text);

//sunny, light cloud
       string cond = jsonWeather["current"]["condition"]["text"];
       if(cond.ToLower().Contains("sun") || cond.ToLower().Contains("clear")) {
            //sun
            ava.GetComponent<ObjectManipulation>().LoadModel(1);
       }
       else if(cond.ToLower().Contains("cloud")) {
            //cloud
            ava.GetComponent<ObjectManipulation>().LoadModel(2);

       }
       else {
            //rain
            ava.GetComponent<ObjectManipulation>().LoadModel(3);

       }
       Debug.Log(jsonWeather["current"]["condition"]["text"]);
       Debug.Log(jsonWeather["current"]["temp_c"] + "C");
       Debug.Log(jsonWeather["current"]["temp_f"] + "F");

    }
	
	// Update is called once per frame
	void Update () {
	
	}



}
