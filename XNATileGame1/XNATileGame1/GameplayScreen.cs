using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XNATileGame1
{
    class GameplayScreen
    {
        Game1 game;
        SpriteBatch spriteBatch;

        //Texture2D tank;
        Texture2D tile_black;
        Texture2D tile_blue;
        Texture2D tile_red;

        int scale;
        int[,] board;
        int activePlayer;
        List<Tank> p1units;
        int p1sel;
        List<Tank> p2units;
        int p2sel;
        int turn;
        List<ActionEntry> actions;
        int moveCount;
        KeyboardState lastKeyState;

        bool newTurn;

        SpriteFont Font1;
        Vector2 FontPos;

        public GameplayScreen(Game1 game)
        {
            this.game = game;
            Initialize();
            LoadContent();
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected void Initialize()
        {
            // TODO: Add your initialization logic here
            scale = 25;
            turn = 0;
            p1sel = 0;
            p2sel = 0;
            newTurn = true;
            actions = new List<ActionEntry>();
            moveCount = 0;

            #region init board
            board = new int[32, 20];
            #endregion

            #region init starting positions
            p1units = new List<Tank>();
            p1units.Add(new Tank { Player = 1, pos = new Point(18, 12) });
            p1units.Add(new Tank { Player = 1, pos = new Point(19, 12) });

            p2units = new List<Tank>();
            p2units.Add(new Tank { Player = 2, pos = new Point(18, 10) });
            p2units.Add(new Tank { Player = 2, pos = new Point(19, 10) });
            #endregion

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            // TODO: use this.Content to load your game content here
            for (int i = 0; i < p1units.Count; i++)
                p1units[i].LoadContent(game.Content, spriteBatch);
            for (int i = 0; i < p2units.Count; i++)
                p2units[i].LoadContent(game.Content, spriteBatch);
            tile_black = game.Content.Load<Texture2D>("tile_black");
            tile_blue = game.Content.Load<Texture2D>("tile_blue");
            tile_red = game.Content.Load<Texture2D>("tile_red");

            // Create a new SpriteBatch, which can be used to draw textures.
            Font1 = game.Content.Load<SpriteFont>("InGame");

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            #region manage turn
            if (newTurn)
            {
                newTurn = false;
                turn++;
                activePlayer = 1;

                for (int i = 0; i < p1units.Count; i++)
                {
                    p1units[i].movement = 2;
                }

                for (int i = 0; i < p2units.Count; i++)
                {
                    p2units[i].movement = 2;
                }

            }
            #endregion

            if (keyState != lastKeyState)// && !inAction)
            {
                lastKeyState = keyState;
                //inAction = true;                

                if (
                    keyState.IsKeyDown(Keys.Up) ||
                    keyState.IsKeyDown(Keys.Down) ||
                    keyState.IsKeyDown(Keys.Left) ||
                    keyState.IsKeyDown(Keys.Right)
                    )
                {
                    if (keyState.IsKeyDown(Keys.RightAlt))
                    {
                        #region manage firing
                        switch (activePlayer)
                        {
                            case 1:
                                checkForHits(p2units, p1units[p1sel], p1units[p1sel].Fire(keyState, actions));
                                break;
                            case 2:
                                checkForHits(p1units, p2units[p2sel], p2units[p2sel].Fire(keyState, actions));
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        #region manage movement
                        switch (activePlayer)
                        {
                            case 1:
                                p1units[p1sel].MovePlayer(keyState, actions);
                                break;
                            case 2:
                                p2units[p2sel].MovePlayer(keyState, actions);
                                break;
                        }
                        #endregion
                    }
                }
                else
                {
                    #region end turn
                    if (keyState.IsKeyDown(Keys.Space))
                    {
                        if (moveCount < actions.Count)
                        {
                            moveCount++;
                            if (activePlayer == 1)
                            {
                                p1sel++;
                                if (p1sel >= p1units.Count)
                                {
                                    p1sel = 0;
                                    activePlayer = 2;
                                }
                            }
                            else
                            {
                                p2sel++;
                                if (p2sel >= p2units.Count)
                                {
                                    p2sel = 0;
                                    newTurn = true;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            drawTiles(spriteBatch);

            drawPlayer(p1units, 1, spriteBatch);
            drawPlayer(p2units, 2, spriteBatch);

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
        }


        private void checkForHits(List<Tank> enemyUnits, Tank firingUnit, Point hitPoint)
        {
            Tank deadTank = null;
            if (firingUnit.pos != hitPoint && hitPoint != new Point())
            {
                foreach (Tank t in enemyUnits)
                {
                    if (t.pos.X == hitPoint.X && t.pos.Y == hitPoint.Y)
                    {
                        t.hit();
                        if (t.hitPoints == 0)
                            deadTank = t;
                    }
                }
                if (deadTank != null)
                    enemyUnits.Remove(deadTank);

                if (enemyUnits.Count == 0)
                    game.EndGame(firingUnit.Player, actions);
            }
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

        private void drawPlayer(List<Tank> units, int currentPlayer, SpriteBatch spriteBatch)
        {

            for (int i = 0; i < units.Count; i++)
            {
                Color tint = Color.White;
                if (activePlayer == currentPlayer && currentPlayer == 1 && i == p1sel)
                    tint = Color.Gray;
                if (activePlayer == currentPlayer && currentPlayer == 2 && i == p2sel)
                    tint = Color.Blue;

                board[units[i].pos.X, units[i].pos.Y] = currentPlayer;
                spriteBatch.Draw(units[i].tex, new Rectangle(units[i].pos.X * scale, units[i].pos.Y * scale, scale, scale), tint);
            }
        }

        private void drawTiles(SpriteBatch spriteBatch)
        {
            #region init board
            for (int i = 6; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1) - 1; j++)
                {
                    if (board[i, j] == 0)
                        drawTile(i, j, tile_black, spriteBatch);
                    if (board[i, j] == 1)
                        drawTile(i, j, tile_blue, spriteBatch);
                    if (board[i, j] == 2)
                        drawTile(i, j, tile_red, spriteBatch);
                }
            }
            #endregion
        }

        private void drawTile(int x, int y, Texture2D t, SpriteBatch spriteBatch)
        {
            KeyboardState keyState = Keyboard.GetState();
            Tank tank = new Tank();
            if (keyState.IsKeyDown(Keys.RightAlt))
            {
                #region manage firing
                switch (activePlayer)
                {
                    case 1:
                        tank = p1units[p1sel];
                        break;
                    case 2:
                        tank = p2units[p2sel];
                        break;
                }
                #endregion
                if (((x - tank.pos.X == 1 || x - tank.pos.X == -1) && y - tank.pos.Y == 0) ||
                    ((y - tank.pos.Y == 1 || y - tank.pos.Y == -1) && x - tank.pos.X == 0))
                    spriteBatch.Draw(t, new Rectangle(x * scale, y * scale, scale, scale), Color.Yellow);
                else
                    spriteBatch.Draw(t, new Rectangle(x * scale, y * scale, scale, scale), Color.White);
            }
            else
                spriteBatch.Draw(t, new Rectangle(x * scale, y * scale, scale, scale), Color.White);
        }
    }
}
