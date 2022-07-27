using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    /// <summary>
    /// 传送门
    /// </summary>
    public class Portal : MonoBehaviour
    {
        //该传送门所传送的房间号
        private int jumpIndex;
        public int JumpIndex
        {
            set => jumpIndex = value;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Consts.PlayerTag))
            {
                RoomManager.instance.JumpRoom(jumpIndex);
            }
        }
    }
}