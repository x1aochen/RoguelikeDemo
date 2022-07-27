using UnityEngine;

namespace Player
{
    /// <summary>
    /// �ɴ�����Ϊ��׼������һ��Ƭ�ӵ�,���ٶȲ�һ��
    /// </summary>
    public class BulletYellowRain : Bullet
    {
        private GameObject childPrefab;
        //����������
        public int number = 6;
        //�������
        public float bulletSpacing = 1.5f;

        private void Awake()
        {
            childPrefab = Resources.Load<GameObject>("Bullet/BulletYellow");
        }

        public override void Reset(RemoteResetArgs args)
        {
            base.Reset(args);
            LaunchBullet();
        }

        private void LaunchBullet()
        {
            Vector3 originPos = transform.position + bulletSpacing * (number / 2) * transform.up;

            for (int i = 0;i < number; i++)
            {
                RemoteResetArgs arg = new RemoteResetArgs(CodeHelper.Random(args.Speed - i - 1, args.Speed),
                    CodeHelper.Random(args.Range, args.Range + i + 2), args.TargetTag, args.action);
                GameObject go = GameObjectPool.instance.CreateObject("BulletYellow", childPrefab, originPos - (i * bulletSpacing * transform.up),transform.rotation);
                go.GetComponent<IReset>().Reset(arg);
            }

            GameObjectPool.instance.RecycleObject(gameObject);
        }
        

    }
}