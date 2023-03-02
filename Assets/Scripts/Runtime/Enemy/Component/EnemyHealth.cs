using UnityEngine;

using Runtime.Enemy.Parameter;


namespace Runtime.Enemy.Component
{
    public class EnemyHealth : MonoBehaviour
    {
        //�p�����[�^
        [SerializeField] private EnemyParameter parameter;


        public float maxHealth => parameter.maxHealth;      //�ő�Hp�Q��
        public float currentHealth { get; private set; }    //����Hp



        //Hp�v�Z
        public virtual float Damage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            else if (currentHealth < 0) currentHealth = 0;

            return damage;
        }




        //Hp�̏�����
        private void OnEnable()
        {
            currentHealth = maxHealth;
        }
    }
}
