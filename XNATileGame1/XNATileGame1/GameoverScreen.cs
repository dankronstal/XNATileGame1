using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNATileGame1
{
    class GameoverScreen
    {
        private Game1 game;
        SpriteBatch spriteBatch;
        SpriteFont Font1, Font2;
        public int Victor { get; set; }
        public List<ActionEntry> Actions { get; set; }
        Vector2 FontPos;

        public GameoverScreen(Game1 game)
        {
            this.game = game;
            LoadContent();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            // Create a new SpriteBatch, which can be used to draw textures.
            Font1 = game.Content.Load<SpriteFont>("GameOver");
            Font2 = game.Content.Load<SpriteFont>("InGame");
            FontPos = new Vector2(game.WindowBounds.Width / 2, game.WindowBounds.Height / 2);
        }

        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up))
                FontPos.Y++;
            if (keyState.IsKeyDown(Keys.Down))
                FontPos.Y--;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string output = "Player " + Victor + " is the winner!";

            Vector2 FontPosTitle = new Vector2(game.WindowBounds.Width / 2, game.WindowBounds.Height / 2);
            Vector2 FontOrigin = Font1.MeasureString(output);
            FontOrigin = new Vector2(FontOrigin.X / 2, FontOrigin.Y / 2);

            spriteBatch.DrawString(Font1, output, FontPosTitle, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            output = "";

            foreach (ActionEntry ae in Actions)
            {
                output += "Player " + ae.t.Player + " : Action [" + ae.at.ToString() + "] : Direction [" + ae.k + "]\n"; 
            }

            FontOrigin = Font2.MeasureString(output);
            FontOrigin = new Vector2(FontOrigin.X / 2, FontOrigin.Y / 2);

            spriteBatch.DrawString(Font2, output, FontPos, Color.Crimson, 0, FontOrigin, 0.75f, SpriteEffects.None, 0.5f);
        }
    }
}
