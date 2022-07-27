using UnityEngine;

namespace Player
{
    public class PlayerMeleeWeapon : MeleeWeapon
    {
        private string[] targetTags;
        protected override void Awake()
        {
            base.Awake();
            targetTags = new string[2];
            targetTags[0] = "Enemy";
            targetTags[1] = "Bullet";
            player = CodeHelper.FindChild(PlayerController.instance.transform, "WeaponPoint");
        }


        private Transform player;
        private void Update()
        {
            //TODO: 子物体添加刚体不跟随父物体移动？
            transform.position = player.position;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
           
            if (collision.CompareTag(targetTags[0]))
            {
                BeDamage(collision);
            }
            //劈砍子弹
            else if (collision.CompareTag(targetTags[1]))
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}