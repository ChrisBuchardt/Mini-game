using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LearnExtended
{
    public class AssetManager
    {
        //Simple assman cause there are very few assets so dynamic loading is not requiared
        public Texture2D BlueOrbTexture { get; protected set; }
        public Texture2D GreenOrbTexture { get; protected set; }
        public Texture2D RedOrbTexture { get; protected set; }
        public Texture2D PurpleOrbTexture { get; protected set; }
        public Texture2D YellowOrbTexture { get; protected set; }
        public Texture2D CyanOrbTexture { get; protected set; }

        //player
        public Texture2D PlayerSpriteTexture { get; protected set; }

        //BackGround
        public Texture2D BackGroundTileTexture { get; protected set; }
        public Texture2D BackGroundMagicTexture { get; protected set; }


        public ContentManager content { get; protected set; }
        public AssetManager(ContentManager content)
        {
            this.content = content;
        }

        public void LoadTextures()
        {
            BlueOrbTexture = content.Load<Texture2D>("Blending/blue");
            RedOrbTexture = content.Load<Texture2D>("Blending/red");
            GreenOrbTexture = content.Load<Texture2D>("Blending/green");
            PlayerSpriteTexture = content.Load<Texture2D>("SmileyWalk");
            BackGroundTileTexture = content.Load<Texture2D>("backgroundcircle");
            BackGroundMagicTexture = content.Load<Texture2D>("magicCircle");
            CyanOrbTexture = content.Load<Texture2D>("Blending/cyanOrb");
            PurpleOrbTexture = content.Load<Texture2D>("Blending/purpleOrb");
            YellowOrbTexture = content.Load<Texture2D>("Blending/yellowOrb");

        }





    }
}