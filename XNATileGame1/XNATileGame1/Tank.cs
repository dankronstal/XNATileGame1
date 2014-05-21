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
        public static int maxMovement = 2;
        public Texture2D tex { get; set; }
        public int movement { get; set; }
        public Point pos { get; set; }
        public int hitPoints { get; set; }
        public ContentManager cm { get; set; }
        public Player Player { get; set; }
        public bool IsActive { get; set; }

        internal void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            cm = content;
            tex = cm.Load<Texture2D>("tank");
            this.movement = 2;
            this.hitPoints = 1;
        }

        internal void MoveUnit(KeyboardState keyState, List<ActionEntry> actions, List<Tank> presentUnits)
        {
            Point destination = getTargetPoint(this.pos,keyState);
            if (movement > 0 && inBounds(destination) && !isTileOccupied(destination, presentUnits))
            {
                movement--;
                actions.Add(new ActionEntry() { actionType = ActionTypes.Movement, tank = this, actionFrom = pos, actionTo = destination });
                pos = destination;
            }            
        }

        /// <summary>
        /// Detect whether or not a specified point falls within tile boundary
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool inBounds(Point p)
        {
            //TODO: abstract boundary in Game1 class
            return p.X >= 0  && p.X <= 31 && p.Y >= 0  && p.Y <= 18;
        }

        internal Point Fire(KeyboardState keyState, List<ActionEntry> actions)
        {
            Point target = getTargetPoint(this.pos, keyState);
            actions.Add(new ActionEntry() { actionType = ActionTypes.Firing, tank = this, actionFrom = pos, actionTo = target });
            return target;
        }

        internal void hit()
        {
            hitPoints--;
            if (hitPoints == 0)
                tex = cm.Load<Texture2D>("tank_dead");
        }

        /// <summary>
        /// Resolve a directional action to it's target tile
        /// </summary>
        /// <param name="origin">Point from which the action was intiated</param>
        /// <param name="key">Direction of impact</param>
        /// <returns>Destination or target point</returns>
        internal Point getTargetPoint(Point origin, KeyboardState keyState)
        {
            Point destination = origin;
            if(keyState.IsKeyDown(Keys.Left))
                    destination = new Point(origin.X - 1, origin.Y);
            if(keyState.IsKeyDown(Keys.Right))
                    destination = new Point(origin.X + 1, origin.Y);
            if(keyState.IsKeyDown(Keys.Up))
                    destination = new Point(origin.X, origin.Y - 1);
            if(keyState.IsKeyDown(Keys.Down))
                    destination = new Point(origin.X, origin.Y + 1);
            
            return destination;
        }

        private static bool isTileOccupied(Point target, List<Tank> units)
        {
            foreach (Tank t in units)
            {
                if (t.pos.X == target.X && t.pos.Y == target.Y)
                    return true;
            }
            return false;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Rectangle(pos.X * Game1.Scale, pos.Y * Game1.Scale, Game1.Scale, Game1.Scale), null, (IsActive ? Player.Tint : Color.White), 0f, new Vector2(pos.X, pos.Y), Player.Effect, 0f);
        }
    }
}
