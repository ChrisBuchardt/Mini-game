using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LearnExtended
{
    public abstract class AGameEntity
    {
        public enum EntityType
        {
            Blue,
            Red,
            Green,
            Player,
            Purple,
            Cyan,
            Yellow,
            Neutral

        }

        public enum eTeam
        {
            Player,
            Enemy,
            Projectile
        }


        public Vector2 Position;
        public float Range;
        public EntityType id;
        public Rectangle hitBox;
        public eTeam Team;
        public EntityType hasCollidedWith = EntityType.Neutral;
        public abstract void Draw(SpriteBatch spritebatch);
        public abstract void Update(GameTime gt);
        public virtual bool isColliding(AGameEntity ge)
        {
            if (hitBox.Intersects(ge.hitBox)) {
                hasCollidedWith = ge.id;
                return true;
            }
            return false;
        }

    }
}