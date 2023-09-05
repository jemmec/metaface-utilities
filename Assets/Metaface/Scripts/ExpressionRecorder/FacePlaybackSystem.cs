using System.Collections;
using System.Collections.Generic;
using Oculus.Movement;
using Oculus.Movement.Attributes;
using Oculus.Movement.Tracking;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Applies Prerecorded Face tracking data into the Avatar's face blendshapes
/// Base on oculus' FaceTrackingSystem
/// </summary>
public class FacePlaybackSystem : MonoBehaviour
{

    /// <summary>
    /// Blendshape mapping component.
    /// </summary>
    [SerializeField]
    [Tooltip(FaceTrackingSystemTooltips.BlendshapeMapping)]
    protected BlendshapeMapping _blendShapeMapping;

    /// <summary>
    /// Optional corrective shapes driver component.
    /// </summary>
    [SerializeField]
    [Optional]
    [Tooltip(FaceTrackingSystemTooltips.CorrectiveShapesDriver)]
    protected CorrectiveShapesDriver _correctiveShapesDriver;

    /// <summary>
    /// Optional blendshape modifier component.
    /// </summary>
    [SerializeField]
    [Optional]
    [Tooltip(FaceTrackingSystemTooltips.BlendshapeModifier)]
    protected BlendshapeModifier _blendshapeModifier;

    /// <summary>
    /// If true, the correctives driver will apply correctives.
    /// </summary>
    public bool CorrectivesEnabled { get; set; }

    /// <summary>
    /// Last updated expression weights.
    /// </summary>
    public float[] ExpressionWeights { get; private set; }

    private void Awake()
    {
        Assert.IsNotNull(_blendShapeMapping);
        ExpressionWeights = new float[(int)OVRFaceExpressions.FaceExpression.Max];
        CorrectivesEnabled = true;
    }

    /// <summary>
    /// Updates the face given an array of weights,
    /// Ensure the order of these weights match that of the FaceExpressions and
    /// the length also matches.
    /// </summary>
    /// <param name="weights"></param>
    public void ApplyFaceWeight(
        float[] weights
    )
    {
        if (ExpressionWeights.Length != weights.Length)
            throw new System.Exception("The passed weights length is not equal");

        UpdateExpressionWeights(weights);
        UpdateAllMeshesUsingFaceTracking();

        if (CorrectivesEnabled && _correctiveShapesDriver != null)
        {
            _correctiveShapesDriver.ApplyCorrectives();
        }
    }

    private void UpdateExpressionWeights(float[] weights)
    {
        for (var expressionIndex = 0;
                expressionIndex < (int)OVRFaceExpressions.FaceExpression.Max;
                ++expressionIndex)
        {
            ExpressionWeights[expressionIndex] = weights[expressionIndex];
        }
    }

    private void UpdateAllMeshesUsingFaceTracking()
    {
        foreach (var m in _blendShapeMapping.Meshes)
        {
            UpdateSkinnedMeshUsingFaceTracking(m.Mesh, m.Blendshapes);
        }
    }

    private void UpdateSkinnedMeshUsingFaceTracking(
                SkinnedMeshRenderer renderer,
                List<OVRFaceExpressions.FaceExpression> mapping)
    {
        if (renderer == null)
        {
            return;
        }

        if (renderer.sharedMesh != null)
        {
            int numBlendshapes = Mathf.Min(mapping.Count,
                renderer.sharedMesh.blendShapeCount);
            for (int blendShapeIndex = 0; blendShapeIndex < numBlendshapes; ++blendShapeIndex)
            {
                var blendShapeToFaceExpression = mapping[blendShapeIndex];
                if (blendShapeToFaceExpression == OVRFaceExpressions.FaceExpression.Max)
                {
                    continue;
                }
                float currentWeight = ExpressionWeights[(int)blendShapeToFaceExpression];

                // Recover true eyes closed values
                if (blendShapeToFaceExpression == OVRFaceExpressions.FaceExpression.EyesClosedL)
                {
                    currentWeight += ExpressionWeights[(int)OVRFaceExpressions.FaceExpression.EyesLookDownL];
                }
                else if (blendShapeToFaceExpression == OVRFaceExpressions.FaceExpression.EyesClosedR)
                {
                    currentWeight += ExpressionWeights[(int)OVRFaceExpressions.FaceExpression.EyesLookDownR];
                }

                if (_blendshapeModifier != null)
                {
                    currentWeight = _blendshapeModifier.GetModifiedWeight(blendShapeToFaceExpression, currentWeight);

                }
                renderer.SetBlendShapeWeight(blendShapeIndex, currentWeight * 100.0f);
            }
        }
        else
        {
            Debug.LogError("Renderer.sharedMesh is null.");
        }
    }
}
