using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNAGifAnimation;

namespace Castlevania_Clone
{

    // TODO: Add ADT for candles. Needs to have visibility property, draw property, and store the image loaded.

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    ///
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public class Candle
        {
            public GifAnimation image;
            public bool visible = true;
            public bool draw = true;
            public Rectangle location;
            public int id = 0;
            public Candle(GifAnimation inImage, int locX, int inID)
            {
                image = inImage;
                location.X = locX;
                location.Y = 390;
                location.Width = 32;
                location.Height = 64;
                id = inID;
            }
        }

        SpriteFont gameFont;

        int viewPosX = -1300, viewPosY = 0;
        int timePerFrame = 80;
        int timeSinceLastFrame = 0;

        Rectangle viewportRect;
        int viewportX = 0;
        int viewportY = 0;

        Vector2 trevorPosition = new Vector2(0, 370);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // An image
        Texture2D background;
        Texture2D background2;
        Texture2D trevor;
        Texture2D dagger;
        Texture2D skeleton;

        GifAnimation medusa;
        GifAnimation lgCandle;
        List<Candle> candles = new List<Candle>();

        Vector2 medusaPosition = new Vector2(0, 200);
        Vector2 medusaSpeed = new Vector2(75, 150);
        Vector2 skeletonPosition = new Vector2(300, 395);

        bool jumping = false;
        bool weapon = false;
        bool[] candleVisible = new bool[8];
        bool left = true;
        bool skeletonVisible = true;
        bool medusaVisible = true;
        bool level1 = false;
        bool level2 = true;

        int currentFrameX = 0;
        int currentFrameY = 0;
        int enemyCurrentFrameX = 0;
        int enemyCurrentFrameY = 0;
        Candle x;

        int[] candleLocX = { 100, 300, 450, 600, 800, 1000, 1400, 1700 };
        int num_candles = 8;

        int position;
        Point frameSize = new Point(40, 62);

        #region Bounding Boxes
        Rectangle daggerBound = new Rectangle(0, 0, 32, 18);
        Rectangle[] candleBound = new Rectangle[4];
        #endregion

