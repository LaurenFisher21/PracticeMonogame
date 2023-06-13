using MainGame.Objects;
using MainGame.States.Base;
using MainGame.Enum;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MainGame.Input;
using MainGame.Input.Base;

using System.Collections.Generic;

namespace MainGame.States
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("splash")));
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is SplashInputCommand.GameSelect)
                {
                    SwitchState(new GameplayState());
                }
            });
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }
    }
}