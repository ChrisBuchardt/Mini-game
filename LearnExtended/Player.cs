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

        public Player( Vector2 center, AssetManager asset)
        {
            Team = 1;
            Position = center;
            walkingTextureSheet = asset.PlayerSpriteTexture;
            hitBox = new Rectangle(new Point((int)Position.X,(int)Position.Y),new Point(walkingTextureSheet.Width/4, walkingTextureSheet.Height/4));
            animSprite = new AnimatedSprite(walkingTextureSheet, 4, 4);
        }

        public override void Update(GameTime gt)
        {
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
            throw new NotImplementedException();
        }

        
    }
}
