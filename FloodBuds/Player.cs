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

        public void Update(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.W)) { hitbox.Y -= 5; }
            if (kb.IsKeyDown(Keys.A)) { hitbox.X -= 5; }
            if (kb.IsKeyDown(Keys.S)) { hitbox.Y += 5; }
            if (kb.IsKeyDown(Keys.D)) { hitbox.X += 5; }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, hitbox, Color.White);
        }
    }
}
