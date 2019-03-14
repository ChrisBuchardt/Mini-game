using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnExtended
{
    class Player : AGameEntity
    {
        private AnimatedSprite animSprite;
        public int Score = 0;
        private Texture2D walkingTextureSheet;
        public int Health;
        public bool Alive;

        public Player( Vector2 center, AssetManager asset)
        {
            Team = eTeam.Player;
            Position = center;
            walkingTextureSheet = asset.PlayerSpriteTexture;
            hitBox = new Rectangle(new Point((int)Position.X,(int)Position.Y),new Point(walkingTextureSheet.Width/4, walkingTextureSheet.Height/4));
            animSprite = new AnimatedSprite(walkingTextureSheet, 4, 4);
            Health = 100;
        }

        public override void Update(GameTime gt)
        {
            Alive = (Health <= 0);

            animSprite.Update();
                
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animSprite.Draw(spriteBatch, Position);

        }

        internal void RotateLeft(GameTime gt)
        {
            animSprite.Rotation -= 0.15f;
        }

        internal void RotateRight(GameTime gt)
        {
            animSprite.Rotation += 0.15f;

        }

        internal void Deflect(GameTime gt)
        {
            //Deflects the magic, if succesfull add points to score.
        }

        internal void Hit(int damage)
        {
            Health -= damage;
        }
        
    }
}
