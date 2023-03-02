using UnityEngine;
using UnityEngine.AI;



namespace Runtime.Enemy.Component
{
    public class EnemyMove : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        //�ݒ荀��
        [SerializeField] private float rangeByTarget;          //�^�[�Q�b�g�ɂǂ��܂ŋ߂Â���
        [SerializeField] private bool canRaycast;              //��Q��������Ƃ��Ɉړ����I�����邩        

        //�ړ��ς݃t���O
        private bool moved;



        //�ړ��֐�
        public bool MoveToTarget(Vector3 targetPosition)
        {
            //�͈̓`�F�b�N
            if (Vector3.Distance(agent.transform.position, targetPosition) < rangeByTarget)
            {
                //rayCast�ɂ��`�F�b�N���s��Ȃ��ꍇ�I��
                if (!canRaycast) return false;

                //TODO Raycast�ɂ���Q���̃`�F�b�N
            }


            //agent��ݒ�
            moved = true;
            agent.isStopped = false;
            agent.SetDestination(targetPosition);
            return true;
        }




        //�t���O������
        private void OnEnable()
        {
            moved = false;
            agent.isStopped = false;
        }

        //�t���O�X�V
        private void Update()
        {
            if (moved) moved = false;
            else agent.isStopped = true;
        }
    }

}