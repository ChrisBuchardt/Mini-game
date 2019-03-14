using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Threading;
using static LearnExtended.Enemy;

namespace LearnExtended
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private List<AGameEntity> gameEntities;
        private List<Orb> orbs = new List<Orb>();

        private Vector2 ScreenCenter;
        private Player playerOne;
        private int ScreenWidth = 750;
        private int ScreenHeight = 750;
        private SpriteFont font;
        public static AssetManager asset; 

        public Game1()
        {
            Window.AllowUserResizing = false;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameEntities = new List<AGameEntity>();
            ScreenCenter = new Vector2(ScreenWidth / 2, ScreenHeight / 2);
            asset = new AssetManager(Content);
            base.Initialize();
        }

        ///<summary>
        ///Simple Highscore game, a timer will tick up giving the player higher a timed score
        ///The player moves a character with simple movement, 
        ///Three colors rotate arround the character, when they collide depending on thier color they shoot at the player..

        ///The higher score the faster the colors rotate. 
        ///Use blending for the colors cause it looks cool
        ///Use Extended for particles for the shots
        ///Use Extended for the player animations
        ///If possible use lights for the well lights
            /// </summary>


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Creats the colors ment for blend testing
            asset.LoadTextures();
            float dist = (ScreenHeight / 2)-15;

            playerOne = new Player(ScreenCenter, asset);
            gameEntities.Add(new Enemy(OrbType.Blue, 0, 0.025f, ScreenCenter, dist,playerOne, asset));
            gameEntities.Add(new Enemy(OrbType.Red, 0, -0.022f, ScreenCenter, dist, playerOne, asset));
            gameEntities.Add(new Enemy(OrbType.Green, 0, 0.017f, ScreenCenter, dist, playerOne, asset));

            //Creates a font for the score
            font = Content.Load<SpriteFont>("gamefont");
            //Creates a player, atm only an animated sprite
            gameEntities.Add(playerOne);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            handleInput(gameTime);
            
            //Dette er noget kæmpe rod
            foreach (var ge in gameEntities)
            {
                foreach (var ga in gameEntities)
                {
                    //checks teams during collision
                    //team 1 is the player, team 2 is the enemy, team 3 is the enemy projectiles
                    //2 && 2, 3 && 1
                    if (ga != ge )
                    {
                        if (ge.isColliding(ga))
                        {

                            //Checks if both entities which have collided are both enemies
                            if (ge.Team == 2 && ga.Team == 2)
                            {
                                enemyXenemyCollision(ge, ga);
                            }




                        }
                        

                    }
                }

                ge.Update(gameTime);
            }
            gameEntities.AddRange(orbs);
            orbs.Clear;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void handleInput(GameTime gt)
        {
           KeyboardState ks = Keyboard.GetState();
            if ( ks.IsKeyDown(Keys.Escape))
                Exit();

            if (ks.IsKeyDown(Keys.Space))
            {
                playerOne.Deflect(gt);
            }
            else
            {
                if (ks.IsKeyDown(Keys.Left))
                {
                    //Rotate left
                    playerOne.RotateLeft(gt);
                }

                if (ks.IsKeyDown(Keys.Right))
                {
                    //Rotate right
                    playerOne.RotateRight(gt);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred,null);
            spriteBatch.Draw(asset.BackGroundTileTexture, GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.Draw(asset.BackGroundMagicTexture, GraphicsDevice.Viewport.Bounds, Color.White);

            spriteBatch.End();

            //Drawn with blendstate meaning that the red, green and blue lights will blend over the each other and sprites
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);


            foreach (var gameEntity in gameEntities)
            {
                gameEntity.Draw(spriteBatch);
            }

            spriteBatch.End();

            //No blendState meaning that this will be drawn over the previously drawn textures.
            spriteBatch.Begin(SpriteSortMode.Deferred, null);
            spriteBatch.DrawString(font, "Score: " + playerOne.Score, new Vector2(50, 50), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    public void playerXprojectile(AGameEntity ge, AGameEntity ga)
        {

        }
    public void enemyXenemyCollision(AGameEntity ge, AGameEntity ga)
    {
        if (ge.hasCollidedWith != "")
        {
            Enemy enemy = (Enemy)ge; 
            System.Console.WriteLine(ge.id + " has collided with " + ge.hasCollidedWith);

            switch (ge.id)
            {
                case "Blue":
                        if (ge.hasCollidedWith == "Red")
                    {
                        enemy.Shoot(OrbType.Purple, orbs);
                        ge.hasCollidedWith = "";
                        ga.hasCollidedWith = "";
                        break;
                    }
                    else if (ge.hasCollidedWith == "Green")
                    {

                        enemy.Shoot(OrbType.Cyan, orbs);
                        ge.hasCollidedWith = "";
                        ga.hasCollidedWith = "";
                        break;
                    }
                    break;

                case "Green":
                        if (ge.hasCollidedWith == "Red")
                    {
                        enemy.Shoot(OrbType.Yellow, orbs);
                        ge.hasCollidedWith = "";
                        ga.hasCollidedWith = "";
                        break;
                    }
                    else if (ge.hasCollidedWith == "Blue")
                    {
                        enemy.Shoot(OrbType.Cyan,orbs);
                        ge.hasCollidedWith = "";
                        ga.hasCollidedWith = "";
                        break;
                    }
                    break;

                case "Red":
                    if (ge.hasCollidedWith == "Blue")
                    {
                        enemy.Shoot(OrbType.Purple,orbs);
                        ge.hasCollidedWith = "";
                        ga.hasCollidedWith = "";
                        break;
                    }
                    else if (ge.hasCollidedWith == "Green")
                    {
                        enemy.Shoot(OrbType.Yellow,orbs);
                        ge.hasCollidedWith = "";
                        ga.hasCollidedWith = "";
                        break;
                    }
                    break;

                default:
                    return;
            }



        }
    }
    }

}
