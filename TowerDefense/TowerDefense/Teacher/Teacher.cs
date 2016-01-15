using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class Teacher : Sprite
    {
        /*The cost, damage, radius of the teacher*/
        protected int cost; 
        protected int damage;
        protected float radius; 
        
        protected Student target;

        /*Different paper image to print*/
        protected bool isPaperDead;
        protected float paperTimer; 
        protected Texture2D paperTexture, paperBreakTexture;
        
        /*A list to store the papaer*/
        protected List<Paper> paperList = new List<Paper>();

        public int Cost
        {
            get { return cost; }
        }
        public int Damage
        {
            get { return damage; }
        }
        public float Radius
        {
            get { return radius; }
        }
        public Student Target
        {
            get { return target; }
        }
        public virtual bool HasTarget
        {
            get { return target != null; }
        }
        public Teacher(Texture2D texture, Texture2D paperTexture, Vector2 position, Texture2D paperBreakTexture, int totalFrame = 1)
            : base(texture, position, totalFrame)
        {
            isPaperDead = false;
            this.paperTexture = paperTexture;
            this.paperBreakTexture = paperBreakTexture;
        }

        protected void FaceTarget()
        {
            /*To get the direction*/
            Vector2 direction = center - target.Center;
            direction.Normalize();
            rotation = (float)Math.Atan2(-direction.X, direction.Y);
        }

        public bool IsInRange(Vector2 position)
        {
            return Vector2.Distance(center, position) <= radius;
        }

        public virtual void GetClosestStudent(List<Student> students)
        {
            /*Get the closest student and set it to be the target*/
            target = null;
            float smallestRange = radius;

            foreach (Student student in students)
            {
                if (Vector2.Distance(center, student.Center) < smallestRange)
                {
                    smallestRange = Vector2.Distance(center, student.Center);
                    target = student;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            paperTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            /*Set the refresh speed of the paper images*/ 
            if (target != null )
            {
                FaceTarget();

                if (!IsInRange(target.Center) || target.IsDead)
                {
                    target = null;
                    paperTimer = 0;
                }
                RefreshSpeed = 4;
            }
            else
            {
                RefreshSpeed = 0;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            /*Draw the paper images*/
            base.Draw(spriteBatch, Color.White);
            foreach (Paper paper in paperList)
                paper.Draw(spriteBatch, Color.White);
            
        }

    }
}
