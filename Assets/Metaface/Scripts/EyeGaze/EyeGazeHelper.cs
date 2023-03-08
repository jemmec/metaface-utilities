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

        private Dictionary<OVREyeGaze, RaycastHit> eyeCache = new Dictionary<OVREyeGaze, RaycastHit>();

        void Update()
        {

            RaycastHit hitLeft, hitRight;
            bool didHitLeft = RaycastEye(leftEye, out hitLeft);
            bool didHitRight = RaycastEye(rightEye, out hitRight);

            if (didHitLeft && didHitRight)
            {
                //Hitting the same target
                if (hitLeft.transform == hitRight.transform)
                {
                    //In most cases only should be used
                    //It implies both eyes are converging on the same object

                    if (hitLeft.transform.gameObject.name == "GazePoint")
                        hitLeft.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else
                {
                    //Use confidence to determin which eye wins
                    if (leftEye.Confidence >= rightEye.Confidence)
                    {
                        if (hitLeft.transform.gameObject.name == "GazePoint")
                            hitLeft.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }
                    else
                    {
                        if (hitRight.transform.gameObject.name == "GazePoint")
                            hitRight.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
                    }
                }
            }
            else if (didHitLeft)
            {
                if (hitLeft.transform.gameObject.name == "GazePoint")
                    hitLeft.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
            else if (didHitRight)
            {
                if (hitRight.transform.gameObject.name == "GazePoint")
                    hitRight.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
            }
        }


        /// <summary>
        /// Performs a raycast from the particular eye
        /// </summary>
        /// <param name="gaze"></param>
        /// <param name="hit"></param>
        /// <returns></returns>
        private bool RaycastEye(OVREyeGaze gaze, out RaycastHit hit)
        {
            return Physics.Raycast(gaze.transform.position, gaze.transform.forward, out hit, 1000f);
        }

    }
}