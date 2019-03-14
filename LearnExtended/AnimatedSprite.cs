using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnExtended
{
    class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Colums { get; set; }
        private int currentFrame;
        private int totalFrames;
        public float Rotation = 0f;
        private Vector2 SpriteCenter; 



        public AnimatedSprite(Texture2D texture, int rows, int colums)
        {
            Texture = texture;
            Rows = rows;
            Colums = colums;
            currentFrame = 0;
            totalFrames = Rows * Colums;
            SpriteCenter = new Vector2((Texture.Width / Colums) / 2, (Texture.Height / Rows)/2);
        }

        public void Update()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Colums;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Colums);
            int column = currentFrame % Colums;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
           // sp.Draw(Texture, Center + Position, null, Color.White, 1f, SpriteCenter, 0.5f, SpriteEffects.None, 0f);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, Rotation, SpriteCenter, SpriteEffects.None, 0f);
        }
    }

}
