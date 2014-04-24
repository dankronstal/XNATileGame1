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

namespace XNATileGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tank;
        Texture2D tile_black;
        Texture2D tile_blue;
        Texture2D tile_red;

        int scale;
        int[,] board;
        int activePlayer;
        int[,] p1pos;
        int p1sel;
        int[] p1moves;
        int[,] p2pos;
        int p2sel;
        int[] p2moves;
        int turn;
        Dictionary<int,Dictionary<int,Keys>> moves;
        int moveCount;

        bool newTurn;

        SpriteFont Font1;
        Vector2 FontPos;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            scale = 25;
            turn = 0;
            p1sel = 0;
            p2sel = 0;
            newTurn = true;
            moves = new Dictionary<int, Dictionary<int, Keys>>();
            moveCount = 0;

            #region init board
            board = new int [32,20];
            #endregion

            #region init starting positions
            p1pos = new int[,] { { 18, 18 }, { 19, 18 } };
            p2pos = new int[,] { { 18, 0 }, { 19, 0 } };
            #endregion

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tank = Content.Load<Texture2D>("tank");
            tile_black = Content.Load<Texture2D>("tile_black");
            tile_blue = Content.Load<Texture2D>("tile_blue");
            tile_red = Content.Load<Texture2D>("tile_red");

            // Create a new SpriteBatch, which can be used to draw textures.
            Font1 = Content.Load<SpriteFont>("InGame");

        }

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
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState keyState = Keyboard.GetState();

            // TODO: Add your update logic here
            if (keyState.IsKeyDown(Keys.Escape))
                this.Exit();

            #region manage turn
            if (newTurn)
            {
                newTurn = false;
                turn++;
                activePlayer = 1;

                p1moves = new int[p1pos.GetUpperBound(0) + 1];
                for (int i = p1pos.GetUpperBound(0); i >= 0; i--)
                {
                    p1moves[i] = 0;
                }

                p2moves = new int[p2pos.GetUpperBound(0) + 1];
                for (int i = p2pos.GetUpperBound(0); i >= 0; i--)
                {
                    p2moves[i] = 0;
                }
            }
            #endregion

            #region manage movement
            switch (activePlayer)
            {
                case 1:
                    MovePlayer(keyState, p1moves, p1sel, p1pos, moves);
                    break;
                case 2:
                    MovePlayer(keyState, p2moves, p2sel, p2pos, moves);
                    break;
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                if (moveCount < moves.Count)
                {
                    moveCount++;
                    if (activePlayer == 1)
                    {
                        p1sel++;
                        if (p1sel > p1pos.GetUpperBound(0))
                        {
                            p1sel = 0;
                            activePlayer = 2;
                        }
                    }
                    else
                    {
                        p2sel++;
                        if (p2sel > p2pos.GetUpperBound(0))
                        {
                            p2sel = 0;
                            newTurn = true;
                        }
                    }
                }
            }
            #endregion

            base.Update(gameTime);
        }

        private void MovePlayer(KeyboardState keyState, int[] pMoves, int pSel, int[,] pPos, Dictionary<int, Dictionary<int, Keys>> moves)
        {
            if (pMoves[pSel] == 0)
            {
                if (keyState.IsKeyDown(Keys.Left) && pPos[pSel, 0] > 0)
                {
                    pPos[pSel, 0]--;
                    pMoves[pSel]++;
                    moves.Add(moves.Count, new Dictionary<int, Keys>() { { pSel, Keys.Left } });
                }
                if (keyState.IsKeyDown(Keys.Right) && pPos[pSel, 0] < 31)
                {
                    pPos[pSel, 0]++;
                    pMoves[pSel]++;
                    moves.Add(moves.Count, new Dictionary<int, Keys>() { { pSel, Keys.Right } });
                }
                if (keyState.IsKeyDown(Keys.Up) && pPos[pSel, 1] > 0)
                {
                    pPos[pSel, 1]--;
                    pMoves[pSel]++;
                    moves.Add(moves.Count, new Dictionary<int, Keys>() { { pSel, Keys.Up } });
                }
                if (keyState.IsKeyDown(Keys.Down) && pPos[pSel, 1] < 18)
                {
                    pPos[pSel, 1]++;
                    pMoves[pSel]++;
                    moves.Add(moves.Count, new Dictionary<int, Keys>() { { pSel, Keys.Down } });
                }                
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            drawTiles();

            drawPlayer(p1pos,1);
            drawPlayer(p2pos,2);

            // Draw Hello World
            string output = "Turn: " + turn;

            // Find the center of the string
            FontPos = Font1.MeasureString(output);
            Vector2 FontOrigin = Font1.MeasureString(output);
            // Draw the string
            spriteBatch.DrawString(Font1, output, FontPos, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            
            output = "Player 1: " + countTiles(1);
            FontPos = Font1.MeasureString(output);
            FontPos.Y += 24;
            FontOrigin = Font1.MeasureString(output);
            spriteBatch.DrawString(Font1, output, FontPos, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            
            output = "Player 2: " + countTiles(2);
            FontPos = Font1.MeasureString(output);
            FontPos.Y += 48;
            FontOrigin = Font1.MeasureString(output);
            spriteBatch.DrawString(Font1, output, FontPos, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private int countTiles(int playerNumber)
        {
            int tiles = 0;
            for (int i = 4; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1) - 1; j++)
                {
                    tiles += board[i, j] == playerNumber ? 1 : 0;
                }
            }
            return tiles;
        }

        private void drawPlayer(int[,] pPos, int currentPlayer)
        {

            for (int i = pPos.GetUpperBound(0); i >= 0 ; i--)
            {
                Color tint = Color.White;
                if (activePlayer == currentPlayer && currentPlayer == 1 && i == p1sel)
                    tint = Color.Gray;
                if (activePlayer == currentPlayer && currentPlayer == 2 && i == p2sel)
                    tint = Color.Blue;
                    
                board[pPos[i,0],pPos[i,1]] = currentPlayer;
                spriteBatch.Draw(tank, new Rectangle(pPos[i, 0] * scale, pPos[i, 1] * scale, scale, scale), tint);
            }
        }

        private void drawTiles()
        {
            #region init board
            for (int i = 6; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1) - 1; j++)
                {
                    if (board[i, j] == 0)
                        spriteBatch.Draw(tile_black, new Rectangle(i * scale, j * scale, scale, scale), Color.White);
                    if (board[i, j] == 1)
                        spriteBatch.Draw(tile_blue, new Rectangle(i * scale, j * scale, scale, scale), Color.White);
                    if (board[i, j] == 2)
                        spriteBatch.Draw(tile_red, new Rectangle(i * scale, j * scale, scale, scale), Color.White);
                }
            }
            #endregion           
        }
    }
}
