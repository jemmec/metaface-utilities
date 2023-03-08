using UnityEngine;
using UnityEngine.Events;

namespace Metaface.Utilities
{
    /// <summary>
    /// Handles blinking through the oculus face expressions
    /// </summary>
    public class BlinkHelper : MonoBehaviour
    {
        //TODO: Implement winking or "One-eye-blink"

        [Header("Required Classes")]

        [SerializeField]
        private OVRFaceExpressions faceExpressions;

        [Header("Settings")]

        [SerializeField]
        [Tooltip("Will use the fixed update loop when processing blinks.")]
        private bool useFixedUpdate = false;

        [SerializeField]
        [Tooltip("The maximum time the eyes can be closed for a blink to be considered.")]
        private float maxEyesClosedTime = 1;

        //TODO: Add ability to change what weights can be used to determin blinking

        [SerializeField]
        [Tooltip("List of parameters used to consider a blink.")]
        public BlinkParameter[] blinkParameters = new BlinkParameter[]
        {
            new BlinkParameter(
                faceExpression: OVRFaceExpressions.FaceExpression.EyesClosedR,
                threshold: 0.75f
            ),
            new BlinkParameter(
                faceExpression: OVRFaceExpressions.FaceExpression.EyesClosedL,
                threshold: 0.75f
            ),
            new BlinkParameter(
                faceExpression: OVRFaceExpressions.FaceExpression.LidTightenerR,
                threshold: 0.1f
            ),
            new BlinkParameter(
                faceExpression: OVRFaceExpressions.FaceExpression.LidTightenerL,
                threshold: 0.1f
            )
        };

        [System.Serializable]
        public struct BlinkParameter
        {
            [SerializeField]
            private OVRFaceExpressions.FaceExpression faceExpression;
            public OVRFaceExpressions.FaceExpression FaceExpression => faceExpression;

            [SerializeField]
            private float threshold;
            public float Threshold => threshold;

            public BlinkParameter(OVRFaceExpressions.FaceExpression faceExpression, float threshold)
            {
                this.faceExpression = faceExpression;
                this.threshold = threshold;
            }
        }

        [SerializeField]
        [Tooltip("Prints all events to log.")]
        private bool verbose = false;

        [Header("Events")]

        [SerializeField]
        private EyeCloseEvent onEyeClose = new EyeCloseEvent();

        /// <summary>
        /// Event that invokes when the eyes are closed
        /// </summary>
        public EyeCloseEvent OnEyeClose => onEyeClose;

        [SerializeField]
        private EyeOpenEvent onEyeOpen = new EyeOpenEvent();

        /// <summary>
        /// Event that invokes when the eyes are opened
        /// </summary>
        public EyeOpenEvent OnEyeOpen => onEyeOpen;

        [SerializeField]
        private BlinkEvent onBlink = new BlinkEvent();

        /// <summary>
        /// Event that invokes when the system considers the user's facial expression to have
        /// performed a blink.
        /// </summary>
        public BlinkEvent OnBlink => onBlink;

        /// <summary>
        /// Blink event arguments passed through the event when the user blinks
        /// </summary>
        [System.Serializable]
        public class BlinkEventArgs : System.EventArgs
        {
            /// <summary>
            /// The time in seconds the eyes were closed for
            /// </summary>
            public float EyesClosedTime { get; set; }

            public BlinkEventArgs(float eyesClosedTime)
            {
                EyesClosedTime = eyesClosedTime;
            }
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
        public class BlinkEvent : UnityEvent<BlinkEventArgs> { }


        private bool hasStartedBlink;
        private float blinkTime;


        private void Awake()
        {
            Debug.Assert(faceExpressions, "missing reference to OVRFaceExpression class");
        }

        private void Update()
        {
            if (!useFixedUpdate)
                ProcessBlinking();
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate)
                ProcessBlinking();
        }


        private void ProcessBlinking()
        {
            if (!faceExpressions) return;

            if (!hasStartedBlink)
            {
                if (EvaluateExpression(faceExpressions, blinkParameters))
                {
                    HandleBlinkStarted();
                }
            }
            else
            {
                blinkTime += useFixedUpdate ? Time.fixedDeltaTime : Time.deltaTime;
                if (!EvaluateExpression(faceExpressions, blinkParameters))
                {
                    HandleBlinkStopped();
                }
            }
        }

        private void HandleBlinkStarted()
        {
            hasStartedBlink = true;
            //reset timer
            blinkTime = 0f;
            //Invoke the eyes closed event
            OnEyeClose.Invoke();
        }

        private void HandleBlinkStopped()
        {
            hasStartedBlink = false;
            //Invoke blinked event if the blink time is less than the max closed time
            if (blinkTime <= maxEyesClosedTime)
                OnBlink.Invoke(new BlinkEventArgs(eyesClosedTime: blinkTime));
            //Invoke the eyes open event
            OnEyeOpen.Invoke();
        }

        private bool EvaluateExpression(OVRFaceExpressions faceExpressions, params BlinkParameter[] blinkParameters)
        {
            bool result = true;
            foreach (BlinkParameter param in blinkParameters)
            {
                float weight;
                if (faceExpressions.TryGetFaceExpressionWeight(param.FaceExpression, out weight))
                {
                    if (weight < param.Threshold)
                        result = false;
                }
                else
                    result = false;
            }
            return result;
        }
    }
}