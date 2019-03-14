using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LearnExtended
{
    class Orb : AGameEntity
    {
        public Texture2D Texture;
        public Vector2 Target;
        private Vector2 SpriteCenter;
        public int Damage;
        private bool hit = false;
        private float Scale = 0.05f;
        private float Rotation;
        private float RotationSpeed;

        public Orb(Enemy.EntityType type, Vector2 position, Vector2 target, AssetManager asset)
        {
            Position = position+target;
            Target = target;
            Team = eTeam.Projectile;

            switch (type){

                case Enemy.EntityType.Cyan:
                    Texture = asset.CyanOrbTexture;
                    Damage = 10;
                    RotationSpeed = 0.01f;
                    id = EntityType.Cyan;
                    break;
                case Enemy.EntityType.Yellow:
                    Damage = 15;
                    Texture = asset.YellowOrbTexture;
                    RotationSpeed = 0.1f;
                    id = EntityType.Yellow;
                    break;
                case Enemy.EntityType.Purple:
                    Texture = asset.PurpleOrbTexture;
                    RotationSpeed = 0.15f;
                    Damage = 13;
                    id = EntityType.Purple;
                    break;
            }
            SpriteCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
            hitBox = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point((int)(Texture.Width* Scale), (int)(Texture.Height * Scale)));

        }

        

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, Position, null, Color.White, Rotation, SpriteCenter,Scale, SpriteEffects.None, 0f);

        }

        public override void Update(GameTime gt)
        {
            Vector2 move = Position - Target;
            move.Normalize();
            Rotation += RotationSpeed;
            Position -= move;
            hitBox.Location = new Point((int)Position.X, (int)Position.Y);
        }
    }
}
