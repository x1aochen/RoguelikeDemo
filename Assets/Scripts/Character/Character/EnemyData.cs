using System;
using UnityEngine;

public enum EnemyType
{
    RedMouse = 1,
    ArrowSoldier = 2,
    BlueElder = 3
}

namespace Player
{
    public class EnemyData : CharacterData
    {
        public float runSpeed;
        public float sightDist;
        public float attackDist;
        public EnemyType type;
        public int damage;
        public GameObject prefab;
        public string configurationFile;
        public int sightAngle;
        public float detectionInterval;
        public float atkInterval;
        public int wpID;

        public EnemyData()
        {

        }

        public EnemyData(EnemyData data)
        {
            this.hp = this.maxHp = data.maxHp;
            this.moveSpeed = data.moveSpeed;
            this.runSpeed = data.runSpeed;
            this.sightDist = data.sightDist;
            this.attackDist = data.attackDist;
            this.prefab = data.prefab;
            this.configurationFile = data.configurationFile;
            this.sightAngle = data.sightAngle;
            this.detectionInterval = data.detectionInterval;
            this.atkInterval = data.atkInterval;
            this.wpID = data.wpID;
        }
    }
}
