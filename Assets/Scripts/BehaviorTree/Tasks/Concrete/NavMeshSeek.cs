using UnityEngine;
using UnityEngine.AI;

using Enemy;

namespace BehaviorTree.Tasks.Concrete
{
    public class NavMeshSeek : ActionTask
    {
        private NavMeshAgent agent;
        [SerializeField] private Transform target;
        [SerializeField] private float endDistance;

        [SerializeField] private EnemyController controller;

        //������
        public override void OnAwake()
        {
            agent = owner.GetComponent<NavMeshAgent>();
        }


        public override void OnStart()
        {
            agent.isStopped = false;
        }

        //�ǐ�
        public override TaskStatus OnUpdate()
        {
            /*
            agent.SetDestination(target.position);

            if (agent.pathPending | agent.remainingDistance > endDistance) return TaskStatus.Running;
            else
            {
                agent.isStopped = true;
                return TaskStatus.Success;
            }*/

            controller.MoveToTarget();
            return TaskStatus.Running;
        }

    }
}
