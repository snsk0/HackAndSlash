using UnityEngine;


namespace Runtime.Enemy.Component
{
    public abstract class EnemyAttack : MonoBehaviour
    {



        //�U���p���\�b�h
        public abstract float AttackToTarget(GameObject target, int index);


        //�L�����Z���p
        public virtual void CancellAtack()
        {

        }
    }
}
