using UnityEngine;

namespace Player
{
    /// <summary>
    /// ˳�������Ӧ
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
        
        private Vector3 cross; //����жϽ�ɫ����
        private float rotZ; //������ת�Ƕ�
        private Transform firePoint; //��ǰ���������
        private Transform weaponPoint; //�������е�
        private Face currentFace; //��ɫ��ǰ����
        //��ұ��������������л�
        private WeaponData[] weaponBag = new WeaponData[2];
        //��ǰ�����±�
        private int currentWeaponIndex;
        private Transform currentWeapon;

        private WeaponAttack attack;
        //TODO: ���ѡ������
        [Header("---��������:��ʱֻ��ѡ��������,�Ҳ����ظ�---")]
        public PlayerWeaponType[] wpTypes = new PlayerWeaponType[2];
        
        private void Awake()
        {
            weaponPoint = CodeHelper.FindChild(transform, "WeaponPoint");
            //TODO: ��ʼ������
            weaponBag[0] = LoadGameData.GetWeaponData((int)wpTypes[0]);
            weaponBag[1] = LoadGameData.GetWeaponData((int)wpTypes[1]);           
            //ʵ��������
            foreach (WeaponData wp in weaponBag)
            {
                wp.obj = GameObjectPool.instance.CreateObject(wp.weaponName,
                   wp.prefab,weaponPoint.position, Quaternion.identity
                    );
                //��Ч���ӵ�Ԥ����
                wp.obj.transform.SetParent(weaponPoint);
                wp.firePoint = CodeHelper.FindChild(wp.obj.transform, "FirePoint");
                wp.owner = transform;
                wp.obj.GetComponentInChildren<WeaponAttack>().Data = wp;
            }
            //Ĭ�ϵ�ǰ����Ϊ������һ��
            currentWeapon = weaponBag[0].obj.transform;
            attack = currentWeapon.GetComponentInChildren<WeaponAttack>();
            firePoint = weaponBag[0].firePoint;

            weaponBag[1].obj.SetActive(false);
        }

        #region �ı䷽��
        /// <summary>
        /// ��ת����
        /// </summary>
        /// <param name="cursorPos"></param>
        public void RotatePlayer(Vector3 cursorPos)
        {
            Vector3 direction = cursorPos - firePoint.position;
            //���������׼����ת
            rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //׼�ǵ������λ�õĲ�ˣ��ж�����ת��,������ʹ��direction����˽�Сʱ������ת��
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
        /// �ı��ɫ����
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
        /// �л�����
        /// </summary>
        /// <param name="bagIndex"></param>
        public void SwitchWeapon(int bagIndex)
        {
            if (bagIndex > 1 || bagIndex < 0 || currentWeaponIndex == bagIndex)
                return;
            //��ǰ��������
            currentWeapon.gameObject.SetActive(false);
            //�µ��±�
            currentWeaponIndex = bagIndex;
            //�µ���������
            currentWeapon = weaponBag[currentWeaponIndex].obj.transform;
            //��ʾ����
            currentWeapon.gameObject.SetActive(true);
            //��������¸�ֵ
            firePoint = weaponBag[currentWeaponIndex].firePoint;
            //�����������¸�ֵ
            attack = currentWeapon.GetComponentInChildren<WeaponAttack>();
        }
        //TODO: ����ʱ�л�����
        public void GetNewWeapon()
        {

        }

        public void Attak()
        {
            attack.Attack();
        }


    }
}
