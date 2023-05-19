using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metaface.Utilities
{
    public class EyeGazeHelper : MonoBehaviour
    {

        [SerializeField]
        OVREyeGaze leftEye;

        [SerializeField]
        OVREyeGaze rightEye;

        [SerializeField]
        private bool showRays = false;

        private Dictionary<OVREyeGaze, RaycastHit> eyeCache = new Dictionary<OVREyeGaze, RaycastHit>();

        void Update()
        {
            RaycastHit hitLeft, hitRight;
            UpdateEye(RaycastEye(leftEye, out hitLeft, 1000f), leftEye, hitLeft);
            UpdateEye(RaycastEye(rightEye, out hitRight, 1000f), rightEye, hitRight);
        }

        private void UpdateEye(bool didHit, OVREyeGaze eyeGaze, RaycastHit hit)
        {
            if (didHit)
            {
                if (!eyeCache.ContainsKey(eyeGaze))
                    eyeCache.Add(eyeGaze, hit);
                else
                    eyeCache[eyeGaze] = hit;
            }
            else
            {
                eyeCache.Remove(eyeGaze);
            }
        }

        /// <summary>
        /// Public helper function to get the current RayCastHit from the left and right eye gaze
        /// </summary>
        /// <param name="leftHit"></param>
        /// <param name="rightHit"></param>
        /// <returns>True if either left or right hit, false if neither hit</returns>
        public bool TryGetEyeGazeRaycast(out RaycastHit leftHit, out RaycastHit rightHit)
        {
            bool hasHit = false;
            leftHit = rightHit = default(RaycastHit);
            //Get the current eye hit from cache
            if (eyeCache.ContainsKey(leftEye))
            {
                leftHit = eyeCache[leftEye];
                hasHit = true;
            }
            if (eyeCache.ContainsKey(rightEye))
            {
                rightHit = eyeCache[rightEye];
                hasHit = true;
            }
            return hasHit;
        }


        /// <summary>
        /// Performs a raycast from the particular eye
        /// </summary>
        /// <param name="gaze"></param>
        /// <param name="hit"></param>
        /// <returns></returns>
        private bool RaycastEye(OVREyeGaze gaze, out RaycastHit hit, float distance = 1000f)
        {
            if (showRays)
                UnityEngine.Debug.DrawRay(gaze.transform.position, gaze.transform.forward * distance, Color.cyan);
            return Physics.Raycast(gaze.transform.position, gaze.transform.forward, out hit, distance);
        }

    }
}