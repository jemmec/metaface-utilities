using System;
using System.Collections.Generic;
using UnityEngine;

namespace Metaface.Debug
{
    public class FaceExpressionDemo : MonoBehaviour
    {
        [SerializeField]
        private OVRFaceExpressions faceExpressions;

        [SerializeField]
        private FaceWeightComponent faceWeightPrefab;

        [SerializeField]
        private Transform faceWeightLayout;

        private List<FaceWeightComponent> components = new List<FaceWeightComponent>();

        void Start()
        {
            //create face weights
            BuildDemo();
        }

        private void BuildDemo()
        {
            foreach (OVRFaceExpressions.FaceExpression e in Enum.GetValues(typeof(OVRFaceExpressions.FaceExpression)))
            {
                FaceWeightComponent comp = Instantiate(faceWeightPrefab);
                comp.transform.SetParent(faceWeightLayout, false);
                comp.FaceExpression = e;
                comp.WeightName = e.ToString();
                components.Add(comp);
            }

            //Hide prefab
            faceWeightPrefab.gameObject.SetActive(false);
        }

        void Update()
        {
            //Do update
            foreach (FaceWeightComponent comp in components)
            {
                float weight;
                if (faceExpressions.TryGetFaceExpressionWeight(comp.FaceExpression, out weight))
                {
                    if (!comp.gameObject.activeInHierarchy)
                        comp.gameObject.SetActive(true);
                    comp.WeightValue = weight;
                }
                else
                {
                    if (comp.gameObject.activeInHierarchy)
                        comp.gameObject.SetActive(false);
                }
            }
        }

    }
}