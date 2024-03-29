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


        //呼び出す関数
        private Action<Vector3, int> generateExp;


        //初期化
        public void initialize(Action<Vector3, int> generateExp)
        {
            this.generateExp = generateExp;
        }


        //呼び出し
        public void Drop()
        {
            generateExp.Invoke(transform.position + offset, (int)parameter.exp);
        }
    }
}
