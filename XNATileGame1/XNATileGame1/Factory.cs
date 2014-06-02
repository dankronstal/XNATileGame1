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
    public class Factory : Unit
    {
        public override void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            cm = content;
            tex = cm.Load<Texture2D>("unit_factory");
            maxMovement = 1;
            movement = 1;
            hitPoints = 5;
        }

        internal Tank Deploy(KeyboardState keyState, List<ActionEntry> actions)
        {
            Point target = getTargetPoint(this.pos, keyState);
            actions.Add(new ActionEntry() { actionType = ActionTypes.Firing, unit = this, actionFrom = pos, actionTo = target });
            Tank t = new Tank { Player = this.Player, pos = target };
            Player.Units.Add(t);
            Player.Resources -= 200;
            return t;
        }
    }
}
