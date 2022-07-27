using UnityEngine;


namespace Player
{
    public class EnemyRemoteWeapon : RemoteWeapon
    {
        protected override void Start()
        {
            base.Start();
            remote.firePoint = CodeHelper.FindChild(transform, "FirePoint");
            targetTag = Consts.PlayerTag;
            target = PlayerController.instance.transform;
        }
        
        private Transform target;
        private void Update()
        {
            //TODO:武器面向目标
            if (target != null)
            {
                Vector3 direction = target.position - transform.position;
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            }
        }

        public override void Attack()
        {
            base.Attack();
        }
    }
}