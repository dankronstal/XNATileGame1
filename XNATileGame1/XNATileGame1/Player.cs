using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATileGame1
{
    public class Player
    {
        public int Id { get; set; }
        public int Tiles { get; set; }
        public double Resources  { get; set; }
        public List<Unit> Units { get; set; }
        public SpriteEffects Effect { get; set; }
        public Color Tint { get; set; }
        public bool IsActive { get; set; }
        public Unit ActiveUnit { get { return Units.First(x => x.Status == UnitStatus.Active); } }


        public void Draw(Board board, Player opponent, SpriteBatch spriteBatch)
        {            
            foreach (Unit u in Units)
            {
                board.CaptureTile(new Point(u.pos.X, u.pos.Y), this, opponent);
                u.Draw(spriteBatch);
            }
        }

        internal void NewTurn()
        {
            Resources += Tiles * 0.1f;

            foreach (Unit u in Units)
            {
                u.movement = u.maxMovement;
                u.Status = UnitStatus.Waiting;
            }
        }

        internal bool SelectNextUnit()
        {
            List<Unit> units = Units.Where(x => x.Status == UnitStatus.Waiting).ToList();
            if (units.Count > 0)
            {
                Units.First(x => x.Status == UnitStatus.Active).Status = UnitStatus.Done;
                units.First().Status = UnitStatus.Active;
                return true;
            }
            return false;
        }
    }
}
