using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public struct RemoteResetArgs
    {
        private float speed;
        private float range;
        private string targetTag;
        public float Speed { get => speed;}

        public float Range { get => range;}
        public string TargetTag { get => targetTag; }

        public Action<Collider2D> action { get; }

        public RemoteResetArgs(float s,float r,string tag,Action<Collider2D> act)
        {
            speed = s;
            range = r;
            targetTag = tag;
            action = act;
        }
    }

    public class RemoteWeapon : WeaponAttack
    {
        protected int currentMagazineCount;
        protected RemoteWeaponData remote;

        protected virtual void Start()
        {
            remote = (RemoteWeaponData)data;
            currentMagazineCount = remote.magazineCapacity;
        }
        
        public override void Attack()
        {
            GameObject fireEffect = GameObjectPool.instance.CreateObject(remote.fireEffectName, remote.fireEffect,remote.firePoint.position, Quaternion.Euler(data.firePoint.eulerAngles));
            GameObject bullet = GameObjectPool.instance.CreateObject(remote.bulletName, remote.bullet,remote.firePoint.position, Quaternion.Euler(data.firePoint.eulerAngles));
            //回收特效
            GameObjectPool.instance.RecycleObject(fireEffect, 0.2f);
            //重置参数
            RemoteResetArgs args = new RemoteResetArgs(remote.bulletSpeed, remote.attackRange,targetTag,BulletCallBack);
            bullet.GetComponent<IReset>().Reset(args);
        }

        protected virtual void BulletCallBack(Collider2D collision)
        {
            BeDamage(collision);
        }
    }
}