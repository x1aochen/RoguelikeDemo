using Player;
using UnityEngine;

public class RemoteWeaponData : WeaponData
{
    public float bulletSpeed; //子弹速度
    public float atkInterval; //攻击间隔
    public int bulletAmount; //子弹总数 
    public int magazineCapacity; //弹匣容量
    public GameObject bullet; //子弹预制体
    public string bulletName; //子弹名
    public string fireEffectName; //枪口特效名
    public GameObject fireEffect; //枪口特效
    public float fillTime; //装填时间
    public float attackRange; //射程

    public RemoteWeaponData()
    {

    }

    public RemoteWeaponData(RemoteWeaponData data)
    {
        this.obj = data.obj;
        this.weaponName = data.weaponName;
        this.damage = data.damage;
        this.bulletSpeed = data.bulletSpeed;
        this.atkInterval = data.atkInterval;
        this.bulletAmount = data.bulletAmount;
        this.magazineCapacity = data.magazineCapacity;
        this.bullet = data.bullet;
        this.bulletName = data.bulletName;
        this.fireEffectName = data.fireEffectName;
        this.fireEffect = data.fireEffect;
        this.firePoint = data.firePoint;
        this.owner = data.owner;
        this.fillTime = data.fillTime;
        this.isFilling = data.isFilling;
        this.attackRange = data.attackRange;
        this.prefab = data.prefab;
    }

}

