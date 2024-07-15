using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MixedReality.Toolkit.Input;
using System.Diagnostics;
using System;


public class eyetrack : MonoBehaviour
{
    [SerializeField]
    private GazeInteractor gazeInteractor;

    [SerializeField]
    private GameObject objectofinterest;

    // to write tracking data 
    private StreamWriter trackerdata;

    private void Awake()
    {
        // file to save eyetracking data
        var filepath = Path.Combine(Application.persistentDataPath, "eyetrackerdata.csv");
        // initialise StreamWrite to write to CSV file
        trackerdata = new StreamWriter(filepath);
        // ensuring dat ais written immediately 
        trackerdata.AutoFlush = true;
    }

    private void Update()
    {
        // constructing ray from gaze origin in the direction of gaze extended by 3 units  
        var ray = new Ray(gazeInteractor.rayOriginTransform.position, 
            gazeInteractor.rayOriginTransform.forward * 3);
        // perform rayacst and stores info of hits 
        Physics.Raycast(ray, out var hit);
        // write hits to CSV
        WriteTrackingPoint(hit.point);
    }

    // method to write to CSV
    private void WriteTrackingPoint(Vector3 hitPoint)
    {
        // writes the x, y, z coordinates 
        trackerdata.WriteLine(FormattableString.Invariant($"{hitPoint.x}, {hitPoint.y}, {hitPoint.z}"));
    }

    // called when script instance is destroyed to ensure
    // all data is flushed ro file and is properly closed 
    private void OnDestroy()
    {
        trackerdata.Close();
    }
}
