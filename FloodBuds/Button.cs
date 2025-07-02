using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FloodBuds
{
    internal class Button
    {
        private Rectangle hitbox;
        private Texture2D button;
        private bool hovering;

        public Button(Rectangle hitbox, Texture2D button)
        {
            this.hitbox = hitbox;
            this.button = button;

            hovering = false;
        }

        /// <summary>
        /// Checks if the button is being hovered over, and if it's clicked.
        /// </summary>
        /// <param name="ms"> The state of the mouse. </param>
        /// <returns> Whether or not the button was clicked. </returns>
        public bool Update(MouseState ms, MouseState pms)
        {
            hovering = hitbox.Contains(ms.Position);
            return hovering && ms.LeftButton == ButtonState.Pressed && pms.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Draws the button to the screen.
        /// </summary>
        /// <param name="sb"> The SpriteBatch we're drawing with. </param>
        public void Draw(SpriteBatch sb)
        {
            if (hovering)
            {
                sb.Draw(button, hitbox, Color.Gray);
            }
            else
            {
                sb.Draw(button, hitbox, Color.White);
            }
        }
    }
}
