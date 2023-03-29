using UnityEngine;

using Data.Enemy;


namespace Runtime.Enemy.Parameter
{
    public class EnemyParameter : MonoBehaviour
    {
        //���ʒ萔
        public static float sprintMultiply { get; } = 1.2f;


        //�R���|�[�l���g�֘A
        [SerializeField] private EnemyData data;


        //�p�����[�^�Q
        public int level { get; private set; }      //���x��(wave�ˑ��ŏ������֐�����)
        public float maxHealth                      //�ő�Hp
        {
            get { return data.maxHealth * (1 + (((float)level * data.growth) / 10)); }
        }
        public float attack                         //�U���p�����[�^
        {
            get { return data.attack * (1 + (((float)level * data.growth) / 10)); }
        }
        public float speed                          //�U�����x
        {
            get { return data.speed; }
        }
        public float poise
        {
            get { return data.poise; }
        }
        public float exp                            //�o���l
        {
            get { return data.exp * (1 + (((float)level * data.growth) / 10)); }
        }
        public float growth                         //�����␳
        {
            get { return data.growth; }
        }


        //������
        public void Initialize(int level)
        {
            this.level = level;
        }
    }
}
