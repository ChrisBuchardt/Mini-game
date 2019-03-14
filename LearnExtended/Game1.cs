using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Threading;
using static LearnExtended.AGameEntity;
using static LearnExtended.Enemy;

namespace LearnExtended
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private List<AGameEntity> gameEntities;
        private List<Orb> orbs = new List<Orb>();
        private List<AGameEntity> removeFromWorld = new List<AGameEntity>();

        private Vector2 ScreenCenter;
        private Player playerOne;
        private int ScreenWidth = 750;
        private int ScreenHeight = 750;
        private SpriteFont font;
        private bool gameOver = false;
        public static AssetManager asset;
        #endregion

        #region Initialize
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
            font = Content.Load<SpriteFont>("gamefont");
            asset.LoadTextures();

            base.Initialize();
        }
        #endregion

        #region Load and unload
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Creats the colors ment for blend testing
            float dist = (ScreenHeight / 2)-15;

            playerOne = new Player(ScreenCenter, asset);
            gameEntities.Add(new Enemy(EntityType.Blue, 0, 0.025f, ScreenCenter, dist,playerOne, asset));
            gameEntities.Add(new Enemy(EntityType.Red, 0, -0.022f, ScreenCenter, dist, playerOne, asset));
            gameEntities.Add(new Enemy(EntityType.Green, 0, 0.017f, ScreenCenter, dist, playerOne, asset));

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
            gameEntities.Clear();
            playerOne = null;
            // TODO: Unload any non ContentManager content here
        }

        private void Reload()
        {
            UnloadContent();
            LoadContent();


        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            handleInput(gameTime);
            gameOver = (playerOne == null || !playerOne.Alive);
            Console.WriteLine(gameOver +  " " + playerOne.Alive);

            if (!gameOver)
            {
                UpdateWorld(gameTime);
            }
           

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public void UpdateWorld(GameTime gameTime)
        {
            foreach (var ge in gameEntities)
            {
                foreach (var ga in gameEntities)
                {
                    //checks teams during collision
                    //team 1 is the player, team 2 is the enemy, team 3 is the enemy projectiles
                    //2 && 2, 3 && 1
                    if (ga != ge)
                    {

                        //Checks if both entities which have collided are both enemies
                        if (ge.Team == eTeam.Enemy && ga.Team == eTeam.Enemy)
                        {
                            if (ge.isColliding(ga))
                            {
                                enemyXenemyCollision(ge, ga);
                            }

                        }
                        //checks if a projectile has collided with a player
                        if (ge.Team == eTeam.Player && ga.Team == eTeam.Projectile || ge.Team == eTeam.Projectile && ga.Team == eTeam.Player)
                        {
                            if (ge.isColliding(ga))
                            {

                                if (ge.Team == eTeam.Player)
                                {
                                    //assuming that the player is ge
                                    playerXprojectile(ge, ga);

                                }
                                else playerXprojectile(ga, ge);

                            }
                        }

                    }
                }

                ge.Update(gameTime);
            }
            foreach (var ded in removeFromWorld)
            {
                if (gameEntities.Contains(ded))
                {
                    gameEntities.Remove(ded);
                }
            }
            removeFromWorld.Clear();
            gameEntities.AddRange(orbs);
            orbs.Clear();

        }

        #endregion

        #region DrawFunctions

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {


            DrawBackground(spriteBatch);
            DrawWithBlend(spriteBatch);
            DrawWithoutBlend(spriteBatch);



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        private void DrawWithoutBlend(SpriteBatch spriteBatch)
        {
            //No blendState meaning that this will be drawn over the previously drawn textures.
            spriteBatch.Begin(SpriteSortMode.Deferred, null);

            if (!gameOver && playerOne.Alive)
            {
                playerOne.Draw(spriteBatch);
            }

           

            spriteBatch.DrawString(font, "Score: " + playerOne.Score, new Vector2(50, 50), Color.White);
            spriteBatch.End();
        }

        private void DrawWithBlend(SpriteBatch spriteBatch)
        {
            //Drawn with blendstate meaning that the red, green and blue lights will blend over the each other and sprites

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);


            foreach (var gameEntity in gameEntities)
            {
                if (gameEntity.Team != eTeam.Player)
                {
                    gameEntity.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null);
            spriteBatch.Draw(asset.BackGroundTileTexture, GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.Draw(asset.BackGroundMagicTexture, GraphicsDevice.Viewport.Bounds, Color.White);

            spriteBatch.End();
        }
        #endregion

        #region Controls
        private void handleInput(GameTime gt)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape))
                Exit();
            
            if (ks.IsKeyDown(Keys.Space))
            {
                if (gameOver) Reload();
                else playerOne.Deflect(gt);
            }
            else if (!gameOver)
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

        private void Restart()
        {
        }

        #endregion

        #region CollisionFunctions

        public void playerXprojectile(AGameEntity ge, AGameEntity ga)
        {
            Player player = (Player)ge;
            Orb orb = (Orb)ga;

            player.Hit(orb.Damage);
            removeFromWorld.Add(orb);
            if (player.Health < 0)
            {
                removeFromWorld.Add(player);
            }

        }


        public void enemyXenemyCollision(AGameEntity ge, AGameEntity ga)
    {
        if (ge.hasCollidedWith != EntityType.Neutral)
        {
            Enemy enemy = (Enemy)ge; 

            switch (ge.id)
            {
                case EntityType.Blue:
                        if (ge.hasCollidedWith == EntityType.Red)
                    {
                        enemy.Shoot(EntityType.Purple, orbs);
                        ge.hasCollidedWith = EntityType.Neutral;
                        ga.hasCollidedWith = EntityType.Neutral;
                        break;
                    }
                    else if (ge.hasCollidedWith == EntityType.Green)
                    {

                        enemy.Shoot(EntityType.Cyan, orbs);
                        ge.hasCollidedWith = EntityType.Neutral;
                        ga.hasCollidedWith = EntityType.Neutral;
                        break;
                    }
                    break;

                case EntityType.Green:
                        if (ge.hasCollidedWith == EntityType.Red)
                    {
                        enemy.Shoot(EntityType.Yellow, orbs);
                        ge.hasCollidedWith = EntityType.Neutral;
                        ga.hasCollidedWith = EntityType.Neutral;
                        break;
                    }
                    else if (ge.hasCollidedWith == EntityType.Blue)
                    {
                        enemy.Shoot(EntityType.Cyan,orbs);
                        ge.hasCollidedWith = EntityType.Neutral;
                        ga.hasCollidedWith = EntityType.Neutral;
                        break;
                    }
                    break;

                case EntityType.Red:
                    if (ge.hasCollidedWith == EntityType.Blue)
                    {
                        enemy.Shoot(EntityType.Purple,orbs);
                        ge.hasCollidedWith = EntityType.Neutral;
                        ga.hasCollidedWith = EntityType.Neutral;
                        break;
                    }
                    else if (ge.hasCollidedWith == EntityType.Green)
                    {
                        enemy.Shoot(EntityType.Yellow,orbs);
                        ge.hasCollidedWith = EntityType.Neutral;
                        ga.hasCollidedWith = EntityType.Neutral;
                        break;
                    }
                    break;

                default:
                    return;
            }



        }
    }
        #endregion
    }

}
