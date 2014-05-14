using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XNATileGame1
{
    public class Tank
    {
        public Texture2D tex { get; set; }
        public int movement { get; set; }
        public Point pos { get; set; }
        public int hitPoints { get; set; }
        public ContentManager cm { get; set; }
        public int Player { get; set; }

        internal void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            cm = content;
            tex = cm.Load<Texture2D>("tank");
            this.movement = 2;
            this.hitPoints = 1;
        }

        internal void MovePlayer(KeyboardState keyState, List<ActionEntry> actions)
        {
            if (movement > 0)
            {
                movement--;
                if (keyState.IsKeyDown(Keys.Left) && pos.X > 0)
                {
                    pos = new Point(pos.X - 1, pos.Y);
                    actions.Add(new ActionEntry() { at = ActionTypes.Movement, t = this, k = Keys.Left });
                }
                if (keyState.IsKeyDown(Keys.Right) && pos.X < 31)
                {
                    pos = new Point(pos.X + 1, pos.Y);
                    actions.Add(new ActionEntry() { at = ActionTypes.Movement, t = this, k = Keys.Right });
                }
                if (keyState.IsKeyDown(Keys.Up) && pos.Y > 0)
                {
                    pos = new Point(pos.X, pos.Y - 1);
                    actions.Add(new ActionEntry() { at = ActionTypes.Movement, t = this, k = Keys.Up });
                }
                if (keyState.IsKeyDown(Keys.Down) && pos.Y < 18)
                {
                    pos = new Point(pos.X, pos.Y + 1);
                    actions.Add(new ActionEntry() { at = ActionTypes.Movement, t = this, k = Keys.Down });
                }
            }
        }

        internal Point Fire(KeyboardState keyState, List<ActionEntry> actions)
        {
            actions.Add(new ActionEntry() { at = ActionTypes.Firing, t = this, k = Keys.Down });
            if (keyState.IsKeyDown(Keys.Left) && pos.X > 0)
            {
                return new Point(pos.X - 1, pos.Y);
            }
            if (keyState.IsKeyDown(Keys.Right) && pos.X < 31)
            {
                return new Point(pos.X + 1, pos.Y);
            }
            if (keyState.IsKeyDown(Keys.Up) && pos.Y > 0)
            {
                return new Point(pos.X, pos.Y - 1);
            }
            if (keyState.IsKeyDown(Keys.Down) && pos.Y < 18)
            {
                return new Point(pos.X, pos.Y + 1);
            }
            return new Point();
        }

        internal void hit()
        {
            hitPoints--;
            if (hitPoints == 0)
                tex = cm.Load<Texture2D>("tank_dead");
        }
    }
}
