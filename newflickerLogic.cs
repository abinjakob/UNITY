IEnumerator FlickerBox()
{
    float startTime = Time.time;
    bool isOn = true;

    // Number of frames per half cycle
    int framesPerHalfCycle = Mathf.RoundToInt((1f / flickerFrequency) * Application.targetFrameRate / 2);
    
    while (Time.time - startTime < trialDuration)
    {
        rend.material.color = isOn ? onColor : offColor;
        isOn = !isOn;

        // Wait for the calculated number of frames
        for (int i = 0; i < framesPerHalfCycle; i++)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    rend.material.color = offColor;
}
