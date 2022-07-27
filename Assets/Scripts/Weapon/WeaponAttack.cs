using UnityEngine;

namespace Player
{  
    /// <summary>
    /// 所有武器基类
    /// </summary>
    public abstract class WeaponAttack : MonoBehaviour
    {
        protected WeaponData data;
        protected float atkTimer;
        protected bool cantFire;
        protected string targetTag;
        public WeaponData Data
        {
            set => data = value;
        }
        public abstract void Attack();

        public virtual void BeDamage(Collider2D collision)
        {
            collision.transform.GetComponent<IBeDamage>().BeDamge(data.damage);
        }
    }
}