using MainGame.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects
{
    public class SplashImage : BaseGameObject
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }
    }
}