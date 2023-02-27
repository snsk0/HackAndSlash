using UnityEngine;
using UniRx;


namespace Enemy.Parameter
{
    //�ő�HP����ScriptableObject�̈Ӗ������܂�Ȃ���Health�͕ʃR���|�[�l���g�Ŏg�����ߖڂ��ނ�
    public class EnemyHealth : MonoBehaviour, IDamagable
    {
        //�t�B�[���h
        public float maxHealth { get; private set; }   //�ő�Hp
        private ReactiveProperty<float> _healthProperty;
        public IReadOnlyReactiveProperty<float> healthProperty => _healthProperty;
        public float currentHealth => healthProperty.Value;




        //�������֐�(�O������maxHealth��ݒ�ł���悤��)
        public void Initialize(float maxHealth)
        {
            this.maxHealth = maxHealth;
            _healthProperty = new ReactiveProperty<float>(maxHealth);  //ReactiveProperty������
        }


        //�_���[�W�֐�
        public void Damage(float damage, GameObject cause)
        {
            _healthProperty.Value -= damage;   //�_���[�W
        }



        //���������ꂽ��ReactiveProperty�𖳌���
        public void OnDisable()
        {
            _healthProperty.Dispose();
        }
    }
}
