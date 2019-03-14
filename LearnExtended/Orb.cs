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
        private bool hit = false;


        public Orb(Enemy.OrbType type, Vector2 position, Vector2 target, AssetManager asset)
        {
            Position = position;
            Target = target;
            Team = 3;

            switch (type){

                case Enemy.OrbType.Cyan:
                    Texture = asset.CyanOrbTexture;
                    id = "Cyan";
                    break;
                case Enemy.OrbType.Yellow:
                    Texture = asset.YellowOrbTexture;
                    id = "Yellow";
                    break;
                case Enemy.OrbType.Purple:
                    Texture = asset.PurpleOrbTexture;
                    id = "Purple";
                    break;
            }
            SpriteCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
            hitBox = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point((int)(Texture.Width*0.1f), (int)(Texture.Height * 0.1f)));

        }

        

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, Position, null, Color.White, 1f, SpriteCenter, 0.1f, SpriteEffects.None, 0f);

        }

        public override void Update(GameTime gt)
        {
            Vector2 move = Position - Target;
            move.Normalize();
            Position -= move;
            hitBox.Location = new Point((int)Position.X, (int)Position.Y);
        }
    }
}
