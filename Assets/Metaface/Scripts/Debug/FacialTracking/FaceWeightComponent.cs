using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Metaface.Debug
{
    public class FaceWeightComponent : MonoBehaviour
    {
        [SerializeField]
        private Slider weightSlider;

        [SerializeField]
        private TextMeshProUGUI nameText, valueText;

        public string WeightName
        {
            set => nameText.text = value;
        }

        public float WeightValue
        {
            set
            {
                valueText.text = value.ToString("0.00");
                weightSlider.SetValueWithoutNotify(value);
            }
        }

        public OVRFaceExpressions.FaceExpression FaceExpression { get; set; }
    }
}