using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FloodBuds
{
    internal class Debris
    {
        private Random rng = new Random();
        private Texture2D[] sprites;
        private int[] speeds;

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
            Tire,
            Rock
        }
        private DebrisType debrisType;

        /// <summary>
        /// The direction that the debris is floating in.
        /// </summary>
        private enum Direction
        {
            Left,
            Right,
            Top,
            Bottom
        }
        private Direction direction;

        /// <summary>
        /// The constructor for a debris object.
        /// </summary>
        /// <param name="score"> The current score. </param>
        /// <param name="sprites"> The sprites for debris. </param>
        public Debris(int score, params Texture2D[] sprites)
        {
            this.sprites = sprites;
            hitbox = new Rectangle();
            speeds = [8, 5, 1];

            int temp;

            if(score <= 10)
            {
                if(rng.Next(1, 6) <= 4)
                {
                    debrisType = DebrisType.Tire;
                }
                else
                {
                    debrisType = DebrisType.DriftWood;
                }
            }
            else if(score <= 30)
            {
                temp = rng.Next(1, 101);

                if(temp <= 40)
                {
                    debrisType = DebrisType.Tire;
                }
                else if(temp <= 80)
                {
                    debrisType = DebrisType.DriftWood;
                }
                else
                {
                    debrisType = DebrisType.Rock;
                }
            }

            GenerateValues();
        }

        /// <summary>
        /// Moves the debris.
        /// </summary>
        /// <param name="xWind"> The wind speed in the x direction. </param>
        /// <param name="yWind"> The wind speed in the y direction. </param>
        public void Move(int xWind, int yWind)
        {
            // Handles simple movement.
            switch (direction)
            {
                case Direction.Left:
                    hitbox.X += speeds[(int)debrisType];
                    break;

                case Direction.Right:
                    hitbox.X -= speeds[(int)debrisType];
                    break;

                case Direction.Top:
                    hitbox.Y += speeds[(int)debrisType];
                    break;

                case Direction.Bottom:
                    hitbox.Y -= speeds[(int)debrisType];
                    break;
            }

            // Accounts for wind with Driftwood.
            if(debrisType == DebrisType.DriftWood)
            {
                hitbox.X += xWind;
                hitbox.Y += yWind;
            }
        }

        /// <summary>
        /// Checks to see if the debris leaves the bounds of the screen.
        /// </summary>
        /// <returns> Whether or not the debris has left the bounds of the screen. </returns>
        public bool CheckDespawn()
        {
            return hitbox.X > 1920 || hitbox.X < 0 - hitbox.Width || hitbox.Y > 1080 || hitbox.Y < 0 - hitbox.Height;
        }

        /// <summary>
        /// Generates the values of the type of debris that just spawned.
        /// </summary>
        public void GenerateValues()
        {
            direction = (Direction)rng.Next(0, 4);

            // Determines the size of the hitbox.
            switch (debrisType)
            {
                case DebrisType.DriftWood:

                    hitbox.Width = 160;
                    hitbox.Height = 40;

                    break;

                case DebrisType.Tire:

                    hitbox.Width = 65;
                    hitbox.Height = 65;

                    break;

                case DebrisType.Rock:

                    hitbox.Width = 125;
                    hitbox.Height = 125;

                    break;
            }

            // Determines the spawnpoint of the debris.
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

        /// <summary>
        /// Draws the debris to screen.
        /// </summary>
        /// <param name="sb"> The SpriteBatch we're drawing with. </param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprites[(int)debrisType], hitbox, Color.White);
        }
    }
}
