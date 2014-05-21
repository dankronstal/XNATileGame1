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
        public List<Tank> Units { get; set; }
        public SpriteEffects Effect { get; set; }
        public Color Tint { get; set; }
        public bool IsActive { get; set; }
        public Tank ActiveUnit { get { return Units.First(x => x.IsActive == true); } }


        public void Draw(Board board, Player opponent, SpriteBatch spriteBatch)
        {            
            foreach (Tank t in Units)
            {
                board.CaptureTile(new Point(t.pos.X, t.pos.Y), this, opponent);
                t.Draw(spriteBatch);
            }
        }

        internal void NewTurn()
        {
            Resources += Tiles * 0.1f;

            foreach (Tank t in Units)
            {
                t.movement = Tank.maxMovement;
            }
        }

        internal bool SelectNextUnit()
        {
            List<Tank> tanks = Units.Where(x => x.movement > 0).ToList();
            if (tanks.Count > 0)
            {
                Units.First(x => x.IsActive == true).IsActive = false;
                tanks.First().IsActive = true;
                return true;
            }
            return false;
        }
    }
}
