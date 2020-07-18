using PixelSimulation.Pixel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSimulation.Creator
{
    public class PixelMaker : MonoBehaviour
    {
        public GameObject pixelPrefab;
        public int brushSize = 0;
        public void Update()
        {
            if (Input.GetMouseButton(0)) CreatePixel();
            if (Input.GetMouseButton(1)) DestroyPixel();
        }

        /// <summary>
        /// Creates pixel
        /// </summary>
        public void CreatePixel()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;
            Vector2Int position = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            if (PixelPhysicsManager.instance.IsFree(position))
            {
                for(int x = 0; x < brushSize; x++)
                    for(int y = 0; y < brushSize; y++)
                        Instantiate(pixelPrefab, new Vector2(position.x + x, position.y + y), Quaternion.identity);
            }
        }

        /// <summary>
        /// Destroys pixel
        /// </summary>
        public void DestroyPixel()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;
            Vector2Int position = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            if (PixelPhysicsManager.instance.pixels.ContainsKey(position))
            {
                PixelPhysicsManager.instance.DestroyPixel(position);
            }
        }
    }
}
