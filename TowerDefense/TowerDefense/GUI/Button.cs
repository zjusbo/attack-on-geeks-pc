using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AttackOnGeek
{
    /// <summary>
    /// Describes the state of the button.
    /// </summary>
    public enum ButtonStatus
    {
        Normal,
        MouseOver,
        Pressed,
    }
    
    /// <summary>
    /// Stores the appearance and functionality of a button.
    /// </summary>
    public class Button : Sprite
    {
        // Store the MouseState of the last frame.
        private MouseState previousState;

        // The the different state textures.
        private Texture2D hoverTexture;
        private Texture2D pressedTexture;

        // A rectangle that covers the button.
        private Rectangle bounds;

        private Rectangle textureBound;
        // Store the current state of the button.
        private ButtonStatus state = ButtonStatus.Normal;

        // Gets fired when the button is pressed.
        public event EventHandler Clicked;
        // Gets fired when the button is held down.
        public event EventHandler OnPress;
        public ButtonStatus State
        {
            get { return state; }
        }
        /// <summary>
        /// Constructs a new button.
        /// </summary>
        /// <param name="texture">The normal texture for the button.</param>
        /// <param name="hoverTexture">The texture drawn when the mouse is over the button.</param>
        /// <param name="pressedTexture">The texture drawn when the button has been pressed.</param>
        /// <param name="position">The position where the button will be drawn.</param>
        public Button(Texture2D texture, Texture2D hoverTexture, Texture2D pressedTexture, Vector2 position, int frame=1)
            : base(texture, position)
        {
            this.hoverTexture = hoverTexture;
            this.pressedTexture = pressedTexture;
            this.textureBound = new Rectangle(0, 0, texture.Width/frame, texture.Height);
            this.bounds = new Rectangle((int)position.X, (int)position.Y, 
                texture.Width/frame, texture.Height);
        }

        /// <summary>
        /// Updates the buttons state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Determine if the mouse if over the button.
            MouseState mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            bool isMouseOver = bounds.Contains(mouseX, mouseY);

            // Update the button state.
            if (isMouseOver && state != ButtonStatus.Pressed)
            {
                state = ButtonStatus.MouseOver;
            }
            else if (isMouseOver == false && state != ButtonStatus.Pressed)
            {
                state = ButtonStatus.Normal;
            }

            // Check if the player holds down the button.
            if (mouseState.LeftButton == ButtonState.Pressed &&
                previousState.LeftButton == ButtonState.Released)
            {
                if (isMouseOver == true)
                {
                    // Update the button state.
                    state = ButtonStatus.Pressed;

                    if (OnPress != null)
                    {
                        // Fire the OnPress event.
                        OnPress(this, EventArgs.Empty);
                    }
                }
            }

            // Check if the player releases the button.
            if (mouseState.LeftButton == ButtonState.Released &&
                previousState.LeftButton == ButtonState.Pressed)
            {
                if (isMouseOver == true)
                {
                    // update the button state.
                    state = ButtonStatus.MouseOver;

                    if (Clicked != null)
                    {
                        // Fire the clicked event.
                        Clicked(this, EventArgs.Empty);
                    }
                }

                else if (state == ButtonStatus.Pressed)
                {
                    state = ButtonStatus.Normal;
                }
            }

            previousState = mouseState;
        }

        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch that has been started</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case ButtonStatus.Normal:
                    spriteBatch.Draw(texture, position, textureBound, Color.White);
                    break;
                case ButtonStatus.MouseOver:
                    spriteBatch.Draw(texture, position + new Vector2(0, -10), textureBound, new Color(200, 230, 80));
                    break;
                case ButtonStatus.Pressed:
                    spriteBatch.Draw(texture, position + new Vector2(0, -10), textureBound, Color.White);
                    break;
                default:
                    spriteBatch.Draw(texture, position, textureBound, Color.White);
                    break;
            }
        }
    }
}
