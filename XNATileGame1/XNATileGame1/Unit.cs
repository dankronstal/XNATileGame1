using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace XNATileGame1
{
    public class Unit
    {
        public virtual int maxMovement { get; set; }
        public Texture2D tex { get; set; }
        public int movement { get; set; }
        public Point pos { get; set; }
        public int hitPoints { get; set; }
        public ContentManager cm { get; set; }
        public Player Player { get; set; }
        public UnitStatus Status { get; set; }

        public virtual void LoadContent(ContentManager content, SpriteBatch spriteBatch) { }

        internal void MoveUnit(KeyboardState keyState, List<ActionEntry> actions, List<Unit> presentUnits)
        {
            Point destination = getTargetPoint(this.pos, keyState);
            if (movement > 0 && inBounds(destination) && !isTileOccupied(destination, presentUnits))
            {
                actions.Add(new ActionEntry() { actionType = ActionTypes.Movement, unit = this, actionFrom = pos, actionTo = destination });
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
            return p.X >= 0 && p.X <= 31 && p.Y >= 0 && p.Y <= 18;
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
            if (keyState.IsKeyDown(Keys.Left))
                destination = new Point(origin.X - 1, origin.Y);
            if (keyState.IsKeyDown(Keys.Right))
                destination = new Point(origin.X + 1, origin.Y);
            if (keyState.IsKeyDown(Keys.Up))
                destination = new Point(origin.X, origin.Y - 1);
            if (keyState.IsKeyDown(Keys.Down))
                destination = new Point(origin.X, origin.Y + 1);

            return destination;
        }

        private static bool isTileOccupied(Point target, List<Unit> units)
        {
            foreach (Unit u in units)
            {
                if (u.pos.X == target.X && u.pos.Y == target.Y)
                    return true;
            }
            return false;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(pos.X * Game1.Scale, pos.Y * Game1.Scale, Game1.Scale, Game1.Scale);
            Rectangle? sourceRectangle = null; //use a rectangle, if specifying a region on a sprite sheet
            Color tint = (Status == UnitStatus.Active ? Player.Tint : Color.White);
            float rotation = 0f;
            Vector2 position = new Vector2(0, 0);// new Vector2(pos.X, pos.Y);
            SpriteEffects effects = Player.Effect;
            float layerDepth = 0f;

            spriteBatch.Draw(
                tex, 
                destinationRectangle, 
                sourceRectangle, 
                tint,
                rotation, 
                position,
                effects,
                layerDepth);
        }
    }
}
