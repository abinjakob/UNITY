using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class flicker : MonoBehaviour
{
    private Renderer rend;
    // flicker frequency 
    public float flickerFrequency = 15f;  
    // set color for on and off state 
    public Color onColor = Color.white;
    public Color offColor = Color.black;
    // total duration of the flicker 
    public float trialDuration = 60f;
    // variable to track time
    private float nextFlickerTime;
    // variable to track flicker cycles
    private float flickerDuration;


    void Start()
    {
        // to ensure consistent frame rate in UNITY
        Application.targetFrameRate = 60;
        // get renderer component attached to game object 
        rend = GetComponent<Renderer>();
        // as each state (on/off) lasts half the cycle
        flickerDuration = 1f / (2 * flickerFrequency);
        // initialise nextFlickerTime
        nextFlickerTime = Time.time;
        // start flicker coroutine
        StartCoroutine(FlickerBox());
    }


    IEnumerator FlickerBox()
    {
        // start time
        float startTime = Time.time; 
        // variable to track elapsed time
        float elapsedTime = 0f;
        bool isOn = true;

        // loop until elapsed time reaches flicker time
        while (elapsedTime < trialDuration)
        {
            if(Time.time >= nextFlickerTime)
            {
                // toggle color
                rend.material.color = isOn ? onColor : offColor;
                isOn = !isOn;
                // schedule next flicker time
                nextFlickerTime = Time.time + flickerDuration;
            }
            // update elapsed time
            elapsedTime = Time.time - startTime;
            // wait until next frame
            yield return null;
        }
        // ensure box ends in off state
        rend.material.color = offColor;
    }
}
