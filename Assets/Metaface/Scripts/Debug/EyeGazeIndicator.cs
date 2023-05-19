using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metaface.Debug
{
    public class EyeGazeIndicator : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

    [SerializeField]
        private float revertDelay = 5f;

        private Color defaultColor;

        private Coroutine revertRoutine;

        private void Start()
        {
            //Save the default color
            defaultColor = meshRenderer.material.color;
        }

        public void SetColor(Color color)
        {
            meshRenderer.material.color = color;
            if(revertRoutine!=null)
                StopCoroutine(revertRoutine);
            revertRoutine = StartCoroutine(RevertRoutine());
        }

        private IEnumerator RevertRoutine()
        {
            yield return new WaitForSeconds(revertDelay);
            meshRenderer.material.color = defaultColor;
        }

    }
}