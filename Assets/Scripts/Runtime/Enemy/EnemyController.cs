using UnityEngine;

using UniRx;

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


        //�R���|�[�l���g
        [SerializeField] protected EnemyHealth health;
        [SerializeField] protected EnemyHate hate;
        [SerializeField] private EnemyParameter _parameter;
        public EnemyParameter parameter => _parameter;


        //���S�t���O
        private ReactiveProperty<bool> _isDeathProperty;
        public IReadOnlyReactiveProperty<bool> isDeathProperty => _isDeathProperty;


        //������
        protected void OnEnable()
        {
            stateMachine.Reset();
            parameter.Initialize(0);
            health.Initialize();
            hate.Initialize();
            _isDeathProperty = new ReactiveProperty<bool>(false);
        }

        //Property�j��
        protected void OnDisable()
        {
            //���S�t���Oproperty��Dispose
            _isDeathProperty.Dispose();

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
        public void Damage(float damage, Vector3 knock, float hate, GameObject cause)
        {
            health.SetDamage(damage);
            this.hate.AddHate(hate, cause);

            //���S����
            _isDeathProperty.Value = isDeath();
            if (isDeathProperty.Value)
            {
                blackBoard.SetValue<bool>("Death", true);
            }
            else
            {
                blackBoard.SetValue<Vector3>("Damaged", knock);
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
