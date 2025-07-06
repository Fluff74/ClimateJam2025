using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FloodBuds
{
    internal class Rescue
    {
        private Texture2D sprite;

        /// <summary>
        /// The hitbox of our rescue ship.
        /// </summary>
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        private Rectangle hitbox;

        /// <summary>
        /// Whether or not the rescue ship is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// The constructor for the rescue boat.
        /// </summary>
        /// <param name="sprite"> The sprite for the rescue boat. </param>
        public Rescue(Texture2D sprite)
        {
            this.sprite = sprite;
            hitbox = new Rectangle(-200, 0, 200, 115);
            Active = false;
        }

        /// <summary>
        /// Moves the rescue ship.
        /// </summary>
        public void Move()
        {
            hitbox.X += 3;

            if(hitbox.X > 1920)
            {
                Active = false;
                hitbox.X = -200;
            }
        }

        /// <summary>
        /// Resets the Rescue boat.
        /// </summary>
        public void Reset()
        {
            Active = false;
            hitbox.X = -200;
        }

        /// <summary>
        /// Draws the rescue ship to the screen.
        /// </summary>
        /// <param name="sb"> The SpriteBatch we're drawing with. </param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, hitbox, Color.White);
        }
    }
}
