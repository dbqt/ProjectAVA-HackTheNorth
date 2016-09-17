using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class TTS : MonoBehaviour {
 	public string words = "Hello";
     
    IEnumerator Start ()
    {
        // Remove the "spaces" in excess
        Regex rgx = new Regex ("\\s+");
        // Replace the "spaces" with "% 20" for the link Can be interpreted
        string result = rgx.Replace (words, "%20");
        string url = "http://translate.google.com/translate_tts?tl=en&q=" + result;
        WWW www = new WWW (url);
        yield return www;
        GetComponent<AudioSource>().clip = www.GetAudioClip (false, false, AudioType.MPEG);
        GetComponent<AudioSource>().Play ();
        Debug.Log("lmao");
    }
}
