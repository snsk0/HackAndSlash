using StateMachines;
using StateMachines.BlackBoards;

using Runtime.Enemy;
using Runtime.Enemy.Component;

namespace Runtime.Wave.State
{
    public class BossState : StateBase<WaveManager>
    {
        //�R���|�[�l���g
        private IEnemyGenerator generator;
        private EnemyHealth health;


        //�R���X�g���N�^
        public BossState(WaveManager manager, IBlackBoard blackBoard) : base(manager, blackBoard)
        {
            foreach(IEnemyGenerator generator in owner.GetComponents<IEnemyGenerator>())
            {
                if (generator.enemyType == EnemyType.Golem) this.generator = generator;
            }
        }



        public override void Start()
        {
            //�{�X���X�|�[��������
            health =  generator.Generate(owner.transform).GetComponent<EnemyHealth>();
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
