/*
 * Sound Controller  
 * 
 * Controls more complex audio clips, usually ones that play for a longer duration or ones that should persist through levels
 * 
 * Audio Clip Reference:
 * [0] : 
 * [1] : 
 * 
 * 
*/

using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
    
    private static AudioSource[] sources;
    private static int[] sourceFades;                    // 0 - nothing, 1 - fade in, 2 - fade out    
    private static float[] sourceFadeDurations;          // How long each fade will last
    private static float[] sourceFadeTimes;              // The current time for each fade

    private static float[] sourceStartVolume;            // The volume at which a fade started
    private static float[] sourceVolumes;                // Volume of the given clip  

    void Awake() {

        // Creates an array of all audiosources attached to this object
        //  *based on the order in which they appear in the inspector currently v4.1.2f1*
        sources = GetComponents<AudioSource>();

        sourceFades = new int[sources.Length];
        sourceFadeDurations = new float[sources.Length];
        sourceFadeTimes = new float[sources.Length];

        sourceStartVolume = new float[sources.Length];
        sourceVolumes = new float[sources.Length];
        // Default all volumes to 1
        for (int i = 0; i < sourceVolumes.Length; i++) {
            sourceVolumes[i] = 1.0F;
        }

        // This object persists
        DontDestroyOnLoad(transform.gameObject);
    }    

    void Update() {

        checkFades();
    }


    // Play AudioSource at index i
    public static void play(int i) {

        if (i <= sources.Length)        // Check for bounds
            sources[i].Play();          // Play the given sound
    }

    // Stop all AudioSource
    public static void stop() {
        
        for (int i = 0; i < sources.Length; i++) {
            sources[i].Stop();
        }        
    }

    // Stop AudioSource at index i
    public static void stop(int i) {

        if (i < sources.Length) {       // Check for bounds
            sources[i].Stop();          // Play the given sound
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }
    }

    // Check sourceFades array for whether any of the AudioSources are fading in or out
    private void checkFades() {

        for (int i = 0; i < sourceFades.Length; i++) {

            if (sourceFades[i] == 1) {

                if (sources[i].volume != sourceVolumes[i]) {
                    sources[i].volume = Mathf.Lerp(sourceStartVolume[i], sourceVolumes[i], sourceFadeTimes[i]);

                    if (sourceFadeTimes[i] < 1) { // while t below the end limit...
                        // increment it at the desired rate every update:
                        sourceFadeTimes[i] += Time.deltaTime / sourceFadeDurations[i];
                    }
                }
                else {
                    sourceFades[i] = 0;
                }
            }
            else if (sourceFades[i] == 2) {

                if (sources[i].volume != 0) {
                    sources[i].volume = Mathf.Lerp(sourceStartVolume[i], 0, sourceFadeTimes[i]);

                    if (sourceFadeTimes[i] < 1) { // while t below the end limit...
                        // increment it at the desired rate every update:
                        sourceFadeTimes[i] += Time.deltaTime / sourceFadeDurations[i];
                    }
                }
                else {
                    sourceFades[i] = 0;
                    sources[i].Stop();
                }
            }
        }       
    }

    // Fade in AudioSource at index i
    public static void setFadeIn(int i) {

        if (i < sources.Length) {
            sourceFadeTimes[i] = 0;
            sourceStartVolume[i] = 0;
            sourceFades[i] = 1;

            sources[i].Play();
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }
    }

    // Fade out AudioSource at index i, assumes AudioSource[i] is already playing
    public static void setFadeOut(int i) {

        if (i < sources.Length) {
            sourceFadeTimes[i] = 0;
            sourceStartVolume[i] = sources[i].volume;
            sourceFades[i] = 2;
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }
    }

    // Fade in AudioSource at index i, over t seconds
    public static void setFadeIn(int i, float t) {

        if (i < sources.Length) {
            sourceFadeTimes[i] = 0;
            sourceStartVolume[i] = 0;
            sourceFades[i] = 1;
            sourceFadeDurations[i] = t;

            sources[i].Play();
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }
    }

    // Fade out AudioSource at index i, over t seconds, assumes AudioSource[i] is already playing
    public static void setFadeOut(int i, float t) {

        if (i < sources.Length) {
            sourceFadeTimes[i] = 0;
            sourceStartVolume[i] = sources[i].volume;
            sourceFades[i] = 2;
            sourceFadeDurations[i] = t;
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }
    }

    // Set volume for AudioSource at index i
    public static void setVolume(int i, float v) {

        if (i < sources.Length) {
            sourceVolumes[i] = v;
            sources[i].volume = v;
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }
    }

    // Set loop for AudioSource at index i
    public static void setLoop(int i,bool b) {

        if (i < sources.Length) {
            sources[i].loop = b;
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }
    }

    // Set play on awake for AudioSource at index i
    public static void setPlayOnAwake(int i, bool b) {

        if (i < sources.Length) {
            sources[i].playOnAwake = b;
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }
    }

    // return isPlaying for AudioSource at index i
    public static bool isPlaying(int i) {

        if (i < sources.Length) {
            return sources[i].isPlaying;
        }
        else {
            Debug.Log("Index " + i + " is out of bounds.");
        }

        return false;

    }
}
