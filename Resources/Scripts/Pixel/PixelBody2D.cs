using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelSimulation.Pixel
{
    public class PixelBody2D : MonoBehaviour
    {
        public bool isStatic = false;
        private PixelPhysicsManager pixelPhysics;
        private Vector2Int _position;
  
        // Start is called before the first frame update
        private void Start()
        {
            pixelPhysics = PixelPhysicsManager.instance;
            if(!isStatic) pixelPhysics.AddActivePixel(this);

            SetPosition(new Vector2Int((int)transform.position.x, (int)transform.position.y));

            pixelPhysics.SetPixel(GetPosition(), this);
        }

        /// <summary>
        /// What to do when physics updates.
        /// </summary>
        public void PhysicsUpdate()
        {
            Move();
        }

        /// <summary>
        /// Moves the pixel
        /// </summary>
        public void Move()
        {   
            //Move down
            List<Vector2Int> positions = GetOpenLocations();
            if (positions.Count > 0)
            {
                int rand = Random.Range(0, positions.Count);
                pixelPhysics.FreePixel(_position);
                SetPosition(positions[rand]);
                pixelPhysics.SetPixel(_position, this);
            }
            else if (IsGroundInactive()) pixelPhysics.RemoveActivePixel(this);
        }

        /// <summary>
        /// Returns a list of open locations near the pixel relative to gravity.
        /// </summary>
        /// <returns></returns>
        public List<Vector2Int> GetOpenLocations()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            Vector2Int position = GetPosition();
            Vector2Int down = new Vector2Int(position.x, position.y + (1 * pixelPhysics.gravity));
            Vector2Int downleft = new Vector2Int(position.x - 1, position.y + (1 * pixelPhysics.gravity));
            Vector2Int downright = new Vector2Int(position.x + 1, position.y + (1 * pixelPhysics.gravity));
            
            if (pixelPhysics.IsFree(down)) positions.Add(down);
            if (pixelPhysics.IsFree(downleft)) positions.Add(downleft);
            if (pixelPhysics.IsFree(downright)) positions.Add(downright);

            return positions;
        }
   
        /// <summary>
        /// Destroys this pixel
        /// </summary>
        public void Destroy()
        {
            pixelPhysics.UpdateNearbyPixels(this);
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Returns the VectorInt Postion
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// Sets the VectorInt Position
        /// </summary>
        /// <param name="position"></param>
        private void SetPosition(Vector2Int position)
        {
            _position = position;
            transform.position = new Vector2(_position.x,_position.y);
        }


        /// <summary>
        /// Returns if the ground isn't active.
        /// </summary>
        /// <returns></returns>
        private bool IsGroundInactive()
        {
            Vector2Int position = new Vector2Int(_position.x, _position.y + (1 * pixelPhysics.gravity));
            if (pixelPhysics.pixels.ContainsKey(position))
            {
                //If it is static, return true
                //If it is not static, return whether it is considered inactive.
                return pixelPhysics.pixels[position].isStatic ? true : pixelPhysics.inactivePixels.Contains(pixelPhysics.pixels[position]);
            } else
            {
                return false;
            }
        }
    }
}
