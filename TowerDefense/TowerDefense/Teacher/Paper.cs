using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class Paper : Sprite
    {
        /*The damage, age, speed of the paper*/
        private int damage;
        private int age;
        private Rectangle textureBound;
        private int speed;
        private Texture2D paperBreak;
        private PaperState state;
        public int Damage
        {
            get { return damage; }
        }

        public bool IsDead()
        {
            return age > 100;
        }
        public PaperState Paperstate
        {
            get { return state; }
        }
        public Paper(Texture2D texture,Texture2D breakTexture, Vector2 position, float rotation, int speed, int damage)
            : base(texture, position)
        {
            this.state = PaperState.FLYING;
            this.rotation = rotation;
            this.damage = damage;
            this.textureBound = new Rectangle(0, 0, texture.Width / 4, texture.Height);
            this.speed = speed;
            this.paperBreak = breakTexture;
            velocity = Vector2.Transform(new Vector2(0, -speed),
                Matrix.CreateRotationZ(rotation));
        }

        public Paper(Texture2D texture, Texture2D breakTexture, Vector2 position, Vector2 velocity, int speed, int damage)
            : base(texture, position)
        {
            this.rotation = rotation;
            this.damage = damage;
            this.paperBreak = breakTexture;
            this.speed = speed;
            this.state = PaperState.FLYING;
            this.velocity = velocity * speed;
        }

        public void Kill(PaperState state)
        {
            /*TO indicate different stage of the paper*/
            if (this.state != PaperState.FLYING)
            {
                return;
            }
            this.state = state;
            if (state == PaperState.MISS)
            {
                this.age = 200;
            }
            else if(state == PaperState.HIT)
            {
                //Replace texture to paper break
                texture = paperBreak;
                totalFrame = 5;
                frame = 0;
                height = texture.Height;
                widthPerFrame = texture.Width / totalFrame;
                count = 0;
                RefreshSpeed = 5;
                velocity = Vector2.Zero;
            }
        }

        public void SetRotation(float value)
        {
            /*Set the rotation and velocity of the paper*/
            rotation = value;

            velocity = Vector2.Transform(new Vector2(0, -speed), 
                Matrix.CreateRotationZ(rotation));
        }

        public override void Update(GameTime gameTime)
        {
            /*Set the different postion of the paper according to the velocity*/
            if (state == PaperState.HIT)
            {
                if (frame == 4)
                {
                    this.age = 200;//Kill Paper
                }
            }
            else
            {
                age++;
                position += velocity;
            }
            base.Update(gameTime);
        }
    }
}
