using System;

using UnityEngine;

using Runtime.Enemy.Parameter;

namespace Runtime.Enemy.Component
{
    public class EnemyItemDropper : MonoBehaviour
    {
        //[SerializeField] private ExpOrb dropItem;
        [SerializeField] private Vector3 offset;
        [SerializeField] private EnemyParameter parameter;


        //�Ăяo���֐�
        private Action<Vector3, int> generateExp;


        //������
        public void initialize(Action<Vector3, int> generateExp)
        {
            this.generateExp = generateExp;
        }


        //�Ăяo��
        public void Drop()
        {
            generateExp.Invoke(transform.position + offset, (int)parameter.exp);
        }
    }
}
