using UnityEngine;
using UnityEngine.AI;



namespace Runtime.Enemy.Component
{
    public class EnemySeek : MonoBehaviour
    {
        //�R���|�[�l���g
        [SerializeField] private EnemyMove move;
        [SerializeField] private EnemyLook look;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private NavMeshObstacle obstacle;
        [SerializeField] private new Rigidbody rigidbody;

        //�p�����[�^
        [SerializeField] private float fixedAgentDistance;
        [SerializeField] private float stoppingAgentDistance;
        [SerializeField] private float stoppingVelocity;

        //�t�B�[���h
        private Transform target;

        //�t���O
        public bool isSeeking { get; private set; }
        public bool isFailed { get; private set; }

        
        //�O������ύX�\
        public float stoppingDistance { set { agent.stoppingDistance = value - fixedAgentDistance; } }



        //������
        private void Start()
        {
            agent.isStopped = true;
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.enabled = false;

            obstacle.enabled = true;

            isSeeking = false;
            isFailed = false;
        }



        //�ǔ��J�n
        public void StartSeek(Transform target)
        {
            //�^�[�Q�b�g���X�V
            this.target = target;

            //�t���O��������
            isSeeking = true;
            isFailed = false;

            //�e�R���|�[�l���g��L����
            move.enabled = true;
            obstacle.enabled = false;
            agent.enabled = true;
            agent.isStopped = false;
        }
        //�I��
        public void EndSeek()
        {
            //���s����Ă����ꍇ�̂ݏ���
            if(isSeeking == true)
            {
                //�t���O�𖳌���
                isSeeking = false;

                //�e�R���|�[�l���g�𖳌���
                move.enabled = false;
                agent.isStopped = true;
                agent.enabled = false;
                obstacle.enabled = true;
            }
        }



        private void Update()
        {
            if (!isSeeking) return;

            //�ړI�n�o�^
            agent.SetDestination(target.position);

            //���ǂ蒅���邩����(pending����ɔ���)
            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                isFailed = true;
                EndSeek();
                return;
            }

            //�p�X�̌v�Z���I�����Ă��邩
            if (agent.pathPending == true) return;


            //�������Ԃ��̂ŕʂŒ���
            /*
            //agent�̏I���𔻒�
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //agent�Ƃ̋����𑪒�
                if (Vector3.Distance(agent.nextPosition, transform.position) < stoppingAgentDistance)
                {
                    //���x�����ȉ��ɂȂ�����I��
                    if (rigidbody.velocity.magnitude <= stoppingVelocity)
                    {
                        EndSeek();
                        return;
                    }
                }

                //agent�ɋ߂Â�
                else
                {
                    //�ړ��Ɖ�]����
                    move.MoveByWorldDir(agent.nextPosition - transform.position);
                    look.Look(agent.nextPosition);
                }
            }
            */

            //agent���W����苗��,�s������������
            Vector3 direction = agent.nextPosition - transform.position;
            direction = direction.normalized * fixedAgentDistance;
            agent.nextPosition = transform.position + direction;


            //Y�������ŋ����𔻒�
            if (transform.position.DistanceIgnoreY(target.position) < agent.stoppingDistance + fixedAgentDistance)
            {
                //���x�����ȉ��ɂȂ�����I��
                if (rigidbody.velocity.magnitude <= stoppingVelocity)
                {
                    EndSeek();
                    return;
                }
            }

            //�������͂Ȃ�Ă�������s��������
            else
            {
                //�ړ��Ɖ�]����
                move.MoveByWorldDir(agent.nextPosition - transform.position);
                look.Look(agent.nextPosition);
            }
        }


    }
}

