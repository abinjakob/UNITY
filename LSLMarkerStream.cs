using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLMarkerStream : MonoBehaviour
{
    // declare LSL outlet and stream info
    private StreamOutlet outlet;
    private StreamInfo streamInfo;

    // Start is called before the first frame update
    void Start()
    {
        // create a new LSL stream info
        // info: stream name, stream type, channel count, irregular sampling rate, data format
        streamInfo = new StreamInfo("EventMarkers", "Markers", 1, LSL.LSL.IRREGULAR_RATE, channel_format_t.cf_string);
        
        // create LSL outlet
        outlet = new StreamOutlet(streamInfo);
    }

    // method to write markers to LSL outlet
    public void Write(string marker)
    {
        // create array to hold marker
        string[] sample = new string[1];
        sample[0] = marker; 

        // push sample to outlet
        outlet.push_sample(sample);
    }
}
