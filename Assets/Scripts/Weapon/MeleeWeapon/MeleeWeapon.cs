using System.Collections;
using UnityEngine;

namespace Player
{
    public class MeleeWeapon : WeaponAttack
    {

        protected float desiredDuration = 0.5f;  //期望持续时间
        protected float elapsedTime; //经过时间
        protected MeleeWeaponData melee;
        protected BoxCollider2D box;
        protected virtual void Awake()
        {
            box = GetComponentInChildren<BoxCollider2D>();
            box.enabled = false;
        }

        protected virtual void Start()
        {
            melee = (MeleeWeaponData)data;
        }

        public override void Attack()
        {
            if (cantFire)
            {
                return;
            }
            box.enabled = true;
            StartCoroutine(RotationAttack());
        }
        /// <summary>
        /// 插值旋转武器，劈砍效果
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator RotationAttack()
        {

            data.isFilling = true;
            cantFire = true;
            elapsedTime = 0;
            Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - melee.atkAngle);
            Quaternion startRotation = transform.rotation;
            while (elapsedTime < desiredDuration)
            {
                elapsedTime += Time.deltaTime * 2;
                float percentageComplete = elapsedTime / desiredDuration;
                transform.rotation = Quaternion.Lerp(startRotation, target, Mathf.SmoothStep(0, 1, percentageComplete));
                yield return null;
            }


            data.isFilling = false;
            yield return null;
            cantFire = false;
            box.enabled = false;
        }

    }
}