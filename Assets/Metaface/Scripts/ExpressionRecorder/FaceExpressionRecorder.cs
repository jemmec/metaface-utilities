using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Records face express 
/// </summary>
public class FaceExpressionRecorder : MonoBehaviour
{

    [SerializeField]
    private OVRFaceExpressions ovrFaceExpressions;

    [SerializeField, Range(1, 60)]
    private float recordFPS = 60f;

    private bool isRecording = false;

    public bool _testRun = false;

    private void Update()
    {
        if (_testRun)
        {
            _testRun = false;
            StartRecording();
        }
    }

    public void StartRecording()
    {
        isRecording = true;
        StartCoroutine(RecordRoutine());
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    //Should I use a coroutine???
    private IEnumerator RecordRoutine()
    {
        float time = 0;
        float fraction = 1000 / recordFPS;
        while (isRecording)
        {
            timeTally += Time.deltaTime;
            time += Time.deltaTime * 1000;
            if (time >= fraction)
            {
                WriteFaceData();
                time = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    float timeTally = 0;

    private void WriteFaceData()
    {
        Debug.Log("Time:   " + timeTally + "s");
    }

}
