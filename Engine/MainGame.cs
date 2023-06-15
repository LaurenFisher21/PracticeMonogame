using System;
using MainGame.Enum;
using MainGame.States;
using MainGame.Engine.States;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NewGame.Engine
{

    public class MainGame : Game
    {
        private BaseGameState _currentGameState;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D _renderTarget;
        private Rectangle _renderScaleRectangle;

        private int _designedResolutionWidth;
        private int _designedResolutionHeight;
        private float _designedResolutionAspectRatio;

        private BaseGameState _firstGameState;

        public MainGame(int width, int height, BaseGameState firstGameState)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _firstGameState = firstGameState;
            _designedResolutionWidth = width;
            _designedResolutionHeight = height;
            _designedResolutionAspectRatio = width / (float)height;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = _designedResolutionWidth;
            _graphics.PreferredBackBufferHeight = _designedResolutionHeight;
            _graphics.IsFullScreen = false;
            Window.Title = "Glock";
            _graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _designedResolutionWidth, _designedResolutionHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            _renderScaleRectangle = GetScaleRectangle();

            base.Initialize();
        }

        private Rectangle GetScaleRectangle()
        {
            var variance = 0.5;
            var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;

            Rectangle scaleRectangle;

            if (actualAspectRatio <= _designedResolutionAspectRatio)
            {
                var presentHeight = (int)(Window.ClientBounds.Width / _designedResolutionAspectRatio + variance);
                var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

                scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                var presentWidth = (int)(Window.ClientBounds.Height * _designedResolutionAspectRatio + variance);
                var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

                scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }

            return scaleRectangle;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            SwitchGameState(_firstGameState);
        }

        private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e)
        {
            SwitchGameState(e);
        }

        private void SwitchGameState(BaseGameState gameState)
        {
            if (_currentGameState != null)
            {
                _currentGameState.OnStateSwitched -= CurrentGameState_OnStateSwitched;
                _currentGameState.OnEventNotification -= _currentGameState_OnEventNotification;
                _currentGameState.UnloadContent();
            }

            _currentGameState = gameState;

            _currentGameState.Initialize(Content, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);

            _currentGameState.LoadContent();

            _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification += _currentGameState_OnEventNotification;
        }

        private void _currentGameState_OnEventNotification(object sender, Events e)
        {
            switch (e)
            {
                case Events.GAME_QUIT:
                    Exit();
                    break;
            }
        }

        protected override void UnloadContent()
        {
            _currentGameState?.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _currentGameState.HandleInput(gameTime);
            _currentGameState.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Render to the Render Target
            GraphicsDevice.SetRenderTarget(_renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _currentGameState.Render(_spriteBatch);

            _spriteBatch.End();

            // Now render the scaled content
            _graphics.GraphicsDevice.SetRenderTarget(null);

            _graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

            _spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
