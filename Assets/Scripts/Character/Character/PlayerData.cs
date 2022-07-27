using UnityEngine;

namespace Player
{
    public class PlayerData : CharacterData
    {
        public PlayerData() { }

        public PlayerData(PlayerData data)
        {
            this.hp = this.maxHp = data.maxHp;
            this.moveSpeed = data.moveSpeed;
        }

    }
}