        #region Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            Content.RootDirectory = "Content";
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Generate 10 candles.
            viewportRect = new Rectangle(viewportX, viewportY, 640, 480);
            for (int i = 0; i < num_candles; i++)
            {
                candleVisible[i] = true;
            }
            // TODO: Add your initialization logic here
            base.Initialize();
        }
        #endregion

        #region LoadContent
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        protected override void LoadContent()
        {
            gameFont = Content.Load<SpriteFont>(@"Fonts\GameFont");
            x = new Candle(lgCandle, 0, 0);
            Camera.WorldRectangle = new Rectangle(0, 0, 2291, 480);
            Camera.ViewPortWidth = 1024;
            Camera.ViewPortHeight = 480;

            // Load the images
            background = Content.Load<Texture2D>(@"Images\Castlevaniast1");
            background2 = Content.Load<Texture2D>(@"Images\stage2");
            lgCandle = Content.Load<GifAnimation>(@"Images\LargeCandleGif");
            // Add music to game.
            Song song = Content.Load<Song>(@"Music\music");  // Put the name of your song in instead of "song_title"
            MediaPlayer.Play(song);

            for (int i = 0; i < num_candles; i++)
            {
                candles.Add(new Candle(lgCandle, candleLocX[i], i+1));
            }

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Medusa
            medusa = Content.Load<GifAnimation>(@"Images\MedusaR");

            // Player
            trevor = Content.Load<Texture2D>(@"Images\trevorwalk");
            
            // Weapon(s)
            dagger = Content.Load<Texture2D>(@"Images\daggerR");
            
            // Skeleton
            skeleton = Content.Load <Texture2D>(@"Images\skeletons");
        }
        #endregion

        #region UnloadContent
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
        #endregion

        public void handleBounds(GameTime gameTime)
        {
            Rectangle trevorRect =
                new Rectangle((int)trevorPosition.X, (int)trevorPosition.Y,
                trevor.Width / 3, trevor.Height / 2);
            
            Rectangle daggerRect =
                new Rectangle((int)position, (int)trevorPosition.Y, 32, 18);
            
            Rectangle[] candleRect = { new Rectangle(100, 390, 32, 64), new Rectangle(300, 390, 32, 64), new Rectangle(450, 390, 32, 64), 
                                         new Rectangle(600, 390, 32, 64), new Rectangle(800, 390, 32, 64), new Rectangle(1000, 390, 32, 64),
                                     new Rectangle(1400, 390, 32, 64), new Rectangle(1700, 390, 32, 64)};
            
            Rectangle skeletonRect = new Rectangle((int)skeletonPosition.X, (int)skeletonPosition.Y, 36, 72);
            
            Rectangle medusaRect = new Rectangle((int)medusaPosition.X, (int)medusaPosition.Y, 32, 32);

            // Check dagger/candle interactions
            foreach (Candle c in candles)
                if (daggerRect.Intersects(c.location))
                {
                    //candles.Remove(c);
                    c.visible = false;
                    x = c;
                    weapon = false;
                    position = -10;
                }
            if (x.id > 0)
                candles.Remove(x);

            // Check skeleton/dagger interactions
            if (daggerRect.Intersects(skeletonRect))
            {
                skeletonVisible = false;
            }

            // Check skeleton/Trevor interactions
            if (skeletonRect.Intersects(trevorRect))
            {
                // WORKS
                //skeletonVisible = false;
            }

            // Check dagger/Medusa interactions
            if (daggerRect.Intersects(medusaRect))
            {
                medusaVisible = false;
            }

            // Check Trevor/Medusa interactions
            if (trevorRect.Intersects(medusaRect))
            {
                // WORKS
                //medusaVisible = false;
            }
            if (viewPosX <= -1460 && trevorPosition.X >= 450)
                level2 = true;
        }

        #region Update
        protected override void Update(GameTime gameTime)
        {
            handleBounds(gameTime);

            daggerBound.X = position;
            
            lgCandle.Update(gameTime.ElapsedGameTime.Ticks);
            // Bounding rectangles...
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            #region Skeleton handling
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > (timePerFrame * 4))
            {
                timeSinceLastFrame -= (timePerFrame * 4);
                if (enemyCurrentFrameX < 36)
                    enemyCurrentFrameX += 36;
                else
                    enemyCurrentFrameX = 0;
            }
            if (skeletonPosition.X == 0)
            {
                left = false;
                skeletonPosition.X += 1;
            }
            else if (skeletonPosition.X > 0 && left)
            {
                skeletonPosition.X -= 1;
                enemyCurrentFrameY = 0;
            }
            else if (skeletonPosition.X < 200)
            {
                skeletonPosition.X += 1;
                enemyCurrentFrameY = 64;
            }
            else if (skeletonPosition.X == 200)
            {
                left = true;
                skeletonPosition.X -= 1;
            }
            #endregion

            #region Player handling
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
            {
                if (trevorPosition.X < 605)
                    trevorPosition.X += 1;
                if (viewPosX > -1651 && trevorPosition.X > 320)
                {
                    viewPosX -= 1;
                    foreach (Candle c in candles)
                        c.location.X -= 1;
                }

                currentFrameY = 62;
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > (timePerFrame / 3))
                {
                    timeSinceLastFrame -= (timePerFrame * 3);
                    if (currentFrameX < 80)
                        currentFrameX += 40;
                    else
                        currentFrameX = 0;
                }
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                if (trevorPosition.X > 0)
                    trevorPosition.X -= 1;
                if ((viewPosX < 0) && trevorPosition.X <= 320)
                {
                    viewPosX += 1;
                    foreach (Candle c in candles)
                        c.location.X += 1;
                }

                currentFrameY = 0;
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > (timePerFrame / 3))
                {
                    timeSinceLastFrame -= (timePerFrame * 3);
                    if (currentFrameX < 80)
                        currentFrameX += 40;
                    else
                        currentFrameX = 0;
                }
            }
            #endregion

            #region Handle jumping...
            if (keyState.IsKeyDown(Keys.Space) && !jumping)
            {
                jumping = true;
                trevorPosition.Y -= 100;
            }
            else if (keyState.IsKeyUp(Keys.Space))
            {
                if (viewPosX < -807 && viewPosX > -900)
                { jumping = false; }
                
                else if (viewPosX < -900 && viewPosX > -1000)
                {
                    if (trevorPosition.Y < 224)
                        trevorPosition.Y += 1;
                    jumping = false;
                }
                else if (trevorPosition.Y < 400)
                    trevorPosition.Y += 4;
                else if (trevorPosition.Y >= 400)
                    jumping = false;
            }
            #endregion

            #region Weapon handling
            if (keyState.IsKeyDown(Keys.D))
            {
                if (!weapon)
                {
                    position = (int)trevorPosition.X;
                    weapon = true;
                }
            }

            if (weapon)
            {
                fireWeapon();
            }
            #endregion

            // Move the Medusa sprite by speed, scaled by elapsed time
            medusaPosition.X += medusaSpeed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            medusaPosition.Y = medusaPosition.Y + (-(float)Math.Acos(medusaPosition.X / 25) * 10);

            base.Update(gameTime);
        }
        #endregion

        #region Weapon Handling Function
        public void fireWeapon()
        {
            if (position < Camera.ViewPortWidth)
            {
                position += 10;
            }
            else if (position >= Camera.ViewPortWidth)
            {
                weapon = false;
                position = -10000;
            }
        }
        #endregion Weapon Handling

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            // Background

            if (!level2)
                spriteBatch.Draw(background, new Rectangle(viewPosX, viewPosY, 2291, 480), Color.White);
            if (level2)
            {
                if (level1)
                {
                    viewPosX = 0;
                    trevorPosition.X = 0;
                    trevorPosition.Y = 370;
                    level1 = false;
                }
                spriteBatch.Draw(background2, new Rectangle(viewPosX, viewPosY, 1963, 480), Color.White);
            }
            spriteBatch.DrawString(gameFont, "X: " + trevorPosition.X + "Y: " + trevorPosition.Y, new Vector2(20, 20), Color.OrangeRed);
            spriteBatch.DrawString(gameFont, "vX: " + viewPosX + " vY: " + viewPosY, new Vector2(20, 50), Color.OrangeRed);

            // Medusa
            if (medusaVisible)
                spriteBatch.Draw(medusa.GetTexture(), medusaPosition, Color.White);
            
            // Player
            spriteBatch.Draw(trevor, trevorPosition, new Rectangle(currentFrameX, currentFrameY, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            
            // Weapon
            if (weapon)
                spriteBatch.Draw(dagger, new Vector2(position, trevorPosition.Y), new Rectangle(0, 0, frameSize.X, 25), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            
            // Candle(s)
            foreach (Candle c in candles)
            {
                if (c.visible)
                    spriteBatch.Draw(c.image.GetTexture(), c.location, Color.White);
            }

            // Skeleton
            if (skeletonVisible)
                spriteBatch.Draw(skeleton, skeletonPosition, new Rectangle(enemyCurrentFrameX, enemyCurrentFrameY, 36, 64), Color.White);            
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
