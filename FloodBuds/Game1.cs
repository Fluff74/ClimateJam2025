using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;

namespace FloodBuds
{
    // Joshua Smith - Aaron Phan - Oliver Dingus
    // 06/29/2025
    //
    // We're making a game for the Climate Jam 2025. This won't be submitted for the compilation, as it is not made in Unity.
    // Hopefully it's good experience :)
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Random rng = new Random();

        private SpriteFont arial;
        private Player player;
        private Rescue rescue;
        private KeyboardState kbs; // The current state of the keyboard.
        private KeyboardState pkbs; // The previous state of the keyboard.
        private GamePadState gps; // The current state of the game controller.
        private GamePadState pgps; // The current state of the game controller.
        private MouseState ms;
        private MouseState pms;
        private Matrix screenRes; // This will automatically scale our game to other people's screen sizes.
        private int score;
        private int highscore;
        private int diffIncrement;
        private int currBuds; // The amount of buds onboard.
        private string windDir;
        private double windTimer;
        private double rescueTimer;

        private int xWind;
        private int yWind;

        private List<Debris> debrisList;
        private List<Buds> budsList;

        private enum GameState
        {
            MainMenu,
            Game,
            GameOver
        }
        private GameState gameState;

        #region Textures

        private Texture2D tempAsset;
        private Texture2D fbLogo;
        private Texture2D gameOver;
        private Texture2D tire;
        private Texture2D driftWood;
        private Texture2D rock;
        private Texture2D buds;

        #endregion

        #region Draw Locations

        private Rectangle fbLogoDrawRect;
        private Rectangle gameOverDrawRect;
        private Vector2 scoreVec;
        private Vector2 capacityVec;
        private Vector2 windVec;
        private Vector2 startInstructions;
        private Vector2 quitInstructions;
        private Vector2 highscoreVec;

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            #region Screen Resolution

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Window.IsBorderless = true;
            _graphics.ApplyChanges();

