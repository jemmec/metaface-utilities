using UnityEngine;
using UnityEngine.Events;

namespace Metaface.Utilities
{
    /// <summary>
    /// Creates a blinking event that can be listened to
    /// </summary>
    public class BlinkHelper : MonoBehaviour
    {
        [Header("Required Classes")]

        [SerializeField]
        private OVRFaceExpressions faceExpressions;

        [SerializeField]
        private OVREyeGaze leftEye, rightEye;

        [Header("Settings")]

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("The eye close threshold required for a blink to be considered")]
        private float expressionCloseThreshold;

        [Header("Events")]

        [SerializeField]
        private EyeCloseEvent onEyeClose = new EyeCloseEvent();

        /// <summary>
        /// On eye closed event
        /// </summary>
        public EyeCloseEvent OnEyeClose => onEyeClose;

        [SerializeField]
        private EyeOpenEvent onEyeOpen = new EyeOpenEvent();

        /// <summary>
        /// On eye open event
        /// </summary>
        public EyeOpenEvent OnEyeOpen => onEyeOpen;

        [SerializeField]
        private EyeBlinkEvent onEyeBlink = new EyeBlinkEvent();

        /// <summary>
        /// On eye blink event
        /// </summary>
        public EyeBlinkEvent OnEyeBlink => onEyeBlink;

        /// <summary>
        /// Blink event arguments passed through the event when the user blinks
        /// </summary>
        [System.Serializable]
        public class BlinkEventArgs : System.EventArgs
        {
            public float LeftCloseWeight { get; set; }
            public float RightCloseWeight { get; set; }
        }

        /// <summary>
        /// Eye close event class
        /// </summary>
        [System.Serializable]
        public class EyeCloseEvent : UnityEvent { }

        /// <summary>
        /// Eye open event class
        /// </summary>
        [System.Serializable]
        public class EyeOpenEvent : UnityEvent { }

        /// <summary>
        /// Blink event class
        /// </summary>
        [System.Serializable]
        public class EyeBlinkEvent : UnityEvent<BlinkEventArgs> { }
    }
}