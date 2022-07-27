using UnityEngine;

namespace Player
{
    /// <summary>
    /// 顺序与表格对应
    /// </summary>
    public enum PlayerWeaponType
    {
        Magnum = 1,
        M4A1 = 2,
        ShotGun = 3,    
        Katana = 101,
    }


    public class WeaponSystem : MonoBehaviour
    {
        
        private Vector3 cross; //叉乘判断角色朝向
        private float rotZ; //武器旋转角度
        private Transform firePoint; //当前武器发射点
        private Transform weaponPoint; //武器持有点
        private Face currentFace; //角色当前朝向
        //玩家背包，两把武器切换
        private WeaponData[] weaponBag = new WeaponData[2];
        //当前武器下标
        private int currentWeaponIndex;
        private Transform currentWeapon;

        private WeaponAttack attack;
        //TODO: 如何选择武器
        [Header("---武器类型:暂时只能选两把武器,且不能重复---")]
        public PlayerWeaponType[] wpTypes = new PlayerWeaponType[2];
        
        private void Awake()
        {
            weaponPoint = CodeHelper.FindChild(transform, "WeaponPoint");
            //TODO: 初始化武器
            weaponBag[0] = LoadGameData.GetWeaponData((int)wpTypes[0]);
            weaponBag[1] = LoadGameData.GetWeaponData((int)wpTypes[1]);           
            //实例化武器
            foreach (WeaponData wp in weaponBag)
            {
                wp.obj = GameObjectPool.instance.CreateObject(wp.weaponName,
                   wp.prefab,weaponPoint.position, Quaternion.identity
                    );
                //特效，子弹预制体
                wp.obj.transform.SetParent(weaponPoint);
                wp.firePoint = CodeHelper.FindChild(wp.obj.transform, "FirePoint");
                wp.owner = transform;
                wp.obj.GetComponentInChildren<WeaponAttack>().Data = wp;
            }
            //默认当前武器为背包第一把
            currentWeapon = weaponBag[0].obj.transform;
            attack = currentWeapon.GetComponentInChildren<WeaponAttack>();
            firePoint = weaponBag[0].firePoint;

            weaponBag[1].obj.SetActive(false);
        }

        #region 改变方向
        /// <summary>
        /// 旋转武器
        /// </summary>
        /// <param name="cursorPos"></param>
        public void RotatePlayer(Vector3 cursorPos)
        {
            Vector3 direction = cursorPos - firePoint.position;
            //武器点跟随准星旋转
            rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //准星点与玩家位置的叉乘，判断人物转向,不可以使用direction，叉乘较小时，无限转向
            cross = Vector3.Cross((cursorPos - transform.position).normalized, transform.up);

            if (cross.z < 0)
            {
                ChangePlayerFace(Face.Left);
            }
            else if (cross.z >0)
            {
                ChangePlayerFace(Face.Right);
            }
        }    
        /// <summary>
        /// 改变角色朝向
        /// </summary>
        /// <param name="dir"></param>
        private void ChangePlayerFace(Face dir)
        {
            switch (dir)
            { 
                case Face.Right:
                    if (currentFace == Face.Left)
                    {  
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        currentFace = Face.Right;
                    }
                    if (!weaponBag[currentWeaponIndex].isFilling)
                        currentWeapon.rotation = Quaternion.Euler(0, 0, rotZ);
                    break;

                case Face.Left:
                    if (currentFace == Face.Right)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        currentFace = Face.Left;
                    }
                    if (!weaponBag[currentWeaponIndex].isFilling)
                        currentWeapon.rotation = Quaternion.Euler(180, 0, -rotZ);
                    break;
            }
        }

        #endregion
        /// <summary>
        /// 切换武器
        /// </summary>
        /// <param name="bagIndex"></param>
        public void SwitchWeapon(int bagIndex)
        {
            if (bagIndex > 1 || bagIndex < 0 || currentWeaponIndex == bagIndex)
                return;
            //当前武器禁用
            currentWeapon.gameObject.SetActive(false);
            //新的下标
            currentWeaponIndex = bagIndex;
            //新的武器物体
            currentWeapon = weaponBag[currentWeaponIndex].obj.transform;
            //显示武器
            currentWeapon.gameObject.SetActive(true);
            //开火点重新赋值
            firePoint = weaponBag[currentWeaponIndex].firePoint;
            //攻击对象重新赋值
            attack = currentWeapon.GetComponentInChildren<WeaponAttack>();
        }
        //TODO: 运行时切换武器
        public void GetNewWeapon()
        {

        }

        public void Attak()
        {
            attack.Attack();
        }


    }
}