            #endregion
        }

        protected override void Initialize()
        {
            gameState = GameState.MainMenu;
            debrisList = new List<Debris>();
            screenRes = Matrix.CreateScale((float)_graphics.PreferredBackBufferWidth / 1920, (float)_graphics.PreferredBackBufferHeight / 1080, 1.0f);
            fbLogoDrawRect = new Rectangle(0, -60, 700, 700);
            gameOverDrawRect = new Rectangle(80, 20, 700, 700);
            score = 0;
            diffIncrement = 0;
            currBuds = 0;
            budsList = new List<Buds>();

            scoreVec = new Vector2(20, 930);
            capacityVec = new Vector2(20, 980);
            windVec = new Vector2(20, 1030);
            highscoreVec = new Vector2(1200, 300);
            startInstructions = new Vector2(1240, 740);
            quitInstructions = new Vector2(900, 900);

            windTimer = rng.Next(12, 18);
            rescueTimer = rng.Next(12, 18);

            if (File.Exists($"Highscore"))
            {
                FileStream file = File.OpenRead($"Highscore");
                BinaryReader reader = new BinaryReader(file);

                try
                {
                    highscore = reader.ReadInt32();
                }
                catch
                {
                    highscore = 0;
                }
                finally
                {
                    reader.Close();
                }
            }
            else
            {
                File.Create($"Highscore");
                highscore = 0;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            arial = Content.Load<SpriteFont>($"Arial");

            tempAsset = Content.Load<Texture2D>($"TempAsset");
            fbLogo = Content.Load<Texture2D>($"FB_Logo");
            gameOver = Content.Load<Texture2D>($"FB_GameOver");
            tire = Content.Load<Texture2D>($"FB_Tire");
            driftWood = Content.Load<Texture2D>($"FB_DriftWood");
            rock = Content.Load<Texture2D>($"FB_Rock");
            buds = Content.Load<Texture2D>($"FB_Buds");

            player = new Player(Content.Load<Texture2D>($"FB_Raft"));
            rescue = new Rescue(Content.Load<Texture2D>($"FB_Rescue"));
        }

        protected override void Update(GameTime gameTime)
        {
            kbs = Keyboard.GetState();
            ms = Mouse.GetState();
            gps = GamePad.GetState(0);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbs.IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameState.MainMenu:

                    // Start game functionality.
                    if (SingleKeyPress(Keys.Enter) || SingleButtonPress(Buttons.A))
                    {
                        RandomizeWind();
                        gameState = GameState.Game;
                        budsList.Add(new Buds(buds));
                        debrisList.Add(new Debris(score, driftWood, tire, rock));
                    }

                    break;

                case GameState.Game:

                    player.Update(kbs, gps, xWind, yWind); // Updates the player's location based on wind and input.

                    windTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                    rescueTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    if(windTimer < 0)
                    {
                        RandomizeWind();
                        windTimer = rng.Next(12, 18);
                    }
                    if(rescueTimer < 0)
                    {
                        rescue.Active = true;
                        rescueTimer = rng.Next(17, 23);
                    }

                    // Move the rescue ship while it is active.
                    if (rescue.Active)
                    {
                        rescue.Move();
                    }

                    // If the player touches the rescue ship, deposit buds.
                    if (player.IsColliding(rescue.Hitbox))
                    {
                        score += currBuds;
                        currBuds = 0;
                    }

                    // Handles the debris.
                    for(int i = 0; i < debrisList.Count; i++)
                    {
                        debrisList[i].Move(xWind, yWind);

                        // Remove debris that leaves the bounds of the screen.
                        if (debrisList[i].CheckDespawn())
                        {
                            debrisList.RemoveAt(i);
                            debrisList.Add(new Debris(score, driftWood, tire, rock));
                            i--;
                        }

                        // Checks if the player has hit any debris.
                        else if (player.IsColliding(debrisList[i].Hitbox))
                        {
                            // Updates their highscore if it's bigger than before.
                            if (score > highscore)
                            {
                                highscore = score;

                                FileStream file = File.OpenWrite($"Highscore");
                                BinaryWriter writer = new BinaryWriter(file);

                                writer.Write(highscore);
                                writer.Close();
                            }

                            player.Reset();
                            debrisList.Clear();
                            budsList.Clear();
                            score = 0;
                            currBuds = 0;
                            diffIncrement = 0;
                            windTimer = rng.Next(12, 18);
                            rescueTimer = rng.Next(12, 18);

                            gameState = GameState.GameOver;
                        }
                    }

                    // Handles the buds.
                    for (int i = 0; i < budsList.Count; i++)
                    {
                        budsList[i].Move();

                        // Remove buds that leave the bounds of the screen.
                        if (budsList[i].CheckDespawn())
                        {
                            budsList.RemoveAt(i);
                            i--;

                            // Decrement score if a bud despawns.
                            if(score > 0)
                            {
                                score--;
                            }

                            budsList.Add(new Buds(buds));
                        }

                        // Checks if the player has saved any buds.
                        else if (player.IsColliding(budsList[i].Hitbox) && currBuds < 7)
                        {
                            budsList.RemoveAt(i);
                            i--;

                            currBuds++;

                            diffIncrement++;
                            if(diffIncrement >= 10)
                            {
                                debrisList.Add(new Debris(score, driftWood, tire, rock));
                                diffIncrement = 0;
                            }

                            budsList.Add(new Buds(buds));
                        }
                    }

                    break;

                case GameState.GameOver:

                    if (SingleKeyPress(Keys.Enter) || SingleButtonPress(Buttons.A))
                    {
                        gameState = GameState.MainMenu;
                    }

                    break;
            }

            pkbs = kbs;
            pms = ms;
            pgps = gps;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            _spriteBatch.Begin(transformMatrix: screenRes);

            switch (gameState)
            {
                case GameState.MainMenu:

                    player.Draw(_spriteBatch);
                    _spriteBatch.Draw(fbLogo, fbLogoDrawRect, Color.White);
                    _spriteBatch.DrawString(arial, $"Press 'enter' or the 'a' button to start!", startInstructions, Color.Orange);
                    _spriteBatch.DrawString(arial, $"Quit any time with `escape` or the `-` button.", quitInstructions, Color.Orange);
                    _spriteBatch.DrawString(arial, $"Highscore: {highscore}", highscoreVec, Color.Yellow);

                    break;

                case GameState.Game:

                    for(int i = 0; i < debrisList.Count; i++)
                    {
                        debrisList[i].Draw(_spriteBatch);
                    }

                    for (int i = 0; i < budsList.Count; i++)
                    {
                        budsList[i].Draw(_spriteBatch);
                    }

                    rescue.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);

                    _spriteBatch.DrawString(arial, $"Score: {score}", scoreVec, Color.White);
                    _spriteBatch.DrawString(arial, $"Buds Capacity: {currBuds}/7", capacityVec, Color.White);
                    _spriteBatch.DrawString(arial, $"Wind Direction: {windDir}", windVec, Color.White);

                    break;

                case GameState.GameOver:

                    _spriteBatch.Draw(gameOver, gameOverDrawRect, Color.White);
                    _spriteBatch.DrawString(arial, $"Press 'enter' or the 'a' button to \n     return to the main menu", startInstructions, Color.Orange);

                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Checks to see if the player pushed a key once.
        /// </summary>
        /// <param name="key"> The key we're checking. </param>
        /// <returns> Whether or not it was pushed once. </returns>
        private bool SingleKeyPress(Keys key)
        {
            return kbs.IsKeyDown(key) && pkbs.IsKeyUp(key);
        }

        /// <summary>
        /// Checks to see if the player pushed a button once.
        /// </summary>
        /// <param name="button"> The button we're checking. </param>
        /// <returns> Whether or not it was pushed once. </returns>
        private bool SingleButtonPress(Buttons button)
        {
            return gps.IsButtonDown(button) && pgps.IsButtonUp(button);
        }

        /// <summary>
        /// Randomizes the wind speed.
        /// </summary>
        private void RandomizeWind()
        {
            xWind = rng.Next(-4, 5);
            yWind = rng.Next(-4, 5);

            if(yWind < 0)
            {
                windDir = "North";
                if(xWind < 0)
                {
                    windDir += "west";
                }
                else if(xWind > 0)
                {
                    windDir += "east";
                }
            }
            else if (yWind > 0)
            {
                windDir = "South";
                if (xWind < 0)
                {
                    windDir += "west";
                }
                else if (xWind > 0)
                {
                    windDir += "east";
                }
            }
            else
            {
                if (xWind < 0)
                {
                    windDir = "West";
                }
                else if (xWind > 0)
                {
                    windDir = "East";
                }
                else
                {
                    windDir = "None!";
                }
            }
        }
    }
}
