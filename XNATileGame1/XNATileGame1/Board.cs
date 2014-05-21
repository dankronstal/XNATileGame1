using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNATileGame1
{
    public class Board
    {
        public int[,] Tiles { get; set; }

        internal void CaptureTile(Point pos, Player currentPlayer, Player opposingPlayer)
        {
            if (Tiles[pos.X, pos.Y] == 0)
            {
                currentPlayer.Tiles++;
                currentPlayer.Resources += 10;
            }
            if (Tiles[pos.X, pos.Y] == opposingPlayer.Id)
            {
                currentPlayer.Tiles++;
                opposingPlayer.Tiles--;
            }
            Tiles[pos.X, pos.Y] = currentPlayer.Id;
        }

        internal int WhoOwns(int x, int y)
        {
            return Tiles[x, y];
        }
    }
}
