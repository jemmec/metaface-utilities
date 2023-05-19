using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metaface.Utilities;

namespace Metaface.Debug
{
    public class EyeGazeDebugger : MonoBehaviour
    {
        [SerializeField]
        private EyeGazeHelper eyeGazeHelper;

        private void Update()
        {
            EyeGazeTarget left, right;
            if (eyeGazeHelper.TryGetEyeGazeRaycast(out left, out right))
            {
                //Hitting the same target
                if (left)
                {
                    var leftIndicator = left.GetComponent<EyeGazeIndicator>();
                    leftIndicator.SetColor(Color.cyan);
                }
                if (right)
                {
                    var rightIndicator = right.GetComponent<EyeGazeIndicator>();
                    rightIndicator.SetColor(Color.magenta);
                }
                if (left && right && left == right)
                {
                    var leftIndicator = left.GetComponent<EyeGazeIndicator>();
                    leftIndicator.SetColor(Color.green);
                    var rightIndicator = right.GetComponent<EyeGazeIndicator>();
                    rightIndicator.SetColor(Color.green);
                }
            }
        }
    }
}