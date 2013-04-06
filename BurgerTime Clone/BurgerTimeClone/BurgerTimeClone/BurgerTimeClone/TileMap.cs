using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BurgerTimeClone
{
    public static class TileMap
    {
        public const int TileWidth = 32;
        public const int TileHeight = 32;
        public const int MapWidth = 25;
        public const int MapHeight = 20;

        static private Texture2D texture;
        // Keep track of which kind of tile we are using...                           
        static private List<Rectangle> tiles = new List<Rectangle>();

        // The map grid...
        static private int[,] mapSquares;

        static public void Initialize(Texture2D tileTexture)
        {
            texture = tileTexture;
            tiles.Clear();
            // Ladder tile (Index 0)
            tiles.Add(new Rectangle(0, 0, TileWidth, TileHeight));
            // Platform tile (Index 1)
            tiles.Add(new Rectangle(32, 0, TileWidth, TileHeight));
            // Air tile (Index 2)
            tiles.Add(new Rectangle(64, 0, TileWidth, TileHeight));
        }

        static public Vector2 GetSquareAtPixel(Vector2 pixel)
        {
            return new Vector2((int)pixel.X / TileWidth, (int)pixel.Y / TileHeight);
        }

        // Retrieves the square center by taking 32 * the square X and Y coordinates and adding 16, which is the center of
        // a tile, as the tile Width/Height is 32.
        static public Vector2 GetSquareCenter(Vector2 square)
        {
            int squareX, squareY;
            squareX = (int)square.X;
            squareY = (int)square.Y;
            return new Vector2(squareX * TileWidth + 16, squareY * TileHeight + 16);
        }

        // Retrieves a square from the world itself.
        static public Rectangle SquareWorldRectangle(int x, int y)
        {
            return new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
        }

        static public Rectangle SquareWorldRectangle(Vector2 square)
        {
            return SquareWorldRectangle((int)square.X, (int)square.Y);
        }

        static public Rectangle SquareScreenRectangle(int x, int y)
        {
            return SquareWorldRectangle(x, y);
        }

        static public int GetTileAtSquare(int tileX, int tileY)
        {
            if ((tileX >= 0) && (tileX < MapWidth) && (tileY >= 0) && (tileY < MapHeight))
                return mapSquares[tileX, tileY];
            else
                return -1;
        }

        static public void SetTileAtSquare(int tileX, int tileY, int tile)
        {
            if ((tileX >= 0) && (tileX < MapWidth) && (tileY >= 0) && (tileY < MapHeight))
                mapSquares[tileX, tileY] = tile;
        }

        static public int GetTileAtPixel(Vector2 pixel)
        {
            return GetTileAtSquare((int)pixel.X / TileWidth, (int)pixel.Y / TileHeight);
        }

        static public bool PlatformTileAtGivenLocation(Vector2 square)
        {
            int tileIndex = GetTileAtSquare((int)square.X, (int)square.Y);
            if (tileIndex == 1)
                return true;
            else
                return false;
        }

        static public bool PlatformTileAtGivenPixelLocation(Vector2 pixel)
        {
            if (PlatformTileAtGivenLocation(new Vector2((int)pixel.X / TileWidth, (int)pixel.Y / TileHeight)) == true)
                return true;
            else
                return false;
        }

        // NEW CODE:
        static public bool LadderTileAtGivenLocation(Vector2 square)
        {
            int tileIndex = GetTileAtSquare((int)square.X, (int)square.Y);
            if (tileIndex == 0)
                return true;
            else
                return false;
        }

        static public bool LadderTileAtGivenPixelLocation(Vector2 pixel)
        {
            if (LadderTileAtGivenLocation(new Vector2((int)pixel.X / TileWidth, (int)pixel.Y / TileHeight)) == true)
                return true;
            else
                return false;
        }


        // Draw the map...
        static public void GenerateMap()
        {
            mapSquares = new int[,] {
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 1, 0, 0, 0, 1, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }
            };
        }

        // Draw loop...
        static public void Draw(SpriteBatch spriteBatch)
        {
            int startX = 0 / TileWidth;
            int endX = 800 / TileWidth;

            int startY = 0 / TileHeight;
            int endY = 640 / TileHeight;

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if ((x >= 0) && (y >= 0) && (x < MapWidth) && (y < MapHeight))
                        spriteBatch.Draw(texture, SquareScreenRectangle(x, y), tiles[GetTileAtSquare(x, y)], Color.White);
                }
            }
        }
    }
}
