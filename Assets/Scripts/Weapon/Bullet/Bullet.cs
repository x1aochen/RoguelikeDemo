using System;
using System.Collections.Generic;
using UnityEngine;
namespace Player 
{
    /// <summary>
    /// 普通子弹，向前直线飞去
    /// </summary>
    public class Bullet : MonoBehaviour,IReset
    {
        protected RemoteResetArgs args;
        protected Vector2 startPos;
        public virtual void Reset(RemoteResetArgs args)
        {
            this.args = args;
            startPos = transform.position;
        }
        protected virtual void Update()
        {
            if (Vector2.Distance(startPos, transform.position) > args.Range)
                gameObject.SetActive(false);
            else
                transform.position += transform.right * Time.deltaTime * args.Speed;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(args.TargetTag))
            {
                args.action(collision);
                GameObjectPool.instance.RecycleObject(gameObject);
            }
            if (collision.CompareTag("Wall"))
            {
                GameObjectPool.instance.RecycleObject(gameObject);
            }
        }
    }
}
