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
                if (left && right && left == right)
                {
                    //Turn green
                }
                else if (left)
                {
                    //Turn cyan   
                }
                else if (right)
                {
                    //Turn magenta
                }
            }
        }

    }

}