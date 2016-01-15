using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AttackOnGeek
{
    public class Toolbar
    {
        private Texture2D texture;
        // A class to access the font we created
        private SpriteFont font;

        // The position of the toolbar
        private Vector2 position;
        // The position of the text
        private Vector2 textPosition;

        public Toolbar(Texture2D texture, SpriteFont font, Vector2 position)
        {
            this.texture = texture;
            this.font = font;

            this.position = position;
            // Offset the text to the bottom right corner
            textPosition = new Vector2(450, position.Y + 470);
        }

        public void Draw(SpriteBatch spriteBatch, Player player, int waveNumber, int totalWaveNumber)
        {
            spriteBatch.Draw(texture, position, Color.White);

            string text = string.Format("Gold : {0} Lives : {1}", player.Money, player.Lives);
            string help = string.Format("Press \"H\" to view \"Help\"\nPress \"Esc\" to view \"Main Menu\"");
            string text1 = string.Format("15");
            string text2 = string.Format("25");
            string text3 = string.Format("40");
            string swaveNumber = string.Format("Round: {0}/{1}", waveNumber, totalWaveNumber - 1);
            spriteBatch.DrawString(font, text, textPosition + new Vector2(160, -25), Color.Black);
            spriteBatch.DrawString(font, help, textPosition + new Vector2(160, -10), Color.Black);
            spriteBatch.DrawString(font, text1, new Vector2(47, 610), Color.Black);
            spriteBatch.DrawString(font, text1, new Vector2(111, 610), Color.Black);
            spriteBatch.DrawString(font, text1, new Vector2(175, 610), Color.Black);
            spriteBatch.DrawString(font, text1, new Vector2(239, 610), Color.Black);
            spriteBatch.DrawString(font, text2, new Vector2(303, 610), Color.Black);
            spriteBatch.DrawString(font, text3, new Vector2(368, 610), Color.Black);
            spriteBatch.DrawString(font, swaveNumber, textPosition, Color.Black);
        }

    }
}
