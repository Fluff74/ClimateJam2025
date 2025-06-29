using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        #region Textures

        private Texture2D tempAsset;

        #endregion

        #region Draw Locations

        private Rectangle tempRect;

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
            tempRect = new Rectangle(200, 200, 200, 200);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            tempAsset = Content.Load<Texture2D>($"TempAsset");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(tempAsset, tempRect, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
