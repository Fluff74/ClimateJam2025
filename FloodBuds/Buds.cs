using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FloodBuds
{
    internal class Buds
    {
        private Random rng = new Random();
        private Texture2D sprite;
        private Rectangle hitbox;
        public Rectangle Hitbox { get { return hitbox; } set { Hitbox = value; } }

        private enum Direction
        {
            Left,
            Right,
            Top,
            Bottom
        }

        private Direction direction;

        public Buds(Texture2D sprite)
        {
            this.sprite = sprite;
            hitbox = new Rectangle(0, 0, 65, 65);
        }

        public void Move()
        {
            switch (direction)
            {
                case Direction.Left:
                    hitbox.X += 5;
                    break;

                case Direction.Right:
                    hitbox.X -= 5;
                    break;

                case Direction.Top:
                    hitbox.Y += 5;
                    break;

                case Direction.Bottom:
                    hitbox.Y -= 5;
                    break;
            }
        }

        public bool CheckDespawn()
        {
            switch (direction)
            {
                case Direction.Left:
                    return hitbox.X > 1920;

                case Direction.Right:
                    return hitbox.X < 0 - hitbox.Width;

                case Direction.Top:
                    return hitbox.Y > 1080;

                case Direction.Bottom:
                    return hitbox.Y < 0 - hitbox.Height;
            }
            return false;
        }

        public void GenerateValues()
        {

            direction = (Direction)rng.Next(0, 4);
            
            switch (direction)
            {
                // Left side
                case Direction.Left:

                    hitbox.X = 0 - hitbox.Width;
                    hitbox.Y = rng.Next(1, 1080 - hitbox.Height);

                    break;

                // Right side
                case Direction.Right:

                    hitbox.X = 1920;
                    hitbox.Y = rng.Next(1, 1080 - hitbox.Height);

                    break;

                // Top
                case Direction.Top:

                    hitbox.X = rng.Next(1, 1920 - hitbox.Width);
                    hitbox.Y = 0 - hitbox.Height;

                    break;

                // Bottom
                case Direction.Bottom:

                    hitbox.X = rng.Next(1, 1920 - hitbox.Width);
                    hitbox.Y = 1080;

                    break;
            }
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, hitbox, Color.White);
        }
    }
}
