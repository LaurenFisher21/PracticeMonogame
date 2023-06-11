using MainGame.Objects.Base;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects
{
    public class PlayerSprite : BaseGameObject
    {
        public PlayerSprite(Texture2D texture)
        {
            _texture = texture;
        }
    }
}