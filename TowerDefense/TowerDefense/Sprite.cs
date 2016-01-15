using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class Sprite
    {
        protected Texture2D texture;

        protected Vector2 position;
        protected Vector2 velocity;

        protected Vector2 center;
        protected Vector2 origin;

        protected float rotation;

        /*Sbo modified*/
        protected int count;
        protected int totalFrame, widthPerFrame;
        protected int height;
        protected int frame;
        private int refreshSpeed;//from 1(slow) to 10(fast) . set 0 if only show the first frame.
        /**/

        public int RefreshSpeed
        {
            get { return refreshSpeed; }
            set { refreshSpeed = value; }
        }
        public Vector2 Position
        {
            get { return position; }
        }
        public Vector2 Center
        {
            get { return center; }
        }

        public Sprite(Texture2D tex, Vector2 pos, int totalFrame = 1, int refreshSpeed = 0)
        {
            texture = tex;

            position = pos;
            velocity = Vector2.Zero;

            this.totalFrame = totalFrame;
            this.height = tex.Height;
            this.widthPerFrame = tex.Width / totalFrame;
            this.refreshSpeed = refreshSpeed;
            this.frame = 0;
            this.count = 0;
            center = new Vector2(position.X + widthPerFrame / 2, position.Y + height / 2);
            origin = new Vector2(widthPerFrame / 2, height / 2);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.center = new Vector2(position.X + widthPerFrame / 2,
                position.Y + height / 2);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, center, null, Color.White,
                rotation, origin, 1.0f, SpriteEffects.None, 0);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color color, SpriteEffects effect = SpriteEffects.None)
        {
            count++;
            if (refreshSpeed != 0 && totalFrame != 1)
            {
                int n = count % 10;
                if (n == 1)
                {
                    frame = ++frame >= totalFrame ? 0 : frame;
                    count = refreshSpeed;
                }
                spriteBatch.Draw(texture, center, new Rectangle(widthPerFrame * frame, 0, widthPerFrame, height), color, 0, origin, 1.0f, effect, 0);
            }
            else
            {
                spriteBatch.Draw(texture, center, new Rectangle(0, 0, widthPerFrame, height), color, 0, origin, 1.0f, effect, 0);
            }
        }
        
    }
}
