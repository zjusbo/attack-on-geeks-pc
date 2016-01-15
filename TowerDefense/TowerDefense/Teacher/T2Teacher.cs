using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class T2Teacher : Teacher
    {
        public T2Teacher(Texture2D texture, Texture2D paperTexture, Vector2 position, Texture2D paperBreakTexture, int totalFrame = 6)
            : base(texture, paperTexture, position, paperBreakTexture, totalFrame)
        {
            this.damage = 10; // Set the damage
            this.cost = 15;   // Set the initial cost

            this.radius = 120; // Set the radius
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (paperTimer >= 0.75f && target != null)
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

                if (target != null && Vector2.Distance(paper.Center, target.Center) < 12)
                {
                    if (target.StudentType != 1 && paper.Paperstate == PaperState.FLYING)
                    {
                         target.CurrentHealth -= paper.Damage;
                    }
                    paper.Kill(PaperState.HIT);
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
