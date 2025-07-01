using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FloodBuds
{
    internal class Player
    {
        private Texture2D sprite;
        private Rectangle hitbox;

        /// <summary>
        /// The constructor for a player object.
        /// </summary>
        /// <param name="sprite"> The player's sprite. </param>
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
            if (kb.IsKeyDown(Keys.W)) { hitbox.Y -= 7; }
            if (kb.IsKeyDown(Keys.A)) { hitbox.X -= 7; }
            if (kb.IsKeyDown(Keys.S)) { hitbox.Y += 7; }
            if (kb.IsKeyDown(Keys.D)) { hitbox.X += 7; }

            hitbox.Y += yWind;
            hitbox.X += xWind;

            // Makes sure the player doesn't move offscreen.
            if(hitbox.X < 0)
            {
                hitbox.X = 0;
            }
            if(hitbox.X > 1855)
            {
                hitbox.X = 1855;
            }
            if(hitbox.Y < 0)
            {
                hitbox.Y = 0;
            }
            if(hitbox.Y > 1015)
            {
                hitbox.Y = 1015;
            }
        }

        /// <summary>
        /// Checks to see if anything is colliding with the player.
        /// </summary>
        /// <param name="other"> The other rectangle we're checking. </param>
        /// <returns> Whether or not the player is colliding with anything. </returns>
        public bool IsColliding(Rectangle other)
        {
            return hitbox.Intersects(other);
        }

        /// <summary>
        /// Resets the position of the player.
        /// </summary>
        public void Reset()
        {
            hitbox.Location = new Point(927, 507);
        }

        /// <summary>
        /// Draws the player to the screen.
        /// </summary>
        /// <param name="sb"> The SpriteBatch we're drawing with. </param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, hitbox, Color.White);
        }
    }
}
