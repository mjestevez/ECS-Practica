using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utils;

namespace Tools
{
    public class FPSCounter : Singleton<FPSCounter>
    {
        public int FPS { get; private set; }

        private void Update()
        {
            CalculateFramesPerSecond();
        }

        private void CalculateFramesPerSecond()
        {
            FPS = (int)(1f / Time.unscaledDeltaTime);
            
        }
    }
}