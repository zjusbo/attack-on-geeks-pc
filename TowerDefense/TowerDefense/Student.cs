using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class Student : Sprite
    {
        private Queue<Vector2> waypoints = new Queue<Vector2>();



        private float speedModifier;

        private float modifierDuration;
        private float modiferCurrentTime;

        protected float startHealth;
        protected float currentHealth;

        protected bool alive = true;

        protected float speed = 0.5f;
        protected int bountyGiven;

        private int studentType;

        public int StudentType
        {
            get { return studentType; }
            set { studentType = value; }
        }

        /// <summary>
        /// Alters the speed of the Student.
        /// </summary>
        public float SpeedModifier
        {
            get { return speedModifier; }
            set { speedModifier = value; }
        }
        /// <summary>
        /// Defines how long the speed modification will last.
        /// </summary>
        public float ModifierDuration
        {
            get { return modifierDuration; }
            set 
            { 
                modifierDuration = value;
                modiferCurrentTime = 0;
            }
        }

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public bool IsDead
        {
            get { return !alive; }
        }
        public int BountyGiven
        {
            get { return bountyGiven; }
        }

        public float DistanceToDestination
        {
            get { return Vector2.Distance(position, waypoints.Peek()); }
        }

        public Student(Texture2D texture, Vector2 position, float health, int bountyGiven, float speed, int studentType, int totalframe = 7, int refreshSpeed = 4)
            : base(texture, position, totalframe, refreshSpeed)
        {
            this.startHealth = health;
            this.currentHealth = startHealth;
            this.bountyGiven = bountyGiven;
            this.speed = speed;
            this.studentType = studentType;
        }


        public void SetWaypoints(Queue<Vector2> waypoints)
        {
            foreach (Vector2 waypoint in waypoints)
                this.waypoints.Enqueue(waypoint);

            this.position = this.waypoints.Dequeue();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (waypoints.Count > 0)
            {
                if (DistanceToDestination < 1f)
                {
                    position = waypoints.Peek();
                    waypoints.Dequeue();
                }

                else
                {
                    Vector2 direction = waypoints.Peek() - position;
                    direction.Normalize();

                    // Store the original speed.
                    float temporarySpeed = speed;

                    // If the modifier has finished,
                    if (modiferCurrentTime > modifierDuration)
                    {
                        // reset the modifier.
                        speedModifier = 0;
                        modiferCurrentTime = 0;
                    }

                    if (speedModifier != 0 && modiferCurrentTime <= modifierDuration)
                    {
                        // Modify the speed of the Student.
                        temporarySpeed *= speedModifier;
                        // Update the modifier timer.
                        modiferCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    velocity = Vector2.Multiply(direction, temporarySpeed);

                    position += velocity;
                }
            }

            else
                alive = false;

            if (currentHealth <= 0)
                alive = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color color, Rectangle sourceRectangle, SpriteEffects effect = SpriteEffects.None)
        {
            spriteBatch.Draw(texture, center, sourceRectangle, color, rotation, origin, 1.0f, effect, 0);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                float healthPercentage = (float)currentHealth / (float)startHealth;
                Color color = new Color(new Vector3(healthPercentage,
                    healthPercentage, healthPercentage));
                SpriteEffects effect = velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                base.Draw(spriteBatch, color, effect);
            }
        }
    }
}
