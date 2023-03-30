using UnityEngine;

using StateMachines;
using StateMachines.BlackBoards;

using Runtime.Enemy.Parameter;
using Runtime.Enemy.Component;



namespace Runtime.Enemy
{
    public abstract class EnemyController : MonoBehaviour, IDamagable
    {
        //�X�e�[�g�}�V���֘A
        protected StateMachine<EnemyController> stateMachine;
        protected IBlackBoard blackBoard = new BlackBoard();
        public StateBase<EnemyController> currentState => stateMachine.currentState;
        public IWriteOnlyBlackBoard blackBoardWriter => blackBoard;


        //�R���|�[�l���g
        [SerializeField] protected EnemyHealth health;
        [SerializeField] protected EnemyHate hate;
        [SerializeField] private EnemyParameter _parameter;
        public EnemyParameter parameter => _parameter;




        //������
        protected void OnEnable()
        {
            stateMachine.Reset();
            parameter.Initialize(0);
            health.Initialize();
            hate.Initialize();
        }

        //Property�j��
        protected void OnDisable()
        {
            //�O����A�N�Z�X����blackBoard��value�����������Ă���
            blackBoard.SetValue<Vector3>("Damaged", Vector3.zero);
            blackBoard.SetValue<bool>("Death", false);
        }


        //�X�e�[�g�}�V���X�V
        protected void Update()
        {
            stateMachine.Tick();
        }


        //�_���[�W�֐�
        public void Damage(float damage, float knock, float hate, GameObject cause)
        {
            health.SetDamage(damage);
            this.hate.AddHate(hate, cause);

            Vector3 dir = cause.transform.position - transform.position;
            dir *= knock;

            //���S����
            if (isDeath())
            {
                blackBoard.SetValue<bool>("Death", true);
            }
            else
            {
                blackBoard.SetValue<Vector3>("Damaged", dir);
            }
        }



        //���S����
        protected virtual bool isDeath()
        {
            if(health.currentHealth.Value <= 0)
            {
                return true;
            }
            return false;
        }


    }
}
