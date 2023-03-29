using UnityEngine;
using UnityEngine.AI;

using Runtime.Enemy.Component;

using StateMachines;
using StateMachines.BlackBoards;



namespace Runtime.Enemy.State
{
    public class SeekState : StateBase<EnemyController>
    {
        //�ݒ�ϐ�
        public float minVelocity { private get; set; }
        public float maxAgentDistance { private get; set; }
        public float followAgentDistance { private get; set; }
        public bool isFixedAgentDistance { private get; set; } = false;

        //private�t�B�[���h
        private NavMeshAgent agent;
        private Rigidbody rigidbody;
        private EnemyLook look;
        private ITargetProvider targetProvider;
        private GameObject target;
        private EnemyMove move;
        private LineRenderer renderer;
        private NavMeshObstacle obstacle;


        //�R���X�g���N�^
        public SeekState(EnemyController owner, IBlackBoard blackBoard) : base(owner, blackBoard)
        {
            targetProvider = owner.GetComponent<ITargetProvider>();
            look = owner.GetComponent<EnemyLook>();
            agent = owner.GetComponent<NavMeshAgent>();
            rigidbody = owner.GetComponent<Rigidbody>();
            move = owner.GetComponent<EnemyMove>();
            renderer = owner.GetComponent<LineRenderer>();
            obstacle = owner.GetComponent<NavMeshObstacle>();


            //agent�̓�����؂�
            //agent.updatePosition = false;
            //agent.updateRotation = false;
            //agent.updateUpAxis = false;
            agent.isStopped = true;

        }




        //������
        public override void Start()
        {
            move.enabled = false;
            target = targetProvider.target.Value;
            agent.isStopped = false;
            obstacle.enabled = false;
        }

        //�I��
        public override void End()
        {
            agent.isStopped = true;
            move.enabled = false;
            //obstacle.enable = true;
        }


        public override void Update()
        {
            //�ړI�n�̍X�V
            agent.SetDestination(target.transform.position);

            //�p�X�̌v�Z�������ł��Ă���ꍇ
            if (agent.pathPending == true) return;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {

            }

            renderer.SetPositions(agent.path.corners);
            /*
            //��������
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //���x�𔻒�
                if (agent.velocity.magnitude <= 0)
                {
                    blackBoard.SetValue<bool>("Seek", false);
                }
            }
            else
            {
                owner.transform.position = agent.nextPosition;
                look.Look(agent.steeringTarget);
            }
            */
            


            //agent.velocity = rigidbody.velocity;

            //agent���W����苗���ɕ␳����(���ɕۂƌ������Ɉړ����Ȃ�)
            //�������� agent�Ƃ̋��������ɂ���(maxAgentDistance�ɌŒ肷�邩�ǂ���
            /*
            Vector3 direction = agent.nextPosition - owner.transform.position;
            direction = direction.normalized * maxAgentDistance;
            agent.nextPosition = owner.transform.position + direction;


            //�����̔���(agent)�ƁA�Ǐ]�����̔���
            if (agent.remainingDistance <= agent.stoppingDistance - maxAgentDistance)
            {
                //���x�𔻒�
                if (rigidbody.velocity.magnitude <= minVelocity)
                {
                    blackBoard.SetValue<bool>("Seek", false);
                }
            }
            else
            {
                //�ړ��Ɖ�]����
                move.MoveByWorldDir(agent.nextPosition - owner.transform.position);
                look.Look(agent.steeringTarget);
            }
            */
        }


    }
}
