using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNATileGame1
{
    public class StartScreen
    {
        private Game1 game;
        SpriteBatch spriteBatch;
        private KeyboardState lastState;
        SpriteFont Font1;

        public StartScreen(Game1 game)
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
            Font1 = game.Content.Load<SpriteFont>("Title");
        }

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter))
            {
                game.StartGame();
            }

            lastState = keyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string output = "Press Enter To Start!!";

            Vector2 FontPos = new Vector2(game.WindowBounds.Width / 2, game.WindowBounds.Height / 2);
            Vector2 FontOrigin = Font1.MeasureString(output);
            FontOrigin.X = FontOrigin.X / 2;
            FontOrigin.Y = FontOrigin.Y / 2;
            
            spriteBatch.DrawString(Font1, output, FontPos, Color.Crimson, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }
    }
}
