using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class Wave
    {
        /*Number of Students to spawn*/
        private int numOfStudents; 
        private int waveNumber;

        /*The time to spawn an Student*/
        private float spawnTimer = 0;
        /*How many Students have spawned in one round*/
        private int studentsSpawned = 0; 
        /*The last student*/
        private bool studentAtEnd; 
        /*To indicate if the students are spawn*/
        private bool spawningStudents;

        /*The refrence of the player and map*/
        private Player player;
        private Map map;

        /*The reference of the student texture*/
        private Texture2D[] studentTextureArray; 
        /*The list to store students*/
        private List<Student> students = new List<Student>();

        public bool RoundOver
        {
            get 
            { 
                return students.Count == 0 && studentsSpawned == numOfStudents; 
            }
        }
        public int RoundNumber
        {
            get { return waveNumber; }
        }

        public bool StudentAtEnd
        {
            get { return studentAtEnd; }
            set { studentAtEnd = value; }
        }
        public List<Student> Students
        {
            get { return students;}
        }

        public Wave(int waveNumber, int numOfStudents,
            Player player, Map map, Texture2D[] studentTextureArray)
        {
            /*A new wave of students*/
            this.waveNumber = waveNumber;
            this.numOfStudents = numOfStudents;

            this.player = player;
            this.map = map;

            this.studentTextureArray = studentTextureArray;
        }

        private void AddStudent()
        {
            /*Randomly select a student to add*/
            int random = new Random().Next();
            Student student = new Student(studentTextureArray[random%4],
                map.Waypoints.Peek(), 30+10*waveNumber, 2+waveNumber, 0.75f+0.2f*waveNumber, random%4);

            student.SetWaypoints(map.Waypoints);

            students.Add(student);

            spawnTimer = 0;
            studentsSpawned++;
        }

        public void Start()
        {
            spawningStudents = true;
        }

        public void Update(GameTime gameTime)
        {
            /*The number of students in one round is fixed*/
            if (studentsSpawned == numOfStudents)
                spawningStudents = false; 

            /*The interval between two students*/
            if (spawningStudents)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (spawnTimer > 2-waveNumber*0.1)
                    AddStudent(); 
            }
            /*Update different students*/
            for (int i = 0; i < students.Count; i++)
            {
                Student student = students[i];
                student.Update(gameTime);

                if (student.IsDead)
                {
                    if (student.CurrentHealth > 0)
                    {
                        StudentAtEnd = true;
                        player.Lives -= 1;
                    }

                    else
                    {
                        player.Money += student.BountyGiven;
                    }

                    Students.Remove(student);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /*Draw the students*/
            foreach (Student student in students)
                student.Draw(spriteBatch);
        }
    }
}
