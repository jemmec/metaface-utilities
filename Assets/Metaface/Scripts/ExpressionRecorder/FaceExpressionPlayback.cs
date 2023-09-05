using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceExpressionPlayback : MonoBehaviour
{
    [SerializeField]
    private TextAsset playbackRecording;

    [SerializeField]
    private FacePlaybackSystem facePlaybackSystem;

    [SerializeField, Range(1, 60)]
    private float recordFPS = 60f;

    private bool isPlayback = false;

    public bool _testPlayback = false;

    private void Update()
    {
        if (_testPlayback)
        {
            _testPlayback = false;
            StartPlayback();
        }
    }

    private void StartPlayback()
    {
        if (playbackRecording != null)
        {
            isPlayback = true;
            StartCoroutine(PlaybackRoutine());
        }
    }

    private IEnumerator PlaybackRoutine()
    {
        Debug.Log("Starting playback");

        var lines = playbackRecording.text.Split("\n");

        float time = 0;
        float timeTotal = 0;
        float fraction = 1000 / recordFPS;

        int lineIdx = 0;
        while (isPlayback && lineIdx < lines.Length)
        {
            time += Time.deltaTime * 1000;
            timeTotal += Time.deltaTime;
            if (time >= fraction)
            {
                PlaybackFaceData(lines[lineIdx]);
                time = 0;
                lineIdx++;
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log($"Playback complete in {timeTotal} seconds");
    }

    private void PlaybackFaceData(string faceDataLine)
    {
        var weights = new float[(int)OVRFaceExpressions.FaceExpression.Max];
        if (!string.IsNullOrWhiteSpace(faceDataLine))
        {
            var split = faceDataLine.Split(";");
            for (var expressionIndex = 0;
                expressionIndex < (int)OVRFaceExpressions.FaceExpression.Max;
                ++expressionIndex)
            {
                if (!float.TryParse(split[expressionIndex], out weights[expressionIndex]))
                {
                    weights[expressionIndex] = 0;
                }
            }
        }
        facePlaybackSystem.ApplyFaceWeight(weights);
    }

}
