using UnityEngine;



namespace Enemy.Parameter
{
    public class EnemyParameter : MonoBehaviour
    {
        //�R���|�[�l���g�֘A
        [SerializeField] private EnemyData data;
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyHate _hate;

        //�A�N�Z�b�T
        public EnemyHealth health => _health;
        public EnemyHate hate => _hate;


        //�p�����[�^�Q
        public int level { get; private set; }      //���x��(wave�ˑ��ŏ������֐�����)
        public float maxHealth                      //�ő�Hp
        {
            get { return data.maxHealth * (((float)level * data.growth) / 10); }
        }
        public float attack                         //�U���p�����[�^
        {
            get { return data.attack * (((float)level * data.growth) / 10); }
        }
        public float speed                          //�U�����x
        {
            get { return data.speed * (((float)level * data.growth) / 100); }
        }
        public float growth                         //�����␳
        {
            get { return data.growth; }
        }



        //�p�����[�^�̏������������s��
        private void Awake()
        {
            health.Initialize(maxHealth);
        }
    }
}
