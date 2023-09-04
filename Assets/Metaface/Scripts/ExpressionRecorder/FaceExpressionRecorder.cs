using System;
using System.Collections;
using System.Runtime.InteropServices;
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

    private string filePath;

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

    private IEnumerator RecordRoutine()
    {
        string folderPath = System.IO.Path.Join(Application.dataPath, "FaceRecordings");
        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);
        filePath = System.IO.Path.Join(folderPath, "face_recording.txt");
        if (!System.IO.File.Exists(filePath))
            System.IO.File.Create(filePath).Close();

        //CLear file just for testing
        System.IO.File.WriteAllText(filePath, "");

        float time = 0;
        float timeTotal = 0;
        float fraction = 1000 / recordFPS;
        while (isRecording)
        {
            time += Time.deltaTime * 1000;
            timeTotal += Time.deltaTime;
            if (time >= fraction)
            {
                WriteFaceData(timeTotal);
                time = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void WriteFaceData(float timeStamp)
    {
        var expressions = Enum.GetNames(typeof(OVRFaceExpressions.FaceExpression));
        string str = $"{timeStamp};";
        for (int i = 0; i < expressions.Length; i++)
        {
            //Try get the weight
            float weight;
            if (ovrFaceExpressions.TryGetFaceExpressionWeight((OVRFaceExpressions.FaceExpression)i, out weight))
                str += $"{weight};";
            else
                str += "0;";
        }
        str += "\n";
        System.IO.File.AppendAllText(filePath, str);
    }

}
