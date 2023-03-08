using UnityEngine;

namespace Metaface.Utilities
{
    /// <summary>
    /// Runs a calibration procedure to calibrate user eye blinking
    /// options to serialize calibration data to file
    /// </summary>
    public class BlinkCalibration : MonoBehaviour
    {
        [SerializeField]
        [Range(2, 10)]
        private int blinkCount = 5;


        [System.Serializable]
        public class CalibrationData
        {
            public BlinkBlendWeights leftQuick;

            public BlinkBlendWeights rightQuick;

            public BlinkBlendWeights leftSlow;

            public BlinkBlendWeights rightSlow;

            public class BlinkBlendWeights
            {



            }

        }
    }
}