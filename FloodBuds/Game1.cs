using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        private Player player;
        private Rescue rescue;
        private KeyboardState kbs; // The current state of the keyboard.
        private KeyboardState pkbs; // The previous state of the keyboard.
        private Matrix screenRes; // This will automatically scale our game to other people's screen sizes.

        private int xWind;
        private int yWind;

        private List<Debris> debrisList;

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

        #endregion

        #region Draw Locations

        private Rectangle fbLogoDrawRect;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            tempAsset = Content.Load<Texture2D>($"TempAsset");
            fbLogo = Content.Load<Texture2D>($"FB_Logo");

            player = new Player(Content.Load<Texture2D>($"FB_Raft"));
            rescue = new Rescue(Content.Load<Texture2D>($"FB_Rescue"));
            rescue.Active = true;
        }

        protected override void Update(GameTime gameTime)
        {
            kbs = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbs.IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameState.MainMenu:

                    if (SingleKeyPress(Keys.Enter))
                    {
                        RandomizeWind();
                        gameState = GameState.Game;
                    }

                    break;

                case GameState.Game:

                    player.Update(Keyboard.GetState(), xWind, yWind); // Updates the player's location based on wind and input.

                    // Move the rescue ship while it is active.
                    if (rescue.Active)
                    {
                        rescue.Move();
                    }

                    // If the player touches the rescue ship, do this.
                    if (player.IsColliding(rescue.Hitbox))
                    {
                        // Functionality
                    }

                    // Handles the debris.
                    for(int i = 0; i < debrisList.Count; i++)
                    {
                        debrisList[i].Move(xWind, yWind);

                        // Remove debris that leaves the bounds of the screen.
                        if (debrisList[i].CheckDespawn())
                        {
                            debrisList.RemoveAt(i);
                            i--;
                        }

                        // Checks if the player has hit any debris.
                        else if (player.IsColliding(debrisList[i].Hitbox))
                        {
                            player.Reset();
                            debrisList.Clear();
                            gameState = GameState.GameOver;
                        }
                    }

                    break;

                case GameState.GameOver:



                    break;
            }

            pkbs = kbs;
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

                    break;

                case GameState.Game:

                    player.Draw(_spriteBatch);
                    rescue.Draw(_spriteBatch);

                    for(int i = 0; i < debrisList.Count; i++)
                    {
                        debrisList[i].Draw(_spriteBatch);
                    }

                    break;

                case GameState.GameOver:



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
        /// Randomizes the wind speed.
        /// </summary>
        private void RandomizeWind()
        {
            xWind = rng.Next(-4, 5);
            yWind = rng.Next(-4, 5);
        }
    }
}
