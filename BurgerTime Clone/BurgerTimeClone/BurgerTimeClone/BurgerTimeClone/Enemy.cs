using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BurgerTimeClone
{
    class Enemy
    {
        private static Texture2D enemy;
        private static int currentFrame = 0;
        private static int timePerFrame, timeSinceLastFrame;
        private static Vector2 enemyPosition;
        public static Rectangle enemyRect;
        private static int tileWidth = 32;
        private static int tileHeight = 32;

        // decide direction
        private static bool left = true;

        public static void Initialize(Texture2D texture2D)
        {
            enemy = texture2D;
            timePerFrame = 250;
            timeSinceLastFrame = 0;
            enemyPosition = new Vector2(300, 240);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            // Draws the enemy to screen.
            // Relevant arguments.
            // Draw(image name, position vector, destination rectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(enemy, enemyPosition, new Rectangle(currentFrame * tileWidth, 0, tileWidth, tileHeight), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public static void Update(GameTime gameTime)
        {
            enemyRect = new Rectangle((int)enemyPosition.X, (int)enemyPosition.Y, tileWidth, tileHeight);

            if (TileMap.PlatformTileAtGivenPixelLocation(new Vector2(enemyPosition.X + tileWidth, enemyPosition.Y + tileHeight)))
            {
                if (enemyPosition.Y == Player.playerPosition.Y)
                {
                    // If on same platform, and the player is to the left of the enemy, then go right.
                    if ((enemyPosition.X < Player.playerPosition.X) && (Math.Abs(enemyPosition.X - Player.playerPosition.X) <= 100))
                        left = false;

                    // If on same platform, and the player is to the right of the enemy, then go left.
                    if ((enemyPosition.X > Player.playerPosition.X) && (Math.Abs(Player.playerPosition.X - enemyPosition.X) <= 100))
                        left = true;
                }
                // Move the enemy to the left 1 pixel per update
                if (enemyPosition.X > 0 && (left == true))
                {
                    enemyPosition.X--;
                    if (enemyPosition.X == 0)
                        left = false;
                }
                else if (enemyPosition.X < 768 && (left == false))
                {
                    enemyPosition.X++;
                    if (enemyPosition.X == 768)
                        left = true;
                }
            }
            // If we are not on a platform, free fall until we drop TO a platform.
            else if (!TileMap.PlatformTileAtGivenPixelLocation(new Vector2(enemyPosition.X + tileWidth, enemyPosition.Y + tileHeight)))
            {
                enemyPosition.Y += 2;
            }

            // Changes the frame of the player
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > timePerFrame)
            {
                timeSinceLastFrame = 0;
                if (currentFrame < 2)
                    currentFrame++;
                else
                    currentFrame = 0;
            }

            // This allows the enemy frames to shift.
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > timePerFrame)
            {
                timeSinceLastFrame = 0;
                if (currentFrame < 1)
                    currentFrame++;
                else
                    currentFrame = 0;
            }
        }
    }
}
