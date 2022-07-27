using Player;
using System.Collections;
using UnityEngine;

namespace AI
{
    //TODO:近战敌人攻击多样?
    /// <summary>
    /// AI RedMouse攻击类
    /// </summary>
    public class RedMouseAttack : MeleeWeapon
    {
        private Transform go;
        private Transform target;

        protected override void Start()
        {
            base.Start();
            target = PlayerController.instance.transform;
            targetTag = Consts.PlayerTag;
            go = data.owner;
        }

        public override void Attack()
        {
            box.enabled = true;
            StartCoroutine(GoAttack());
        }

        private IEnumerator GoAttack()
        {
            Vector3 targetPos = target.position;
            while (Vector3.Distance(targetPos, go.position) > 0.3 )
            {
                go.position = Vector3.MoveTowards(go.position, targetPos, Time.deltaTime * 6);
                yield return null;
            }
            box.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(targetTag))
            {
                BeDamage(collision);
            }
        }
    }

}