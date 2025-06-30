using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FloodBuds
{
    internal class Player
    {
        private Texture2D sprite;
        private Rectangle hitbox;

        public Player(Texture2D sprite)
        {
            this.sprite = sprite;

            hitbox = new Rectangle(927, 507, 65, 65);
        }

        /// <summary>
        /// Updates the player's movement based on the keyboard state, and based on the wind direction.
        /// </summary>
        /// <param name="kb"> The current state of the player's keyboard. </param>
        /// <param name="xWind"> The force of the wind in the X-Axis. </param>
        /// <param name="yWind"> The force of the wind in the Y-Axis. </param>
        public void Update(KeyboardState kb, int xWind, int yWind)
        {
            if (kb.IsKeyDown(Keys.W)) { hitbox.Y -= 5 - yWind; }
            if (kb.IsKeyDown(Keys.A)) { hitbox.X -= 5 - xWind; }
            if (kb.IsKeyDown(Keys.S)) { hitbox.Y += 5 + yWind; }
            if (kb.IsKeyDown(Keys.D)) { hitbox.X += 5 + xWind; }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, hitbox, Color.White);
        }
    }
}
