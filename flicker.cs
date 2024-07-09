using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flicker : MonoBehaviour
{
    private Renderer rend;
    // duration for 15Hz flicker (1/30 because we want 15 full cycles per second,
    // so each state (on/off) lasts half the cycle)
    public float flickerDuration = 1f / 30f;
    // set color for on and off state 
    public Color onColor = Color.white;
    public Color offColor = Color.black;
    // total duration of the flicker 
    public float trialDuration = 60f;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(FlickerBox());
    }

    IEnumerator FlickerBox()
    {
        // variable to track time
        float elapsedTime = 0f;
        // loop until elapsed time reaches flicker time
        while (elapsedTime < trialDuration)
        {
            // set to on state 
            rend.material.color = onColor;
            yield return new WaitForSeconds(flickerDuration);
            // set to offstate 
            rend.material.color = offColor;
            yield return new WaitForSeconds(flickerDuration);
            // increment elapsed time (each loop covers two flicker duration)
            elapsedTime += 2 * flickerDuration;
        }
        // ensure box ends in off state
        rend.material.color = offColor;
    }
}
