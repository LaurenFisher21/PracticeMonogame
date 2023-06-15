using NewGame.Engine;
using MainGame.States;
using System;

namespace NewGame
{
    public static class Program
    {
            private const int WIDTH = 1280;
            private const int HEIGHT = 720;

            [STAThread]
            static void Main()
            {
                using (var game = new NewGame.Engine.MainGame(WIDTH, HEIGHT, new SplashState())) game.Run();
            }

    }

}

