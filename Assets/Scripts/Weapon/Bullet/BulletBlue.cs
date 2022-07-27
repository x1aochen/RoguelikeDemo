using System;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    /// <summary>
    /// 散弹
    /// </summary>
    public class BulletBlue :Bullet
    {
        private float angle = 15f; //子弹间隔角度
        private int number  = 3 ;
        private GameObject prefab;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Bullet/Bullet_BlueGo");
        }

        protected override void Update()
        {

        }

        public override void Reset(RemoteResetArgs args)
        {
            base.Reset(args);
            float offset = number / 2;
            //根据分裂子弹数量设置角度偏移量
            //分裂奇数个子弹
            if ((number - 1) % 2 == 1)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + angle * offset);
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + angle * offset - angle / 2);
            }
            //孩子偏移角度
            for (int i = 0;i < number;i++)
            {
                GameObject go = GameObjectPool.instance.CreateObject("Bullet_BlueGo", prefab, transform.position,
                    Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - angle * i)));
                go.GetComponent<IReset>().Reset(args);
            }
            GameObjectPool.instance.RecycleObject(gameObject);
        }
        
        

    }
}
