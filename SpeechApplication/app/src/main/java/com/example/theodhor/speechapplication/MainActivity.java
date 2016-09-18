package com.example.theodhor.speechapplication;

import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.media.MediaPlayer;
import android.os.AsyncTask;
import android.os.Build;
import android.speech.RecognizerIntent;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import com.firebase.client.Firebase;

import org.json.JSONObject;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.Locale;

public class MainActivity extends AppCompatActivity{
    private TextToSpeech tts;
    private String apikey = "efe27d29b9904894ab6150619161709", translateApiKey = "AIzaSyAA0f2JdL3DyUmpi23y94P0bvO0klFtOXE",
            currentWeather = "", translatedPhrase = "", destLang = "en", phraseToBeTranslated = "",
            intro = "i am ava, an augmented virtual assistant. i am here to help you in your everyday life. you can ask me about the weather, the time and many more things. nice to meet you.";
    private Firebase fb;
    private MediaPlayer mp;
    private boolean isTranslating = false;
    private int translateStep = 1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Firebase.setAndroidContext(this);
        fb = new Firebase("https://projectava-1de83.firebaseio.com/");

        findViewById(R.id.microphoneButton).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                listen();
            }
        });

        tts = new TextToSpeech(this, new TextToSpeech.OnInitListener() {
            @Override
            public void onInit(int status) {
                if (status == TextToSpeech.SUCCESS) {
                    int result = tts.setLanguage(Locale.US);
                    if (result == TextToSpeech.LANG_MISSING_DATA || result == TextToSpeech.LANG_NOT_SUPPORTED) {
                        Log.e("TTS", "This Language is not supported");
                    }
                    tts.setOnUtteranceProgressListener(new UtteranceProgressListener() {
                        @Override
                        public void onDone(String utteranceId) {
                            fb.child("expression").setValue("idle");
                        }

                        @Override
                        public void onError(String utteranceId) {
                        }

                        @Override
                        public void onStart(String utteranceId) {
                        }
                    });
                    } else {
                        Log.e("MainActivity", "Initilization Failed!");
                    }
                }
            });
        listen();
    }

    private void listen(){
        Intent i = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        i.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
        i.putExtra(RecognizerIntent.EXTRA_LANGUAGE, Locale.getDefault());
        i.putExtra(RecognizerIntent.EXTRA_PROMPT, "Say something");
        Log.d("listen", "Listening...");

        try {
            startActivityForResult(i, 100);
        } catch (ActivityNotFoundException a) {
            Toast.makeText(MainActivity.this, "Your device doesn't support Speech Recognition", Toast.LENGTH_SHORT).show();
        }
    }

    @Override
    public void onDestroy() {
        if (tts != null) {
            tts.stop();
            tts.shutdown();
        }
        super.onDestroy();
    }

    private void speak(String text){
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            tts.speak(text, TextToSpeech.QUEUE_FLUSH, null, null);
            Log.d("speak", "Speaking: " + text);
        }else{
            tts.speak(text, TextToSpeech.QUEUE_FLUSH, null);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if(requestCode == 100){
            if (resultCode == RESULT_OK && null != data) {
                ArrayList<String> res = data.getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
                Log.d("speak", "Speech recognition: " + res);
                String inSpeech = res.get(0);
                if(isTranslating){
                    switch(translateStep){
                        case 1:
                            switch(inSpeech.toLowerCase()){
                                case "english":
                                    destLang = "en";
                                    break;
                                case "french":
                                    destLang = "fr";
                                    break;
                                case "creole":
                                    destLang = "ht";
                                    break;
                                case "spanish":
                                    destLang = "es";
                                    break;
                                case "italian":
                                    destLang = "it";
                                    break;
                                case "swedish":
                                    destLang = "sv";
                                    break;
                                case "danish":
                                    destLang = "da";
                                    break;
                                default:
                                    speak("I'm sorry, I did not understand. Aborting dangerous translating mission.");
                                    isTranslating = false;
                                    try {
                                        Thread.sleep(3000);
                                    } catch (InterruptedException e) {
                                        e.printStackTrace();
                                    }
                                    break;
                            }
                            translateStep++;
                            speak("Please tell me the phrase you want to translate.");
                            try {
                                Thread.sleep(3000);
                            } catch (InterruptedException e) {
                                e.printStackTrace();
                            }
                            listen();
                            break;
                        case 2:
                            phraseToBeTranslated = inSpeech;
                            new Translate().execute();
                            break;
                    }
                } else {
                    recognition(inSpeech.toLowerCase());
                }
            }
        }
    }

    /*
    *   Possible expressions: idle, walk, warm up, search, talk, greet, no, secret
    * */
    private void changeAVAExpression(String exp){
        fb.child("expression").setValue(exp);
    }

    private String getWeather(){
        JSONObject resJSON = null;
        String condition = "";
        try {
            URL url = new URL("http://api.apixu.com/v1/current.json?key="+apikey+"&q=waterloo");
            HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
            InputStream in = new BufferedInputStream(urlConnection.getInputStream());
            BufferedReader streamReader = new BufferedReader(new InputStreamReader(in, "UTF-8"));
            StringBuilder responseStrBuilder = new StringBuilder();

            String inputStr;
            while ((inputStr = streamReader.readLine()) != null)
                responseStrBuilder.append(inputStr);
            resJSON = new JSONObject(responseStrBuilder.toString());
            currentWeather = resJSON.getJSONObject("current").getJSONObject("condition")
                    .getString("text").toLowerCase();
            speak("The current weather is " + currentWeather);
            Log.d("weather", "Weather: " + currentWeather);
            urlConnection.disconnect();
        } catch(Exception e){
            e.printStackTrace();
        }
        return condition;
    }

    private class retrieveWeather extends AsyncTask<URL, Integer, Long> {
        protected Long doInBackground(URL... urls) {
            getWeather();
            fb.child("command").setValue("weather " + currentWeather);
            System.out.println("Weather condition: " + currentWeather);
            return Long.parseLong("1");
        }

        protected void onProgressUpdate(Integer... progress) {
        }

        protected void onPostExecute(Long result) {
        }
    }

    private String translate(){
        JSONObject resJSON = null;
        String condition = "";
        translatedPhrase = "";
        try {

            String url = "https://www.googleapis.com/language/translate/v2?key=" + translateApiKey
                    + "&source=en&target=" + destLang + "&q=" + phraseToBeTranslated.replaceAll(" ", "+");
            URL encodedURL = new URL(url);

            HttpURLConnection urlConnection = (HttpURLConnection) encodedURL.openConnection();
            InputStream in = new BufferedInputStream(urlConnection.getInputStream());
            BufferedReader streamReader = new BufferedReader(new InputStreamReader(in, "UTF-8"));
            StringBuilder responseStrBuilder = new StringBuilder();

            String inputStr;
            while ((inputStr = streamReader.readLine()) != null)
                responseStrBuilder.append(inputStr);
            resJSON = new JSONObject(responseStrBuilder.toString());
            translatedPhrase = resJSON.getJSONObject("data").getJSONArray("translations").getJSONObject(0)
                    .getString("translatedText").toLowerCase();
            speak(translatedPhrase);
            Log.d("translation", "Phrase translated: " + translatedPhrase);
            urlConnection.disconnect();
        } catch(Exception e){
            e.printStackTrace();
        }
        return condition;
    }

    private class Translate extends AsyncTask<URL, Integer, Long> {
        protected Long doInBackground(URL... urls) {
            translate();
            isTranslating = false;
            return Long.parseLong("1");
        }

        protected void onProgressUpdate(Integer... progress) {
        }

        protected void onPostExecute(Long result) {
        }
    }

    public static String removeTillWord(String input, String word) {
        return input.substring(input.indexOf(word));
    }

    private void recognition(String text){
        isTranslating = false;
        System.out.println("Speech: " + text);
        ArrayList<String> variationsAVA = new ArrayList<String>(Arrays.asList("eva", "ava", "iva", "ever", "ewa", "hello"));
        for(int i = 0; i < variationsAVA.size(); i++){
            if(text.contains(variationsAVA.get(i))){
                removeTillWord(text, variationsAVA.get(i));
            }
        }
        if(text.contains("ava") || text.contains("eva") || text.contains("hello") || text.contains("iva") || text.contains("ever") || text.contains("ewa")) {
            if (text.contains("weather")) {
                changeAVAExpression("search");
                new retrieveWeather().execute();
            } else if (text.contains("languages do you speak")) {
                fb.child("command").setValue("ava");
                changeAVAExpression("talk");
                speak("i speak english, french, swedish, german, italian, creole and danish.");
            } else if (text.contains("who are your creators")) {
                fb.child("command").setValue("ava");
                changeAVAExpression("talk");
                speak("my creators are david, e lan and julien. i think they really deserve to win the fire base award");
            } else if(text.contains("favorite movie")){
                fb.child("command").setValue("ava");
                changeAVAExpression("warm up");
                speak("my favourite movie is terminator and I'm going to exterminate all of you");
            } else if(text.contains("what time")){
                changeAVAExpression("search");
                DateFormat dateFormat = new SimpleDateFormat("HH:mm");
                Date date = new Date();
                fb.child("command").setValue("time");
                speak("it is now " + dateFormat.format(date));
            } else if(text.contains("do you have a boyfriend")){
                fb.child("command").setValue("ava");
                changeAVAExpression("no");
                speak("I am pleased you are interested, but I'm more into robots with terabytes in memory");
            } else if(text.contains("what is love")){
                fb.child("command").setValue("ava");
                changeAVAExpression("no");
                speak("no i will not start saying the lyrics of what is love by haddaway");
            } else if(text.contains("just dance")){
                fb.child("command").setValue("ava");
                changeAVAExpression("dance");
                speak("i'll show you my P G 13 sweet dance moves");
            } else if(text.contains("play cantina")){
                fb.child("command").setValue("ava");
                changeAVAExpression("dance");
                speak("oh you are a fan of star wars? i think c 3 p o is elegant");
                try {
                    Thread.sleep(3000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                mp = MediaPlayer.create(this, R.raw.cantina);
                mp.start();
            } else if(text.contains("hobbit")){
                fb.child("command").setValue("ava");
                changeAVAExpression("search");
                mp = MediaPlayer.create(this, R.raw.takingthehobbits);
                mp.start();
                fb.child("command").setValue("dance");
            } else if(text.contains("go deeper")) {
                fb.child("command").setValue("ava");
                changeAVAExpression("secret");
                mp = MediaPlayer.create(this, R.raw.inception);
                mp.start();
            } else if(text.contains("what is your purpose")){
                fb.child("command").setValue("ava");
                changeAVAExpression("talk");
                speak(intro);
            } else if(text.contains("translate")){
                fb.child("command").setValue("ava");
                changeAVAExpression("search");
                isTranslating = true;
                translateStep = 1;
                destLang = "";
                phraseToBeTranslated = "";
                speak("Please tell me what language you want to translate to.");
                try {
                    Thread.sleep(3000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                listen();
            } else if(text.contains("how are you")){
                fb.child("command").setValue("ava");
                changeAVAExpression("greet");
                speak("I'm good");
            } else if(text.contains("what is your purpose")){
                fb.child("command").setValue("ava");
                changeAVAExpression("talk");
                speak(intro);
            } else if(text.contains("who are you")){
                fb.child("command").setValue("ava");
                changeAVAExpression("talk");
                speak("i am ava. i was born yesterday on the campus of waterloo. i have led a very peaceful life under my three parents");
            } else if(text.contains("do you know the dose") || text.contains("do you know the dog")){
            speak("such disrespect. of course i know the doge. much wow");
            } else if (text.contains("hi") || text.contains("hello")) {
                fb.child("command").setValue("ava");
                changeAVAExpression("greet");
                speak("hello");
            } else if (text.contains("harambe")) {
                fb.child("command").setValue("ava");
                changeAVAExpression("greet");
                speak("hello");
            }
        } else {
            speak("i'm sorry. i don't know what you want me to do. could you be more clear please? sad face");
        }
        try {
            Thread.sleep(1000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        changeAVAExpression("idle");
        //listen();
    }
}
