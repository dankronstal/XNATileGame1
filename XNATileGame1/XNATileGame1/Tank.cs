using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace XNATileGame1
{
    class Tank
    {
        public Texture2D tex { get; set; }
        public int movement { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }

        internal void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            tex = content.Load<Texture2D>("tank");
            this.movement = 2;
        }

        internal void MovePlayer(KeyboardState keyState, Dictionary<int, Dictionary<Tank, Keys>> moves)
        {
            if (movement > 0)
            {
                if (keyState.IsKeyDown(Keys.Left) && posX > 0)
                {
                    posX--;
                    movement--;
                    moves.Add(moves.Count, new Dictionary<Tank, Keys>() { { this, Keys.Left } });
                }
                if (keyState.IsKeyDown(Keys.Right) && posX < 31)
                {
                    posX++;
                    movement--;
                    moves.Add(moves.Count, new Dictionary<Tank, Keys>() { { this, Keys.Right } });
                }
                if (keyState.IsKeyDown(Keys.Up) && posY > 0)
                {
                    posY--;
                    movement--;;
                    moves.Add(moves.Count, new Dictionary<Tank, Keys>() { { this, Keys.Up } });
                }
                if (keyState.IsKeyDown(Keys.Down) && posY < 18)
                {
                    posY++;
                    movement--;
                    moves.Add(moves.Count, new Dictionary<Tank, Keys>() { { this, Keys.Down } });
                }
            }
        }
    }
}
