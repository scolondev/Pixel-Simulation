using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public readonly List<PixelBody2D> activePixels = new List<PixelBody2D>();
        public readonly List<PixelBody2D> inactivePixels = new List<PixelBody2D>();

        [Range(-1,1)]
        public int gravity = -10;
        public float refreshRate = 10f;

        public void Start()
        {
            InvokeRepeating("PixelUpdate", 0, 1.0f / refreshRate);
        }

        /// <summary>
        /// The physics update function for pixels
        /// </summary>
        public void PixelUpdate()
        {
           // float time = Time.realtimeSinceStartup;
            List<PixelBody2D> update = new List<PixelBody2D>(activePixels);

            foreach(var key in update)
            {
                key.PhysicsUpdate();
              //  UpdateNearbyPixels(key);
            }
           // Debug.Log("Time Elapsed " + ((Time.realtimeSinceStartup - time) * 1000f));
        }

        /// <summary>
        /// Updates nearby pixels so they are no longer inactive
        /// </summary>
        /// <param name="body"></param>
        public void UpdateNearbyPixels(PixelBody2D body)
        {
            //  if (inactivePixels.Contains(body)) return;
            //Get Surrounding Positions
            Vector2Int position = body.GetPosition();
            List<Vector2Int> surroundingPositions = new List<Vector2Int>();
            surroundingPositions.Add(new Vector2Int(position.x, position.y + 1)); //Up
            surroundingPositions.Add(new Vector2Int(position.x, position.y - 1)); //Down
            surroundingPositions.Add(new Vector2Int(position.x - 1, position.y)); //Left
            surroundingPositions.Add(new Vector2Int(position.x + 1, position.y)); //Right
           /* surroundingPositions.Add(new Vector2Int(position.x - 1, position.y + 1)); //Up Left
            surroundingPositions.Add(new Vector2Int(position.x - 1, position.y - 1)); //Down Left
            surroundingPositions.Add(new Vector2Int(position.x + 1, position.y + 1)); //Up Right
            surroundingPositions.Add(new Vector2Int(position.x + 1, position.y - 1)); //Down Right */

            //For each surrounding position, if we have that key, make sure that that key is active.
            //This might cause problems
            foreach (var pos in surroundingPositions)
            { 
                if (pixels.ContainsKey(pos) && inactivePixels.Contains(pixels[pos]))
                {
                    AddActivePixel(pixels[pos]);
                    UpdateNearbyPixels(pixels[pos]);
                }
            }
        }

        /// <summary>
        /// Frees a position
        /// </summary>
        /// <param name="position"></param>
        public void FreePixel(Vector2Int position)
        {
            pixels.Remove(position);
        }

        /// <summary>
        /// Destroys a pixel
        /// </summary>
        /// <param name="position"></param>
        public void DestroyPixel(Vector2Int position)
        {
            if (pixels.ContainsKey(position))
            {
                PixelBody2D body = pixels[position];
                pixels.Remove(position);
                activePixels.Remove(body);
                inactivePixels.Remove(body);
                body.Destroy();
            }
        }


        /// <summary>
        /// Sets the state of a particular position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="body"></param>
        public void SetPixel(Vector2Int position, PixelBody2D body)
        {
            if (pixels.ContainsKey(position)) pixels[position] = body;
            else pixels.Add(position, body);
        }

        /// <summary>
        /// Adds an active pixel.
        /// </summary>
        /// <param name="body"></param>
        public void AddActivePixel(PixelBody2D body)
        {
            body.transform.SetParent(activePixelsParent.transform);
            if (!activePixels.Contains(body)) activePixels.Add(body);
            if (inactivePixels.Contains(body)) inactivePixels.Remove(body);
        }

        /// <summary>
        /// Removes an active pixel.
        /// </summary>
        /// <param name="body"></param>
        public void RemoveActivePixel(PixelBody2D body)
        {
            body.transform.SetParent(inactivePixelsParent.transform);
            if (activePixels.Contains(body)) activePixels.Remove(body);
            if (!inactivePixels.Contains(body)) inactivePixels.Add(body);
        }

        /// <summary>
        /// Returns if a position is free to move to
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsFree(Vector2Int position)
        {
            if (!pixels.ContainsKey(position)) return true;
            return !pixels[position];
        }
    }
}
