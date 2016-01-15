using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class T6Teacher : Teacher//spike teacher
    {
        // A list of directions that the teacher can shoot in.
     
        private Vector2[] directions = new Vector2[8];
        // All the enimes that are in range of the teacher.
        private List<Student> targets = new List<Student>();

       
        public override bool HasTarget
        {
            // The teacher will never have just one target.
            get { return false; }
        }

        /// <summary>
        /// Constructs a new Spike Teacher object.
        /// </summary>
        public T6Teacher(Texture2D texture, Texture2D paperTexture, Vector2 position, Texture2D paperBreakTexture, int totalFrame = 6)
            : base(texture, paperTexture, position, paperBreakTexture, totalFrame)
        {
            this.damage = 30; // Set the damage.
            this.cost = 40;   // Set the initial cost.

            this.radius = 180; // Set the radius.

            // Store a list of all the directions the teacher can shoot.
            directions = new Vector2[]
            {
                new Vector2(-1, -1), // North West
                new Vector2( 0, -1), // North
                new Vector2( 1, -1), // North East
                new Vector2(-1,  0), // West
                new Vector2( 1,  0), // East
                new Vector2(-1,  1), // South West
                new Vector2( 0,  1), // South
                new Vector2( 1,  1), // South East
            };
        }

        public override void GetClosestStudent(List<Student> students)
        {
            // Do a fresh search for targets.
            targets.Clear();

            // Loop over all the students.
            foreach (Student student in students)
            {
                // Check wether this student is in shooting distance.
                if (IsInRange(student.Center))
                {
                    // Make it a target.
                    targets.Add(student);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Decide if it is time to shoot.
            if (paperTimer >= 1.5f && targets.Count != 0)
            {
 
                // For every direction the teacher can shoot,
                for (int i = 0; i < directions.Length; i++)
                {
                    // create a new paper that moves in that direction.
                    Paper paper = new Paper(paperTexture, paperBreakTexture, Vector2.Subtract(center,
                        new Vector2(paperTexture.Width / 2)), directions[i], 5, damage);

                    paperList.Add(paper);
                }

                paperTimer = 0;
            }

            // Loop through all the papers.
            for (int i = 0; i < paperList.Count; i++)
            {
                Paper paper = paperList[i];
                paper.Update(gameTime);

                // Kill the paper when it is out of range.
                if (!IsInRange(paper.Center))
                {
                    paper.Kill(PaperState.MISS);
                }

                // Loop through all the possible targets
                for (int t = 0; t < targets.Count; t++)
                {
                    // If this paper hits a target and is in range,
                    if (targets[t] != null && Vector2.Distance(paper.Center, targets[t].Center) < 12)
                    {
                        // hurt the student.
                        if (paper.Paperstate == PaperState.FLYING)
                        {
                            targets[t].CurrentHealth -= paper.Damage;
                        }
                        
                        paper.Kill(PaperState.HIT);

                        // This paper can't kill anyone else.
                        break;
                    }
                }
                
                // Remove the paper if it is dead.
                if (paper.IsDead())
                {
                    paperList.Remove(paper);
                    i--;
                }

            }
            if (paperList.Count > 0)
            {
                RefreshSpeed = 4;
            }
            else { RefreshSpeed = 0; }
        }
    }
}
