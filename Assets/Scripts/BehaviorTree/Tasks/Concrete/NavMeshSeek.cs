using UnityEngine;

//using Runtime.LegacyEnemy;


namespace BehaviorTree.Tasks.Concrete
{
    public class NavMeshSeek : ActionTask
    {
        //[SerializeField] private EnemyController controller;


        //�ǐ�
        public override TaskStatus OnUpdate()
        {
            //bool isMoved = controller.MoveToTarget();

            //if (isMoved) return TaskStatus.Running;
            return TaskStatus.Success;
        }

    }
}
