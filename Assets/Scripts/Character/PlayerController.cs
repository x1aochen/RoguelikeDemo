using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum Face
    {
        Right,
        Left
    }

    //保证在场景切换时只有一份
    public class PlayerController : MonoSingleton<PlayerController>,IAlive,IBeDamage
    {
        private Rigidbody2D rb;
        private Animator anim;
        [SerializeField]
        private float moveSpeed = 5f;

        private WeaponSystem weapon;
        private PlayerData data;

        private SpriteRenderer sprite;
        public bool IsAlive => data.hp > 0;

        public override void Init()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            weapon = GetComponent<WeaponSystem>();
            data = LoadGameData.GetPlayerData(1) as PlayerData;
            sprite = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 玩家移动
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public void Move(float x, float z)
        {
            if (x != 0 || z != 0)
            {
                rb.velocity = new Vector2(x, z) * moveSpeed;
                anim.SetBool("Run", true);
            }
            else
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Run", false);
            }
        }
        public void Rotate(Vector3 cursorPos)
        {
            weapon.RotatePlayer(cursorPos);
        }

        public void SwitchWeapon(int index)
        {
            weapon.SwitchWeapon(index);
        }

        public void Attack() 
        {
            
            weapon.Attak();
        }

        public void BeDamge(int damage)
        {
            //TODO:
            Debug.Log("玩家受到伤害:" + damage); 
        }
    }
}
