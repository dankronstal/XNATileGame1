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

        Texture2D tile_black;
        Texture2D tile_blue;
        Texture2D tile_red;

        public Player p1;
        public Player p2;
        public Board board;
        
        int activePlayer;
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
            turn = 0;
            newTurn = true;
            actions = new List<ActionEntry>();
            moveCount = 0;

            #region init board
            board = new Board() { Tiles = new int[32, 20] };
            #endregion
            
            #region init player
            p1 = new Player() { Id = 1, Resources = 0, Tint = Color.Gray, Effect = SpriteEffects.None, IsActive = true };
            List<Tank> p1units = new List<Tank>();
            p1units.Add(new Tank { Player = p1, pos = new Point(18, 12), IsActive = true });
            p1units.Add(new Tank { Player = p1, pos = new Point(19, 12) });
            p1.Units = p1units;

            p2 = new Player() { Id = 2, Resources = 0, Tint = Color.Gray, Effect = SpriteEffects.FlipVertically };
            List<Tank> p2units = new List<Tank>();
            p2units.Add(new Tank { Player = p2, pos = new Point(18, 10) });
            p2units.Add(new Tank { Player = p2, pos = new Point(19, 10) });
            p2.Units = p2units;
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
            for (int i = 0; i < p1.Units.Count; i++)
                p1.Units[i].LoadContent(game.Content, spriteBatch);
            for (int i = 0; i < p2.Units.Count; i++)
                p2.Units[i].LoadContent(game.Content, spriteBatch);
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

            #region start new turn
            if (newTurn)
            {
                newTurn = false;
                turn++;
                activePlayer = 1;

                for (int i = 0; i < p1.Units.Count; i++)
                {
                    p1.Units[i].movement = 2;
                }

                for (int i = 0; i < p2.Units.Count; i++)
                {
                    p2.Units[i].movement = 2;
                }

            }
            #endregion

            if (keyState != lastKeyState)
            {
                lastKeyState = keyState;          

                if (
                    keyState.IsKeyDown(Keys.Up) ||
                    keyState.IsKeyDown(Keys.Down) ||
                    keyState.IsKeyDown(Keys.Left) ||
                    keyState.IsKeyDown(Keys.Right)
                    )
                {
                    if (keyState.IsKeyDown(Keys.RightAlt) && keyState.GetPressedKeys().Length == 2)
                    {
                        #region manage firing
                        checkForHits(OpponentPlayer().Units, ActivePlayer().ActiveUnit, ActivePlayer().ActiveUnit.Fire(keyState, actions));
                        /*
                        switch (activePlayer)
                        {
                            case 1:
                                checkForHits(p2.Units, p1.ActiveUnit(), p1.ActiveUnit().Fire(keyState, actions));
                                break;
                            case 2:
                                checkForHits(p1.Units, p2.Units[p2sel], p2.Units[p2sel].Fire(keyState, actions));
                                break;
                        }
                        */
                        #endregion
                    }
                    else if (keyState.GetPressedKeys().Length == 1)
                    {
                        #region manage movement
                        List<Tank> units = new List<Tank>();
                        units.AddRange(p1.Units);
                        units.AddRange(p2.Units);

                        ActivePlayer().ActiveUnit.MoveUnit(keyState, actions, units);
                        #endregion
                    }
                }
                else if (keyState.IsKeyDown(Keys.Space))
                {
                    #region end turn
                    List<Player> players = new List<Player>() { p1, p2 };
                    Player activeP = players.Where(x => x.IsActive == true).First();

                    if (!activeP.SelectNextUnit())
                    {
                        activeP.Units.First(x => x.IsActive).IsActive = false;
                        activeP.IsActive = false;

                        activeP = players.First(x => x.Id != activeP.Id);

                        activeP.IsActive = true;
                        activeP.Units.First().IsActive = true;

                        if (activeP == p2)
                        {
                            newTurn = true;
                            p1.NewTurn();
                            p2.NewTurn();
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

            p1.Draw(board, p2, spriteBatch);
            p2.Draw(board, p1, spriteBatch);

            #region Turn counter
            string output = "Turn: " + turn;

            // Find the center of the string
            FontPos = Font1.MeasureString(output);
            Vector2 FontOrigin = Font1.MeasureString(output);
            // Draw the string
            spriteBatch.DrawString(Font1, output, FontPos, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            #endregion

            #region Player stats
            output = "Player 1: \n" + " - Units " + p1.Units.Count + "\n - Tiles " + p1.Tiles + "\n - Res.: " + p1.Resources.ToString("##");
            FontPos = Font1.MeasureString(output);
            FontPos.Y += 48;
            FontOrigin = Font1.MeasureString(output);
            spriteBatch.DrawString(Font1, output, FontPos, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            output = "Player 2: \n" + " - Units " + p2.Units.Count + "\n - Tiles " + p2.Tiles + "\n - Res.: " + p2.Resources.ToString("##");
            FontPos = Font1.MeasureString(output);
            FontPos.Y += 176;
            FontOrigin = Font1.MeasureString(output);
            spriteBatch.DrawString(Font1, output, FontPos, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            #endregion

            #region Unit stats
            output = "Active Unit: \n" + " - Health " + ActivePlayer().ActiveUnit.hitPoints + "\n - Moves  " + ActivePlayer().ActiveUnit.movement+ "\n - Kills  ?";
            FontPos = Font1.MeasureString(output);
            FontPos.Y += 300;
            FontOrigin = Font1.MeasureString(output);
            spriteBatch.DrawString(Font1, output, FontPos, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            #endregion
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

        private void drawTiles(SpriteBatch spriteBatch)
        {
            #region init board
            for (int i = 6; i < board.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < board.Tiles.GetLength(1) - 1; j++)
                {
                    switch(board.WhoOwns(i,j))
                    {
                        case 1:
                            drawTile(i, j, tile_blue, spriteBatch);
                            break;
                        case 2:
                            drawTile(i, j, tile_red, spriteBatch);
                            break;
                        default:
                            drawTile(i, j, tile_black, spriteBatch);
                            break;
                    }
                }
            }
            #endregion
        }

        private void drawTile(int x, int y, Texture2D t, SpriteBatch spriteBatch)
        {
            KeyboardState keyState = Keyboard.GetState();            
            
            if (keyState.IsKeyDown(Keys.RightAlt))
            {
                #region manage firing indicators
                Tank tank = ActivePlayer().ActiveUnit;
                if (((x - tank.pos.X == 1 || x - tank.pos.X == -1) && y - tank.pos.Y == 0) ||
                    ((y - tank.pos.Y == 1 || y - tank.pos.Y == -1) && x - tank.pos.X == 0))
                    spriteBatch.Draw(t, new Rectangle(x * Game1.Scale, y * Game1.Scale, Game1.Scale, Game1.Scale), Color.Yellow);
                else
                    spriteBatch.Draw(t, new Rectangle(x * Game1.Scale, y * Game1.Scale, Game1.Scale, Game1.Scale), Color.White);
                #endregion
            }
            else
                spriteBatch.Draw(t, new Rectangle(x * Game1.Scale, y * Game1.Scale, Game1.Scale, Game1.Scale), Color.White);
           
        }

        private Player ActivePlayer()
        {
            return new List<Player>() { p1, p2 }.First(x => x.IsActive == true);
        }

        private Player OpponentPlayer()
        {
            return new List<Player>() { p1, p2 }.First(x => x.IsActive == false);
        }
    }
}
