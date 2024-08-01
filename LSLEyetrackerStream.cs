using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using LSL4Unity.Utils;
using MixedReality.Toolkit.Input;

namespace LSL4Unity.Samples.Complex
{
    public class LSLEyetrackerStream : MonoBehaviour
    {
        // declare LSL outlet and stream info
        private StreamOutlet outlet;
        private StreamInfo streamInfo;

        // channel names for gaze direction and position
        private string[] channelNames = new string[] { "Timestamps", "GazeDirX", "GazeDirY", "GazeDirZ", "GazePosX", "GazePosY", "GazePosZ" };
        
        // NOTE: Gaze direction is the where the user is looking at and Gaze Position is the 
        // location is user's field where their gaze intersect. 
        // Check for more details:
        // https://github.com/abinjakob/UNITY/blob/main/EyeTracking_GazeDirectionsVSGazePosition.md

        // eye tracking sampling rate
        public float samplingRate = 30f;
        private float lastSampleTime = 0f;
        //private float srate = 1.0f / samplingRate; 

        // eye tracking data source
        [SerializeField]
        private GazeInteractor gazeInteractor;

        private void Awake()
        {
            float sampleInterval = 1.0f / samplingRate;
            Debug.Log("Sample time: " + sampleInterval);

            // create a new LSL stream info
            // info: stream name, stream type, channel count, irregular sampling rate, data format
            streamInfo = new StreamInfo("EyeTrackerStream", "EyeTracking", channelNames.Length, LSL.LSL.IRREGULAR_RATE, channel_format_t.cf_float32);

            // append channel names to stream info
            var desc = streamInfo.desc();
            var chns = desc.append_child("channels");
            foreach (var channelName in channelNames)
            {
                chns.append_child("channel").append_child_value("label", channelName);
            }

            // create LSL outlet
            outlet = new StreamOutlet(streamInfo);
        }

        private void Update()
        {
            float sampleInterval = 1.0f / samplingRate;
            // check if its time too sample data 
            if (Time.time - lastSampleTime >= sampleInterval)
            {
                // log last sample time 
                lastSampleTime = Time.time;

                // get current Gaze direction and position
                Vector3 gazeDirection = gazeInteractor.rayOriginTransform.forward;
                Vector3 gazePosition = gazeInteractor.rayOriginTransform.position;

                // prepare sample data 
                float[] sample = new float[channelNames.Length];
                sample[0] = Time.time;
                sample[1] = gazeDirection.x;
                sample[2] = gazeDirection.y;
                sample[3] = gazeDirection.z;
                sample[4] = gazePosition.x;
                sample [5] = gazePosition.y;
                sample [6] = gazePosition.z;

                // push sample to outlet
                outlet.push_sample(sample);
            }
        }

        private void OnDestroy()
        {
            outlet.Close();
        }

        private void OnApplicationQuit()
        {
            OnDestroy();
        }

    }
}
