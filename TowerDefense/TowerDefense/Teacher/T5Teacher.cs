using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class T5Teacher : Teacher//slow teacher
    {
        // Defines how fast an student will move when hit.
        private float speedModifier;
        // Defines how long this effect will last.
        private float modifierDuration;

        public T5Teacher(Texture2D texture, Texture2D paperTexture, Vector2 position, Texture2D paperBreakTexture, int totalFrame = 5)
            : base(texture, paperTexture, position, paperBreakTexture, totalFrame)
        {
            this.damage = 5; // Set the damage
            this.cost = 25;   // Set the initial cost

            this.radius = 240; // Set the radius

            this.speedModifier = 0.6f;
            this.modifierDuration = 2.0f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (paperTimer >= 2.0f && target != null)
            {
                Paper paper = new Paper(paperTexture, paperBreakTexture, Vector2.Subtract(center,
                    new Vector2(paperTexture.Width / 2)), rotation, 5, damage);

                paperList.Add(paper);
                paperTimer = 0;
            }

            for (int i = 0; i < paperList.Count; i++)
            {
                Paper paper = paperList[i];

                paper.SetRotation(rotation);
                paper.Update(gameTime);

                if (!IsInRange(paper.Center))
                    paper.Kill(PaperState.MISS);

                // If the paper hits a target,
                if (target != null && Vector2.Distance(paper.Center, target.Center) < 12)
                {
                    // destroy the paper and hurt the target.
                    if (paper.Paperstate == PaperState.FLYING)
                    {
                        target.CurrentHealth -= paper.Damage;
                    }
                    
                    paper.Kill(PaperState.HIT);

                    // Apply our speed modifier if it is better than
                    // the one currently affecting the target :
                    if (target.SpeedModifier <= speedModifier)
                    {
                        target.SpeedModifier = speedModifier;
                        target.ModifierDuration = modifierDuration;
                    }
                }
                if (target == null)
                {
                    paper.Kill(PaperState.MISS);
                }
                if (paper.IsDead())
                {
                    paperList.Remove(paper);
                    i--;
                }
            }
        }
    }
}
