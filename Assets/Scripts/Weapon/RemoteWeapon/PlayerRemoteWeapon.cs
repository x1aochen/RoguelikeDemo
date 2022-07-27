using System.Collections;
using UnityEngine;


namespace Player
{
    public class PlayerRemoteWeapon : RemoteWeapon
    {
        protected override void Start()
        {
            base.Start();
            targetTag = "Enemy";
        }

        private void Update()
        {
            atkTimer += Time.deltaTime;
        }

        /// <summary>
        /// 装填弹药
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator FillingBullet()
        {
            data.isFilling = cantFire = true;

            float fillTimer = 0;
            while (fillTimer < remote.fillTime)
            {
                data.obj.transform.Rotate(Vector3.forward, 20);
                fillTimer += Time.deltaTime;
                yield return null;
            }
            currentMagazineCount = remote.magazineCapacity;
            remote.bulletAmount -= remote.magazineCapacity;

            data.isFilling = false;
            //暂停一帧，让武器转向鼠标点才可以进行攻击
            yield return null;
            cantFire = false;
        }

        public override void Attack()
        {
            //攻击间隔不够 || 正在装填弹药
            if (atkTimer < remote.atkInterval || cantFire)
            {
                return;
            }

            //判断子弹是否足够
            if (currentMagazineCount <= 0)
            {
                StartCoroutine(FillingBullet());
                return;
            }

            base.Attack();

            currentMagazineCount--;
            atkTimer = 0;
        }
    }
}