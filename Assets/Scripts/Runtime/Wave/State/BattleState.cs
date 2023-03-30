using UnityEngine;

using Runtime.Enemy;
using Runtime.Enemy.Component;

using StateMachines;
using StateMachines.BlackBoards;


namespace Runtime.Wave.State
{
    public class BattleState : ParentStateBase<WaveManager>
    {
        //�R���|�[�l���g
        private GameObject player;
        private EnemyHealth towerHealth;
        private EnemyManager manager;

        //�^���[�w���X

        public BattleState(WaveManager manager, IBlackBoard blackBoard) : base(manager, blackBoard) 
        {
            //tower��player�̃C���X�^���X���擾
            towerHealth = GameObject.FindGameObjectWithTag("Tower").GetComponent<EnemyHealth>();

            this.manager = owner.GetComponent<EnemyManager>();
        }


        protected override void SelfStart()
        {
            blackBoard.SetValue<bool>("EndWave", false);
        }
        protected override void SelfEnd()
        {
            blackBoard.SetValue<bool>("Wait", true);
        }

        protected override void SelfUpdate()
        {
            //player��tower�ǂ��炩�����񂾂�Q�[���I�[�o�[��
            if (towerHealth.currentHealth.Value == 0) blackBoard.SetValue<bool>("GameOver", true);

            //bossState���I��������EndWave
            if(innerStateMachine.currentState is BossState && !blackBoard.GetValue<bool>("Boss"))
            {
                blackBoard.SetValue<bool>("EndWave", true);

                //�����Ă�G���������I�Ɏ��S������
                foreach (EnemyController enemy in manager.livingEnemyList)
                {
                    enemy.blackBoardWriter.SetValue<bool>("Death", true);
                }
            }
        }


    }
}
