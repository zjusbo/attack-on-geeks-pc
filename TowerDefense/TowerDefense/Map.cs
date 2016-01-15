using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class Map
    {
        private Texture2D tileTexture;
        private Queue<Vector2> waypoints = new Queue<Vector2>();

        int[,] map = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,1,1,1,1,1,1,0,0},
            {0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,1,1,1,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        };

        public Queue<Vector2> Waypoints
        {
            get { return waypoints; }
        }

        public int Width
        {
            get { return map.GetLength(1); }
        }
        public int Height
        {
            get { return map.GetLength(0); }
        }

        public Map()
        {
            waypoints.Enqueue(new Vector2(0, 8) * 60);
            /**
             waypoints.Enqueue(new Vector2(1, 8) * 60);
            waypoints.Enqueue(new Vector2(2, 8) * 60);
            waypoints.Enqueue(new Vector2(3, 8) * 60);
            waypoints.Enqueue(new Vector2(4, 8) * 60);
            waypoints.Enqueue(new Vector2(5, 8) * 60);
            waypoints.Enqueue(new Vector2(6, 8) * 60);
            waypoints.Enqueue(new Vector2(7, 8) * 60);
            waypoints.Enqueue(new Vector2(8, 8) * 60);
             * */

            waypoints.Enqueue(new Vector2(9, 8) * 60);

            /**
            waypoints.Enqueue(new Vector2(9, 7) * 60);
            waypoints.Enqueue(new Vector2(9, 6) * 60);
            **/
            waypoints.Enqueue(new Vector2(9, 5) * 60);
            /*
            waypoints.Enqueue(new Vector2(9, 4) * 60);
            waypoints.Enqueue(new Vector2(8, 4) * 60);
            waypoints.Enqueue(new Vector2(7, 4) * 60);
            */
            waypoints.Enqueue(new Vector2(6, 5) * 60);
            /*
            waypoints.Enqueue(new Vector2(6, 3) * 60);
            waypoints.Enqueue(new Vector2(6, 2) * 60);
             */
            waypoints.Enqueue(new Vector2(6, 1) * 60);
            /*
            waypoints.Enqueue(new Vector2(7, 1) * 60);
            waypoints.Enqueue(new Vector2(8, 1) * 60);
            waypoints.Enqueue(new Vector2(9, 1) * 60);
            waypoints.Enqueue(new Vector2(10, 1) * 60);
            waypoints.Enqueue(new Vector2(11, 1) * 60);
            */
            waypoints.Enqueue(new Vector2(12, 1) * 60);
        }

        public int GetIndex(int posX, int posY)
        {
            // It needed to be Width - 1 and Height - 1.
            if (posX < 0 || posX > Width - 1 || posY < 0 || posY > Height - 1)
                return 0;

            return map[posY, posX];
        }

        public void AddTexture(Texture2D texture)
        {
            tileTexture = texture;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tileTexture, new Rectangle(0, 0, 900, 600), Color.White);
        }
    }
}
