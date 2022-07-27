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
        /// װ�ҩ
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
            //��ͣһ֡��������ת������ſ��Խ��й���
            yield return null;
            cantFire = false;
        }

        public override void Attack()
        {
            //����������� || ����װ�ҩ
            if (atkTimer < remote.atkInterval || cantFire)
            {
                return;
            }

            //�ж��ӵ��Ƿ��㹻
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