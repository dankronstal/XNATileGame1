using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNATileGame1
{
    public class Player
    {
        public int Id { get; set; }
        public float Resources  { get; set; }
        public List<Tank> Units { get; set; }
    }
}
