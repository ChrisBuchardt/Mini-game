using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LearnExtended
{
    public abstract class AGameEntity
    {
        public Vector2 Position;
        public float Range;
        public string id;
        public Rectangle hitBox;
        public int Team;
        public string hasCollidedWith;
        public abstract void Draw(SpriteBatch spritebatch);
        public abstract void Update(GameTime gt);
        public virtual bool isColliding(AGameEntity ge)
        {
            if (hitBox.Intersects(ge.hitBox)) {
                return true;
            }
            return false;
        }

    }
}