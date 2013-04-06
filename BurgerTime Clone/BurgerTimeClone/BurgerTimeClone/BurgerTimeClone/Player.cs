using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BurgerTimeClone
{
    public static class Player
    {
        private static Texture2D player;
        public static Vector2 playerPosition;
        private static int playerX, playerY;
        private static int currentFrame, timePerFrame, timeSinceLastFrame;
        private static int tileWidth = 32;
        private static int tileHeight = 32;

        // Collision rectangles
        public static Rectangle playerRect;

        public static void Initialize(Texture2D texture2D)
        {
            // Start player at X = 200, Y = 300
            playerPosition = new Vector2(200, 240);
            player = texture2D;
            currentFrame = 0;
            timeSinceLastFrame = 0;
            timePerFrame = 100;
            playerRect = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 32, 32);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            playerX = (int)playerPosition.X;
            playerY = (int)playerPosition.Y;
            // Relevant arguments.
            // Draw(image name, position vector, destination rectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(player, playerPosition, new Rectangle(currentFrame * tileWidth, 0, tileWidth, tileHeight), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public static void Update(GameTime gameTime)
        {
            // Update collision rectangle
            playerRect = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, tileWidth, tileHeight);

            KeyboardState keyState = Keyboard.GetState();
            // If the player is on a platform, then player can move left and right.
            // NOTE: We need to add tileHeight to the playerPosition.Y to compensate for the fact that
            // the Y coordinate is calculated from the top of the sprite.
            if (TileMap.PlatformTileAtGivenPixelLocation(new Vector2(playerPosition.X + tileWidth, playerPosition.Y + tileHeight)))
            {
                if (keyState.IsKeyDown(Keys.Left))
                {
                    playerPosition.X -= 2;
                }
                else if (keyState.IsKeyDown(Keys.Right))
                {
                    playerPosition.X += 2;
                }
            }
            // If we are not on a platform, free fall until we drop TO a platform.
            else if (!TileMap.PlatformTileAtGivenPixelLocation(new Vector2(playerPosition.X + tileWidth, playerPosition.Y + tileHeight)))
            {
                playerPosition.Y += 2;
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
        }
    }
}
