using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FloodBuds
{
    // Joshua Smith - Aaron Phan
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
        private KeyboardState kbs; // The current state of the keyboard.
        private KeyboardState pkbs; // The previous state of the keyboard.

        private enum GameState
        {
            MainMenu,
            Game,
            GameOver
        }
        private GameState gameState;

        #region Textures

        private Texture2D tempAsset;

        #endregion

        #region Draw Locations



        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            #region Screen Resolution

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            Window.IsBorderless = true;
            _graphics.ApplyChanges();

            #endregion
        }

        protected override void Initialize()
        {
            gameState = GameState.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            tempAsset = Content.Load<Texture2D>($"TempAsset");

            player = new Player(tempAsset);
        }

        protected override void Update(GameTime gameTime)
        {
            kbs = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbs.IsKeyDown(Keys.Escape))
                Exit();

            
            player.Update(Keyboard.GetState(), rng.Next(-4, 4), rng.Next(-4, 4));

            pkbs = kbs;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.MainMenu:

                    player.Draw(_spriteBatch);

                    break;

                case GameState.Game:

                    player.Draw(_spriteBatch);

                    break;

                case GameState.GameOver:



                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
