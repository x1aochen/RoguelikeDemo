using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    public class Room02 : Room
    {
        protected override void Awake()
        {
            roomIndex = 1;
            enemies = new EnemyType[] { EnemyType.RedMouse, EnemyType.BlueElder };
            base.Awake();

        }

    }
}