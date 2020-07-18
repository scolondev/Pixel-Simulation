using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSimulation.Pixel
{
    public class PixelPhysicsManager : MonoBehaviour
    {
        public static PixelPhysicsManager instance;
        public void Awake()
        {
            instance = this;
        }

        public Dictionary<Vector2Int, PixelBody2D> pixels = new Dictionary<Vector2Int, PixelBody2D>();
        public GameObject activePixelsParent;
        public GameObject inactivePixelsParent;
        private List<PixelBody2D> activePixels = new List<PixelBody2D>();
        private List<PixelBody2D> inactivePixels = new List<PixelBody2D>();
        public float gravity = -9.8f;
        public float refreshRate = 10f;

        public void Start()
        {
            InvokeRepeating("PixelUpdate", 0, 1.0f / refreshRate);
        }

        public void PixelUpdate()
        {
            float time = Time.realtimeSinceStartup;
            List<PixelBody2D> update = new List<PixelBody2D>(activePixels);
            foreach(var key in update)
            {
                key.PhysicsUpdate();
            }
            Debug.Log("Time Elapsed " + ((Time.realtimeSinceStartup - time) * 1000f));
        }

        public void FreePixel(Vector2Int position)
        {
            pixels.Remove(position);
        }

        public void SetPixel(Vector2Int position, PixelBody2D body)
        {
            if (pixels.ContainsKey(position)) pixels[position] = body;
            else pixels.Add(position, body);
        }

        public void AddActivePixel(PixelBody2D body)
        {
            body.transform.SetParent(activePixelsParent.transform);
            if (!activePixels.Contains(body)) activePixels.Add(body);
            if (inactivePixels.Contains(body)) inactivePixels.Remove(body);
        }

        public void RemoveActivePixel(PixelBody2D body)
        {
            body.transform.SetParent(inactivePixelsParent.transform);
            if (activePixels.Contains(body)) activePixels.Remove(body);
            if (!inactivePixels.Contains(body)) inactivePixels.Add(body);
        }
        public bool IsFree(Vector2Int position)
        {
            if (!pixels.ContainsKey(position)) return true;
            return !pixels[position];
        }
    }
}
