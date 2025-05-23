using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cueControl : MonoBehaviour
{
    // cues and flicker GameObjects
    public GameObject cueLeft;
    public GameObject cueRight;
    public GameObject Flicker1;
    public GameObject Flicker2;

    // experiment start delay
    public float startDelay = 5;
    // cue duration
    public float cueDuration = 2f;
    // total number of trials
    public int trialcount = 10;
    // base ITI duration
    public float interTrialInterval = 1f;
    // ITI with a jitter
    private float iti;

    // reference from flickerControl 
    private float flickerPeriod;
    private float freq1;
    private float freq2;
    private flickerControl flickerControl1;
    private flickerControl flickerControl2;

    private ConnectionManager connectionManager;



    void Start()
    {
        // initialise all gameObjects inactive
        cueLeft.SetActive(false);
        cueRight.SetActive(false);
        Flicker1.SetActive(false);
        Flicker2.SetActive(false);

        // get reference from flickerControl script
        flickerControl1 = Flicker1.GetComponent<flickerControl>();
        flickerControl2 = Flicker2.GetComponent<flickerControl>();
        flickerPeriod = flickerControl1.trialDuration;
        freq1 = flickerControl1.flickerFrequency;
        freq2 = flickerControl2.flickerFrequency;

        // start experiment loop
        StartCoroutine(RunExperiment());
    }

    //experiment loop coroutine
    IEnumerator RunExperiment()
    {
        yield return new WaitForSeconds(startDelay);
        // loop over trials 
        for (int i = 0; i < trialcount; i++)
        {
            // run trials
            yield return StartCoroutine(BeginTrial());
            // run iti with a jitter
            iti = interTrialInterval + Random.value;
            yield return new WaitForSeconds(iti);
        }
    }

    // single trial logic
    IEnumerator BeginTrial()
    {
        // initialise string marker 
        string markerstr = "";

        // randomise the cues
        if (Random.value > 0.5f)
        {
            // show left cue
            cueLeft.SetActive(true);
            yield return new WaitForSeconds(cueDuration);
            cueLeft.SetActive(false);

            // set marker string as left stim freq
            markerstr = freq1.ToString();
        }
        else
        {
            // show right cue
            cueRight.SetActive(true);
            yield return new WaitForSeconds(cueDuration);
            cueRight.SetActive(false);

            // set marker string as right stim freq
            markerstr = freq2.ToString();
        }

        // Send the marker using the connection manager
        if (connectionManager != null)
        {
            connectionManager.Send(markerstr, "MarkerStream");
            Debug.Log("Sending marker: " + markerstr);
        }

        // show flickers
        Flicker1.SetActive(true);
        Flicker2.SetActive(true);

        // running the flicker script
        flickerControl1.StartFlickering();
        flickerControl2.StartFlickering();

        // running until the flicker period
        yield return new WaitForSeconds(flickerPeriod);

        // hide flicker
        Flicker1.SetActive(false);
        Flicker2.SetActive(false);
    }
}