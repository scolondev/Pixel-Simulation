using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsExtensions.UI
{
    public class FPSCounter : MonoBehaviour
    {
        public Text fpsDisplay;
        private int frameCount = 0; //Current frame.
        private float nextUpdate = 0.0f; //When the next time we are updating is.
        private int fps = 0; //Current FPS.
        private int updateRate = 4;  // 4 updates per sec.

        public void Update()
        {
            frameCount++;
            if (Time.time > nextUpdate)
            {
                nextUpdate += 1.0f / updateRate;
                fps = frameCount * updateRate;
                frameCount = 0;
                fpsDisplay.text = string.Format("{0} FPS", fps);
            }
        }
    }
}
