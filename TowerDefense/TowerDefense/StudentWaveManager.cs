using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class StudentWaweManager
    {
        private int numberOfWaves; // How many waves the game will have
        private float timeSinceLastWave; // How long since the last wave ended

        private Queue<Wave> waves = new Queue<Wave>(); // A queue of all our waves

        private Texture2D[] studentTextureArray; // The texture used to draw the Students

        private bool waveFinished = false; // Is the current wave over?
        private bool soundFlag = false;//help to make sound

        private Map map; // A reference to our map class.

        public Wave CurrentWave // Get the wave at the front of the queue
        {
            get { return waves.Peek(); }
        }
        public int TotalRounds
        {
            get { return numberOfWaves; }
        }
        public List<Student> Students // Get a list of the current enemeies
        {
            get { return CurrentWave.Students; }
        }
        public int Round // Returns the wave number
        {
            get { return CurrentWave.RoundNumber + 1; }
        }
        public bool SoundFlag
        {
            get { return soundFlag; }
            set { soundFlag = value; }
        }
        public StudentWaweManager(Player player, Map map, int numberOfWaves, Texture2D[] studentTextureArray)
        {
            this.numberOfWaves = numberOfWaves;
            this.studentTextureArray = studentTextureArray;

            this.map = map;
            soundFlag = true;

            for (int i = 0; i < numberOfWaves; i++)
            {
                int initialNumerOfStudents = 10;
                int numberModifier = 2*(i / 10) + 1;

                // Pass the reference to the player, to the wave class.
                Wave wave = new Wave(i, initialNumerOfStudents * 
                    numberModifier, player, map, studentTextureArray);

                waves.Enqueue(wave);
            }

            StartNextWave();
        }

        private void StartNextWave()
        {
            if (waves.Count > 0) // If there are still waves left
            {
                soundFlag = true;
                waves.Peek().Start(); // Start the next one

                timeSinceLastWave = 0; // Reset timer
                waveFinished = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            CurrentWave.Update(gameTime); // Update the wave

            if (CurrentWave.RoundOver) // Check if it has finished
            {
                waveFinished = true;
            }

            if (waveFinished) // If it has finished
            {
                timeSinceLastWave += (float)gameTime.ElapsedGameTime.TotalSeconds; // Start the timer
            }

            if (timeSinceLastWave > 5.0f) // If 30 seconds has passed
            {
                waves.Dequeue(); // Remove the finished wave
                StartNextWave(); // Start the next wave
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentWave.Draw(spriteBatch);
        }
    }
}
