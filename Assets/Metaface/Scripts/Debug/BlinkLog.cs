using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metaface.Utilities;

namespace Metaface.Debug
{

    public class BlinkLog : MonoBehaviour
    {
        [SerializeField]
        private BlinkHelper blinkHelper;

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private RectTransform openCloseLayout;

        [SerializeField]
        private RectTransform blinkLayout;

        [SerializeField]
        [Tooltip("How long the event will display before self destory")]
        private float eventTtl = 8;

        void Start()
        {
            //Hide prefab
            prefab.SetActive(false);
        }

        private void OnEnable()
        {
            blinkHelper.OnBlink.AddListener(HandleBlink);
            blinkHelper.OnEyeClose.AddListener(HandleEyeClosed);
            blinkHelper.OnEyeOpen.AddListener(HandleEyeOpen);
        }

        private void OnDisable()
        {
            blinkHelper.OnBlink.RemoveListener(HandleBlink);
            blinkHelper.OnEyeClose.RemoveListener(HandleEyeClosed);
            blinkHelper.OnEyeOpen.RemoveListener(HandleEyeOpen);
        }

        private void HandleEyeOpen(float eyesClosedTime)
        {
            CreateEvent(openCloseLayout, $"eyes <color=#ff6060>open</color> event : <color=#ffff60>{eyesClosedTime * 1000}ms</color>");
        }

        private void HandleEyeClosed()
        {
            CreateEvent(openCloseLayout, "eyes <color=#60ff60>closed</color> event");
        }

        private void HandleBlink(BlinkHelper.BlinkEventArgs args)
        {
            CreateEvent(blinkLayout, $"eyes <color=#6060ff>blinked</color> event : <color=#ffff60>{args.EyesClosedTime * 1000}ms</color>");
        }

        private void CreateEvent(RectTransform layout, string text)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(layout, false);
            obj.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = text;
            obj.SetActive(true);
            Destroy(obj, eventTtl);
        }

    }

}