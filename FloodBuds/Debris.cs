using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FloodBuds
{
    internal class Debris
    {

        private Texture2D sprite;

        /// <summary>
        /// The hitbox of the debris. Dependent on which type of debris it is.
        /// </summary>
        private Rectangle hitbox;
        public Rectangle Hitbox { get { return hitbox; } set { Hitbox = value; } }


        /// <summary>
        /// The types of debris to spawn
        /// </summary>
        private enum DebrisType
        {
            DriftWood,
            Tree,
            Car
        }

        private DebrisType debrisType;

        public Debris(Texture2D sprite)
        {

            this.sprite = sprite;
            hitbox = new Rectangle();
        }

        public void Move()
        {
            switch (debrisType)
            {
                case DebrisType.DriftWood:
                    hitbox.X += 5;
                    break;

                case DebrisType.Tree:
                    hitbox.X += 10;
                    break;

                case DebrisType.Car:
                    hitbox.X += 1;
                    break;
            }
            if(hitbox.X > 1920)
            {
                hitbox.X = -200;
            }
        }

        public void Draw(SpriteBatch sb)
        { 
            sb.Draw(sprite, hitbox, Color.White);
        }
    }
}
