using System;
using UnityEngine;


namespace Player
{
    public enum WeaponType
    {
        Remote,
        Melee
    }
    public class WeaponData
    {
        public GameObject obj;
        public string weaponName;
        public int damage;
        public bool isFilling;
        public Transform owner;
        public Transform firePoint;
        public GameObject prefab;
    }
}