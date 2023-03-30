using StateMachines;
using StateMachines.BlackBoards;

using Runtime.Enemy;
using Runtime.Enemy.Component;

namespace Runtime.Wave.State
{
    public class BossState : StateBase<WaveManager>
    {
        //�R���|�[�l���g
        private EnemyManager manager;
        private EnemyHealth health;

        public EnemyType type { private get; set; }


        //�R���X�g���N�^
        public BossState(WaveManager manager, IBlackBoard blackBoard) : base(manager, blackBoard)
        {
            this.manager = owner.GetComponent<EnemyManager>();
        }



        public override void Start()
        {
            //�{�X���X�|�[��������
            EnemyController controller = manager.GetInitialEnemy(type);
            health = controller.GetComponent<EnemyHealth>();
        }

        public override void Update()
        {
            //�{�X�������Ă邩�ǂ����A���񂾂�ȍ~
            if(health.currentHealth.Value == 0)
            {
                blackBoard.SetValue<bool>("Boss", false);
            }
        }

    }
}
