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

    public bool _testRecord = false;

    private void Update()
    {
        if (_testRecord)
        {
            _testRecord = false;
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
        Debug.Log("Starting face recording");

        string folderPath = System.IO.Path.Join(Application.dataPath, "FaceRecordings");
        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);
        filePath = System.IO.Path.Join(folderPath, "test_face_recording.txt");
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

        Debug.Log($"Recoded {timeTotal} seconds of face");

    }

    private void WriteFaceData(float timeStamp)
    {
        var expressions = Enum.GetNames(typeof(OVRFaceExpressions.FaceExpression));
        string str = "";
        for (var expressionIndex = 0;
                expressionIndex < (int)OVRFaceExpressions.FaceExpression.Max;
                ++expressionIndex)
        {
            //Try get the weight
            float weight;
            if (ovrFaceExpressions.TryGetFaceExpressionWeight((OVRFaceExpressions.FaceExpression)expressionIndex, out weight))
                str += $"{weight};";
            else
                str += "0;";
        }
        str += "\n";
        System.IO.File.AppendAllText(filePath, str);
    }

}
