using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    /// <summary>
    /// ������
    /// </summary>
    public class Portal : MonoBehaviour
    {
        //�ô����������͵ķ����
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