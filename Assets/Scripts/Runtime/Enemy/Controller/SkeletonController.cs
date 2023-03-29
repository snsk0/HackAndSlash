using UnityEngine;

using StateMachines;
using StateMachines.BlackBoards;

using Runtime.Enemy.Animation;
using Runtime.Enemy.Component;


namespace Runtime.Enemy.Controller
{
    /*
    public class SkeletonController : EnemyController
    {
        [SerializeField] private GameObject player;
        [SerializeField] private EnemyHate hate;

        //������
        private void OnEnable()
        {
            blackBoard = new BlackBoard();
            stateMachine = new StateMachine<EnemyController>(this, blackBoard);


            hate.AddHate(100f, player);




            //�X�e�[�g�}�V���\�z

            //battleMove�Z�b�g�A�b�v
            BattleMove battleMove = new BattleMove(this, blackBoard);
            Idle idle = new Idle(this, blackBoard);
            RandomWalk randomWalk = new RandomWalk(this, blackBoard);
            FrontWalk frontWalk = new FrontWalk(this, blackBoard);
            BackWalk backWalk = new BackWalk(this, blackBoard);
            Taunt taunt = new Taunt(this, blackBoard);


            battleMove.range = 4.5f;
            battleMove.attackCoolTime = 3.5f;
            idle.minWaitTime = 0.3f;
            idle.maxWaitTime = 1.5f;
            idle.farRange = 3.8f;
            idle.nearRange = 2.2f;
            randomWalk.minWalkTime = 2f;
            randomWalk.maxWalkTime = 3f;
            randomWalk.verticalWalkTime = 0.4f;
            randomWalk.tauntRandom = 0f;
            randomWalk.avoidRange = 2.0f;
            frontWalk.range = 3.0f;
            backWalk.range = 3.0f;


            battleMove.innerStateMachine.AddTransition(idle, randomWalk, blackBoard => { return (blackBoard.GetValue<bool>("RandomWalk") == true); });
            battleMove.innerStateMachine.AddTransition(randomWalk, taunt, blackBoard => { return (blackBoard.GetValue<bool>("Taunt") == true); });  //taunt�ւ̑J�ڂ�idle���D��
            battleMove.innerStateMachine.AddTransition(randomWalk, backWalk, blackBoard => { return (blackBoard.GetValue<bool>("BackWalk") == true); }); //back��idle���D��
            battleMove.innerStateMachine.AddTransition(randomWalk, idle, blackBoard => { return (blackBoard.GetValue<bool>("RandomWalk") == false); });
            battleMove.innerStateMachine.AddTransition(taunt, idle, blackBoard => { return (blackBoard.GetValue<bool>("Taunt") == false); });
            battleMove.innerStateMachine.AddTransition(idle, frontWalk, blackBoard => { return (blackBoard.GetValue<bool>("FrontWalk") == true); });
            battleMove.innerStateMachine.AddTransition(frontWalk, idle, blackBoard => { return (blackBoard.GetValue<bool>("FrontWalk") == false); });
            battleMove.innerStateMachine.AddTransition(idle, backWalk, blackBoard => { return (blackBoard.GetValue<bool>("BackWalk") == true); });
            battleMove.innerStateMachine.AddTransition(backWalk, idle, blackBoard => { return (blackBoard.GetValue<bool>("BackWalk") == false); });


            battleMove.innerStateMachine.Initialize(idle);

            blackBoard.SetValue<bool>("RandomWalk", false);
            blackBoard.SetValue<bool>("FrontWalk", false);
            blackBoard.SetValue<bool>("BackWalk", false);
            blackBoard.SetValue<bool>("Taunt", false);
            blackBoard.SetValue<bool>("Attack", true);


            //�X�e�[�g���C���X�^���X��
            Seek seek = new Seek(this, blackBoard);
            Attack attack = new Attack(this, blackBoard);


            //�J�ڂ�ݒ�
            stateMachine.AddTransition(battleMove, seek, blackBoard => { return (blackBoard.GetValue<bool>("Seek") == true); });
            stateMachine.AddTransition(seek, battleMove, blackBoard => { return (blackBoard.GetValue<bool>("Seek") == false); });
            stateMachine.AddTransition(battleMove, attack, blackBoard => { return (blackBoard.GetValue<bool>("Attack") == true); });
            stateMachine.AddTransition(attack, battleMove, blackBoard => { return (blackBoard.GetValue<bool>("Attack") == false); });
            //stateMachine.AddTransition(seek, atk, b => { return !b.GetValue<bool>("move"); });
            //stateMachine.AddTransition(atk, seek, b => { return b.GetValue<bool>("move"); });


            //�u���b�N�{�[�h�̏����l��ݒ�(�l���Ȃ��ꍇDefault�l���Ԃ���ĈӐ}���Ȃ����������邱�Ƃ����邽��
            blackBoard.SetValue<bool>("Seek", false);

            stateMachine.Initialize(battleMove);
        }

        private void Update()
        {
            stateMachine.Tick();
        }
    }





    //�ǐՃX�e�[�g
    public class Seek : StateBase<EnemyController>
    {
        //�K�v�R���|�[�l���g
        private EnemySeek seek;
        private EnemyHate hate;


        //������
        public Seek(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            seek = owner.GetComponent<EnemySeek>();
            hate = owner.GetComponent<EnemyHate>();
        }



        public override void Start()
        {
            //seek�̌Ăяo��
            seek.StartSeek(hate.GetMaxHateObject().transform);
        }


        public override void Update()
        {
            //�ǐՂ��I��������false�ɂ���
            if (!seek.isSeeking) blackBoard.SetValue<bool>("Seek", false);
        }
    }





    //�퓬���̈ړ�
    public class BattleMove : ParentStateBase<EnemyController>
    {
        //�K�v�R���|�[�l���g
        private EnemyHate hate;
        private GameObject target;


        //�p�����[�^
        public float range { private get; set; }
        public float attackCoolTime { private get; set; }
        private float timer;


        //������
        public BattleMove(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            hate = owner.GetComponent<EnemyHate>();
        }


        protected override void SelfStart()
        {
            target = hate.GetMaxHateObject();
            timer = 0;
        }

        protected override void SelfUpdate()
        {
            //�ǐՋ����𔻒�(Y������)
            if (owner.transform.position.GetDistanceIgnoreY(target.transform.position) > range)
            {
                blackBoard.SetValue<bool>("Seek", true);
                return;
            }

            //�U���𔻒�
            timer += Time.deltaTime;
            if (timer > attackCoolTime) blackBoard.SetValue<bool>("Attack", true);
        }
    }





    //�����X�e�[�g
    public class Idle : StateBase<EnemyController>
    {
        //�v���R���|�[�l���g
        private EnemyHate hate;
        private EnemyLook look;
        private EnemyAnimator animator;
        private GameObject target;


        //�p�����[�^
        public float minWaitTime { private get; set; }
        public float maxWaitTime { private get; set; }
        public float farRange { private get; set; }
        public float nearRange { private get; set; }


        //�ҋ@����
        private float waitTime;


        //������
        public Idle(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            hate = owner.GetComponent<EnemyHate>();
            look = owner.GetComponent<EnemyLook>();
            animator = owner.GetComponent<EnemyAnimator>();
        }



        public override void Start()
        {
            //target���擾
            target = hate.GetMaxHateObject();

            //�ҋ@���Ԃ𐶐�
            waitTime = Random.Range(minWaitTime, maxWaitTime);

            animator.PlayIdle();
        }


        public override void Update()
        {
            //�Ώۂ�����
            look.Look(target.transform);

            //��������
            float distance = owner.transform.position.GetDistanceIgnoreY(target.transform.position);

            
            //�����ꍇ
            if(distance > farRange)
            {
                blackBoard.SetValue<bool>("FrontWalk", true);
                return;
            }
            //�߂�
            else if(distance < nearRange)
            {
                blackBoard.SetValue<bool>("BackWalk", true);
                return;
            }


            //�ҋ@���Ԍ���
            waitTime -= Time.deltaTime;

            //�ҋ@���Ԃ��I�������ꍇ
            if(waitTime < 0)
            {
                blackBoard.SetValue<bool>("RandomWalk", true);
            }
        }
    }





    //�����_���ړ�
    public class RandomWalk : StateBase<EnemyController>
    {
        //�R���|�[�l���g
        private Enemy.Component.EnemyMove move;
        private EnemyLook look;
        private EnemyHate hate;
        private GameObject target;


        //�p�����[�^
        public float minWalkTime { private get; set; }
        public float maxWalkTime { private get; set; }
        public float verticalWalkTime { private get; set; }
        public float tauntRandom { private get; set; }
        public float avoidRange { private get; set; }
        private float walkTime;
        private Vector3 direction;
        private bool tauntFlag;


        //������
        public RandomWalk(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            move = owner.GetComponent<Enemy.Component.EnemyMove>();
            look = owner.GetComponent<EnemyLook>();
            hate = owner.GetComponent<EnemyHate>();
        }



        //target�̎擾
        public override void Start()
        {
            target = hate.GetMaxHateObject();

            //���s���Ԃ����߂�
            walkTime = Random.Range(minWalkTime, maxWalkTime);

            //�����ŕ��������߂�
            float ran = Random.Range(0, 4);
            if (ran < 1.0f) direction = Vector3.forward;
            else if (ran < 2.0f) direction = -Vector3.forward;
            else if (ran < 3.0f) direction = Vector3.right;
            else direction = -Vector3.right;

            //�O��̏ꍇ�␳�l��������
            if (direction.z != 0) walkTime *= verticalWalkTime;

            //�Њd�̐�������t���O
            tauntFlag = true;
        }


        public override void Update()
        {
            //�Ώۂ�����
            look.Look(target.transform);

            //�������߂�������͂Ȃ��
            if (owner.transform.position.GetDistanceIgnoreY(target.transform.position) < avoidRange)
            {
                blackBoard.SetValue<bool>("BackWalk", true);
                blackBoard.SetValue<bool>("RandomWalk", false);
                return;
            }

            //�ړ�
            move.Move(direction);

            //�ҋ@���Ԍ���
            walkTime -= Time.deltaTime;

            //�ҋ@���Ԃ��I�������ꍇ
            if (walkTime < 0)
            {
                if(Random.value < tauntRandom && tauntFlag) blackBoard.SetValue<bool>("Taunt", true);
                blackBoard.SetValue<bool>("RandomWalk", false);
                tauntFlag = false;
            }
        }



        //�U���X�e�[�g�ւ̕ύX���u���b�N
        public override bool GuardChangeState(StateBase<EnemyController> nextState)
        {
            if (false) return false;
            return true;
        }
    }





    //�O���ړ�
    public class FrontWalk : StateBase<EnemyController>
    {
        //�R���|�[�l���g
        private Enemy.Component.EnemyMove move;
        private EnemyLook look;
        private EnemyHate hate;

        //�p�����[�^
        public float range { private get; set; }
        private GameObject target;


        //������
        public FrontWalk(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            move = owner.GetComponent<Enemy.Component.EnemyMove>();
            look = owner.GetComponent<EnemyLook>();
            hate = owner.GetComponent<EnemyHate>();
        }



        public override void Start()
        {
            target = hate.GetMaxHateObject();
        }

        public override void Update()
        {
            //�Ώۂ�����
            look.Look(target.transform);

            //��������
            if(owner.transform.position.GetDistanceIgnoreY(target.transform.position) < range)
            {
                //�J��
                blackBoard.SetValue<bool>("FrontWalk", false);
                return;
            }

            //�ړ�
            move.Move(Vector3.forward);
        }
    }




    public class BackWalk : StateBase<EnemyController>
    {
        //�R���|�[�l���g
        private Enemy.Component.EnemyMove move;
        private EnemyLook look;
        private EnemyHate hate;

        //�p�����[�^
        public float range { private get; set; }
        private GameObject target;


        //������
        public BackWalk(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            move = owner.GetComponent<Enemy.Component.EnemyMove>();
            look = owner.GetComponent<EnemyLook>();
            hate = owner.GetComponent<EnemyHate>();
        }



        public override void Start()
        {
            target = hate.GetMaxHateObject();
        }

        public override void Update()
        {
            //�Ώۂ�����
            look.Look(target.transform);

            //��������
            if (owner.transform.position.GetDistanceIgnoreY(target.transform.position) > range)
            {
                //�J��
                blackBoard.SetValue<bool>("BackWalk", false);
                return;
            }

            //�ړ�
            move.Move(-Vector3.forward);
        }
    }




    public class Taunt : StateBase<EnemyController>
    {
        //�R���|�[�l���g
        private EnemyAnimator animator;

        private float waitTime = 1.5f;
        private float deltTime;

        //������
        public Taunt(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            animator = owner.GetComponent<EnemyAnimator>();
        }



        public override void Start()
        {
            animator.PlayAttack(0);
            deltTime = 0;
        }

        public override void Update()
        {
            deltTime += Time.deltaTime;

            //��������
            if (waitTime < deltTime)
            {
                //�J��
                blackBoard.SetValue<bool>("Taunt", false);
                return;
            }
        }
    }





    public class Attack : StateBase<EnemyController>
    {
        //�R���|�[�l���g
        private Runtime.LegacyEnemy.Component.EnemyAttack attack;
        private EnemyHate hate;

        private float time;

        public Attack(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            attack = owner.GetComponent<LegacyEnemy.Component.EnemyAttack>();
            hate = owner.GetComponent<EnemyHate>();
        }



        public override void Start()
        {
            time = 3.0f;
            attack.AttackToTarget(hate.GetMaxHateObject(), 0);
        }

        public override void Update()
        {
            time -= Time.deltaTime;

            if (time < 0) blackBoard.SetValue<bool>("Attack", false);
        }
    }





    public class TargetPlayer : ParentStateBase<EnemyController>
    {
        private EnemyHate hate;

        public TargetPlayer(EnemyController controller, IBlackBoard blackBoard) : base(controller, blackBoard)
        {
            hate = owner.GetComponent<EnemyHate>();
        }



        protected override void SelfUpdate() 
        {
            GameObject target = hate.GetMaxHateObject();

            if (target.tag == "Tower") blackBoard.SetValue<bool>("TowerTarget", true);
        }
    }
    */



}
public static class ExtensionVector3
{
    public static float DistanceIgnoreY(this Vector3 origin, Vector3 target)
    {
        Vector3 distance = (target - origin);
        distance.y = 0;

        return distance.magnitude;
    }
}
