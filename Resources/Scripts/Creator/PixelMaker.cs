using PixelSimulation.Pixel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSimulation.Creator
{
    public class PixelMaker : MonoBehaviour
    {
        public GameObject pixelPrefab;
        public void Update()
        {
            if (Input.GetMouseButton(0)) CreatePixel();
        }

        public void CreatePixel()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;
            Vector2Int position = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            if (PixelPhysicsManager.instance.IsFree(position))
            {
                Instantiate(pixelPrefab, new Vector2(position.x,position.y), Quaternion.identity);
            }
        }
    }
}
