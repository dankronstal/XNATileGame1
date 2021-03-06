﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XNATileGame1
{
    public class Tank : Unit
    {
        public override void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            cm = content;
            tex = cm.Load<Texture2D>("unit_tank");
            maxMovement = 2;
            movement = 2;
            hitPoints = 1;
        }

        internal Point Fire(KeyboardState keyState, List<ActionEntry> actions)
        {
            Point target = getTargetPoint(this.pos, keyState);
            actions.Add(new ActionEntry() { actionType = ActionTypes.Firing, unit = this, actionFrom = pos, actionTo = target });
            return target;
        }
    }
}
