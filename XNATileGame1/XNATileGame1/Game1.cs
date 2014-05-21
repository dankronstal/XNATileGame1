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
    enum Screen
    {
        StartScreen,
        GameplayScreen,
        GameoverScreen
    }

    public struct ActionEntry
    {
        public ActionTypes actionType;
        public Tank tank;
        public Keys key;
        public Point actionFrom;
        public Point actionTo;
    }

    public enum ActionTypes
    {
        Movement,
        Firing
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        StartScreen startScreen;
        GameplayScreen gamePlayScreen;
        GameoverScreen gameOverScreen;

        Screen currentScreen;

        public static int Scale = 25;

        public Rectangle WindowBounds { get { return this.Window.ClientBounds; } }

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
            startScreen = new StartScreen(this);
            currentScreen = Screen.StartScreen;

            base.LoadContent();
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            switch (currentScreen)
            {
                case Screen.StartScreen:
                    if (startScreen != null)
                        startScreen.Update();
                    break;
                case Screen.GameplayScreen:
                    if (gamePlayScreen != null)
                        gamePlayScreen.Update(gameTime);
                    break;
                case Screen.GameoverScreen:
                    if (gameOverScreen != null)
                        gameOverScreen.Update();
                    break;
            }

            base.Update(gameTime);
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

            switch (currentScreen)
            {
                case Screen.StartScreen:
                    if (startScreen != null)
                        startScreen.Draw(spriteBatch);
                    break;
                case Screen.GameplayScreen:
                    if (gamePlayScreen != null)
                        gamePlayScreen.Draw(spriteBatch);
                    break;
                case Screen.GameoverScreen:
                    if (gameOverScreen != null)
                        gameOverScreen.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void StartGame()
        {
            gamePlayScreen = new GameplayScreen(this);
            currentScreen = Screen.GameplayScreen;
            startScreen = null;
        }

        public void EndGame(Player victor, List<ActionEntry> actions)
        {
            gameOverScreen = new GameoverScreen(this);
            gameOverScreen.Victor = victor;
            gameOverScreen.Actions = actions;
            currentScreen = Screen.GameoverScreen;
            gamePlayScreen = null;
        }
    }
}