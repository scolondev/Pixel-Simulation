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

        public void PhysicsUpdate()
        {
            Move();
        }

        public void Move()
        {
            //Move down
            List<Vector2Int> positions = GetOpenLocations();
            if(positions.Count > 0)
            {
                int rand = Random.Range(0, positions.Count);
                pixelPhysics.FreePixel(_position);
                SetPosition(positions[rand]);
                pixelPhysics.SetPixel(_position, this);     
            } else
            {
                //If we couldn't find any open positions, then we probably can't move-
                pixelPhysics.RemoveActivePixel(this);
            }
        }

        public List<Vector2Int> GetOpenLocations()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            Vector2Int position = GetPosition();
            Vector2Int down = new Vector2Int(position.x, position.y - 1);
            Vector2Int downleft = new Vector2Int(position.x -1, position.y -1);
            Vector2Int downright = new Vector2Int(position.x + 1, position.y -1);

            if (pixelPhysics.IsFree(down)) positions.Add(down);
            if (pixelPhysics.IsFree(downleft)) positions.Add(downleft);
            if (pixelPhysics.IsFree(downright)) positions.Add(downright);

            return positions;
        }
        public Vector2Int GetPosition()
        {
            return _position;
        }

        private void SetPosition(Vector2Int position)
        {
            _position = position;
            transform.position = new Vector2(_position.x,_position.y);
        }
    }
}
