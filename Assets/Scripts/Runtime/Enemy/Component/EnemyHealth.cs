using UnityEngine;

using UniRx;

using Runtime.Enemy.Parameter;



namespace Runtime.Enemy.Component
{
    public class EnemyHealth : MonoBehaviour
    {
        //�R���|�[�l���g
        [SerializeField] private EnemyParameter parameter;


        //�t�B�[���h
        public float maxHealth => parameter.maxHealth;
        private ReactiveProperty<float> _currentHealth = new ReactiveProperty<float>();
        public IReactiveProperty<float> currentHealth => _currentHealth;



        //������
        public void Initialize()
        {
            _currentHealth.Value = maxHealth;
        }


        //�_���[�W
        public void SetDamage(float damage)
        {
            //Hp�v�Z
            float health = _currentHealth.Value - damage;
            if (health < 0) health = 0;

            //�l�̍X�V
            _currentHealth.Value = health;
        }


        //Dispose
        private void OnDestroy()
        {
            _currentHealth.Dispose();
        }
    }
}
