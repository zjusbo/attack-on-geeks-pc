using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class Player
    {
        /* Player Infomation*/
        private int money = 60;
        private int lives = 10;
        private Boolean beingSet = false;
        
        private Texture2D[] teacherTextures;
        private Texture2D paperTexture, paperBreakTexture;

        /*The list to store the teachers in the map*/
        private List<Teacher> teachers = new List<Teacher>();

        /*The mosue state*/
        private MouseState mouseState;
        private MouseState oldState;

        /*Teacher postion*/
        private int posX;
        private int posY;

        private int posXX;
        private int posYY;

        /*he type of teacher to add*/
        private string newTeacherType;
        /*The index of the new teachers texture*/
        private int newTeacherIndex;

        /*A reference to the map*/
        private Map map;

        public int Money
        {
            get { return money; }
            set { money = value; }
        }
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public string NewTeacherType
        {
            set { newTeacherType = value; }
        }
        public int NewTeacherIndex
        {
            set { newTeacherIndex = value; }
        }
        public Boolean TeacherBeingSet
        {
            get { return beingSet; }
            set { beingSet = value; }
        }

        public Player(Map map, Texture2D[] teacherTextures, Texture2D paperTexture, Texture2D paperBreakTexture)
        {
            /*Set a new player*/
            this.map = map;
            this.teacherTextures = teacherTextures;
            this.paperTexture = paperTexture;
            this.paperBreakTexture = paperBreakTexture;
        }

       
        private bool IsCellClear()
        {
            /*To indicate a cell is valiable*/
            /*Make sure teacher is within limits*/
            bool inBounds = posX >= 0 && posY >= 0 && 
                posX < map.Width && posY < map.Height; 

            bool spaceClear = true;

            /*Check that there is no teacher in this spot*/
            foreach (Teacher teacher in teachers) 
            {
                spaceClear = (teacher.Position != new Vector2(posXX, posYY));

                if (!spaceClear)
                {
                    break;
                }
            }

            bool onPath = (map.GetIndex(posX, posY) != 1);

            /*If both checks are true return true*/
            return inBounds && spaceClear && onPath; 
        }

       
        public void AddTeacher()
        {
            /*Add a teacher to the map*/
            Teacher teacherToAdd = null;

            switch (newTeacherType)
            {
                case "T1 Teacher":
                {
                    teacherToAdd = new T1Teacher(teacherTextures[0],
                        paperTexture, new Vector2(posXX, posYY), paperBreakTexture);
                    break;
                }
                case "T2 Teacher":
                {
                    teacherToAdd = new T2Teacher(teacherTextures[1],
                        paperTexture, new Vector2(posXX, posYY), paperBreakTexture);
                    break;
                }
                case "T3 Teacher":
                {
                    teacherToAdd = new T3Teacher(teacherTextures[2],
                        paperTexture, new Vector2(posXX, posYY), paperBreakTexture);
                    break;
                }
                case "T4 Teacher":
                {
                    teacherToAdd = new T4Teacher(teacherTextures[3],
                        paperTexture, new Vector2(posXX, posYY), paperBreakTexture);
                    break;
                }
                case "T5 Teacher":
                {
                    teacherToAdd = new T5Teacher(teacherTextures[4],
                        paperTexture, new Vector2(posXX, posYY), paperBreakTexture);
                    break;
                }
                case "T6 Teacher":
                {
                    teacherToAdd = new T6Teacher(teacherTextures[5],
                        paperTexture, new Vector2(posXX, posYY), paperBreakTexture);
                    break;
                }
            }

            /*Check player's money and if the cell is avaible*/
            if (IsCellClear() == true && teacherToAdd.Cost <= money)
            {
                teachers.Add(teacherToAdd);
                money -= teacherToAdd.Cost;
                beingSet = true;
                newTeacherType = string.Empty;
            }

            else
            {
                newTeacherType = string.Empty;
            }
        }


        public void Update(GameTime gameTime, List<Student> students)
        {
            mouseState = Mouse.GetState();

            /*Convert the position of the mouse*/
            posX = (int)(mouseState.X / 60); 
            posY = (int)(mouseState.Y / 60);

            /*Convert from array space to map space*/
            posXX = posX * 60; 
            posYY = posY * 60; 

            /*Listen to the mouse and if the mouse is pressed, set a new teacher*/
            if (mouseState.LeftButton == ButtonState.Released
                && oldState.LeftButton == ButtonState.Pressed)
            {
                if (string.IsNullOrEmpty(newTeacherType) == false)
                {
                    AddTeacher();
                }
            }

            /*Update all the teachers*/
            foreach (Teacher teacher in teachers)
            {
                if (teacher.HasTarget == false)
                {
                    teacher.GetClosestStudent(students);
                }

                teacher.Update(gameTime);
            }
            /*Set the oldState so it becomes the state of the previous frame*/
            oldState = mouseState; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /*Draw all the teachers*/
            foreach (Teacher teacher in teachers)
            {
                teacher.Draw(spriteBatch);
            }
        }

        public void DrawPreview(SpriteBatch spriteBatch)
        {
            /*Draw the teacher preview*/
            if (string.IsNullOrEmpty(newTeacherType) == false)
            {
                /*Convert the position of the mouse from array space to map space*/
                int posX = (int)(mouseState.X / 60); 
                int posY = (int)(mouseState.Y / 60);

                /*Convert from array space to map space*/
                int posXX = posX * 60; 
                int posYY = posY * 60; 

                Texture2D previewTexture = teacherTextures[newTeacherIndex];
                int width = previewTexture.Width / 6;
                int height = previewTexture.Height;
                spriteBatch.Draw(previewTexture, new Rectangle(posXX, posYY,
                    width, height), new Rectangle(0, 0, width, height), Color.White);

            }
        }

    }
}
