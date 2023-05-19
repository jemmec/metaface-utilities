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

        [SerializeField]
        private float maxGazeDistance = 1000f;

        private Dictionary<OVREyeGaze, EyeGazeTarget> eyeCache = new Dictionary<OVREyeGaze, EyeGazeTarget>();

        void Update()
        {
            RaycastHit hitLeft, hitRight;
            UpdateEye(
                RaycastEye(
                    leftEye,
                    out hitLeft,
                    maxGazeDistance),
                leftEye,
                hitLeft);
            UpdateEye(
                RaycastEye(
                    rightEye,
                    out hitRight,
                    maxGazeDistance),
                rightEye,
                hitRight);
        }

        private void UpdateEye(bool didHit, OVREyeGaze eyeGaze, RaycastHit hit)
        {
            if (didHit)
            {
                EyeGazeTarget target = hit.transform.gameObject.GetComponent<EyeGazeTarget>();
                if (target)
                {
                    if (!eyeCache.ContainsKey(eyeGaze))
                        eyeCache.Add(eyeGaze, target);
                    else
                        eyeCache[eyeGaze] = target;
                    return;
                }
            }
            eyeCache.Remove(eyeGaze);
        }

        /// <summary>
        /// Public helper function to get the current 
        /// left and right RayCastHit for the CURRENT frame.
        /// </summary>
        /// <param name="leftTarget"></param>
        /// <param name="rightTarget"></param>
        /// <returns>True if either left or right hit, false if neither hit</returns>
        public bool TryGetEyeGazeRaycast(out EyeGazeTarget leftTarget, out EyeGazeTarget rightTarget)
        {
            bool hasHit = false;
            leftTarget = rightTarget = default(EyeGazeTarget);
            //Get the current eye hit from cache
            if (eyeCache.ContainsKey(leftEye))
            {
                leftTarget = eyeCache[leftEye];
                hasHit = true;
            }
            if (eyeCache.ContainsKey(rightEye))
            {
                rightTarget = eyeCache[rightEye];
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
                UnityEngine.Debug.DrawRay(gaze.transform.position, gaze.transform.forward * distance, Color.red);
            return Physics.Raycast(gaze.transform.position, gaze.transform.forward, out hit, distance);
        }

    }
}