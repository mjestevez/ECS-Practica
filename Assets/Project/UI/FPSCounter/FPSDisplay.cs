using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tools
{
    public class FPSDisplay : MonoBehaviour
    {
        public TextMeshProUGUI FpsText;

        private void Start()
        {
            StartCoroutine(UpdateFpsCoroutine());
        }

        IEnumerator UpdateFpsCoroutine()
        {
            while (true)
            {
                UpdateUI();
                yield return new WaitForSecondsRealtime(0.25f);
            }
        }

        private void UpdateUI()
        {
            float fps = FPSCounter.Instance.FPS;
            FpsText.text = fps.ToString() + " FPS";
            ChangeColor(fps);
        }

        private void ChangeColor(float fps)
        {
            if (fps >= 60)
            {
                FpsText.color = Color.green;
            }
            else if (fps >= 30)
            {
                FpsText.color = Color.yellow;
            }
            else
            {
                FpsText.color = Color.red;
            }
        }
    }
}