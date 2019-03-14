using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LearnExtended
{
    internal class Enemy : AGameEntity
    {
        private Texture2D Texture;
        private float Angle;
        private float Speed;
        private float Distance;
        private float Scale = 0.5f;
        private Vector2 SpriteCenter;
        public Vector2 Center { get; private set; }
        private AssetManager asset;
        private Player Target;
        private bool canShoot = true;
        private int coolDown=0;



        public Enemy(EntityType type, float ang, float speed, Vector2 center, float Dist, Player target, AssetManager asset)
        {
            //Team 2 is enemies
            Team = eTeam.Enemy;
            switch (type)
            {
                case EntityType.Blue:
                    Texture = asset.BlueOrbTexture;
                    id = type;
                    break;
                case EntityType.Red:
                    Texture = asset.RedOrbTexture;
                    id = type;
                    break;
                case EntityType.Green:
                    Texture = asset.GreenOrbTexture;
                    id = type;
                    break;
            }
            this.asset = asset;
            Angle = ang;
            Speed = speed;
            Distance = Dist;
            Center = center;
            Target = target;
            hitBox = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point((int)(Texture.Width*Scale), (int)(Texture.Height * Scale)));

            SpriteCenter = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }
        public override void Update(GameTime gt)
        {

            coolDown++;
            if (!canShoot)
            {
                if (coolDown > 100)
                {
                    canShoot = true;
                    coolDown = 0;
                }
            }

            Angle += Speed;
            Position = new Vector2(
                (float)Math.Cos(Angle) * Distance,
                (float)Math.Sin(Angle) * Distance);
            hitBox.Location = new Point((int)Position.X, (int)Position.Y);

            //Each Enemy Knows its own shots
            
        }

        public void Shoot(EntityType orb, List<Orb> list)
        {
            if (canShoot)
            {
                list.Add(new Orb(orb, this.Position, this.Target.Position, asset));
                canShoot = false;
            }
        }

        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(Texture, Center + Position,null, Color.White, 1f, SpriteCenter,Scale,  SpriteEffects.None,0f);
        }

    }
}