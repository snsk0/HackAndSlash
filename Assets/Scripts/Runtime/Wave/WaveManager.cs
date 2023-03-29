using UnityEngine;

using StateMachines;
using StateMachines.BlackBoards;

using Runtime.Enemy;

using Runtime.Wave.State;

namespace Runtime.Wave
{
    public class WaveManager : MonoBehaviour
    {
        //�X�e�[�g�}�V���ƃu���b�N�{�[�h
        public StateMachine<WaveManager> stateMachine { get; private set; }
        private IBlackBoard blackBoard;

        //wave��
        public int wave { get; set; }


        //������
        private void Awake()
        {
            //�X�e�[�g�}�V���֘A�̐���
            blackBoard = new BlackBoard();
            stateMachine = new StateMachine<WaveManager>(this, blackBoard);

            //BattleState�̍\�z
            BattleState battle = new BattleState(this, blackBoard);
            MainState main = new MainState(this, blackBoard);
            BossState boss = new BossState(this, blackBoard);

            //�ϐ��̐ݒ�
            main.waveTime = 30.0f;
            main.generateSpanTime = 10.0f;
            main.generateNumber = 3;
            main.enemiesNumber[(int)EnemyType.Taunt] = 1;

            //�J�ڂ̐ݒ�
            battle.innerStateMachine.AddTransition(main, boss, blackBoard => { return blackBoard.GetValue<bool>("Boss") == true; });

            //�u���b�N�{�[�h�̏�����
            blackBoard.SetValue<bool>("Boss", false);

            //������
            battle.innerStateMachine.Initialize(main);



            //�X�e�[�g�̐���
            WaitState wait = new WaitState(this, blackBoard);
            GameOverState gameOver = new GameOverState(this, blackBoard);

            //�e�X�e�[�g�̕ϐ��ݒ�
            wait.waitTime = 5.0f;

            //�J�ڂ�ݒ�
            stateMachine.AddTransition(wait, battle, blackBoard => { return blackBoard.GetValue<bool>("Wait") == false; });
            stateMachine.AddTransition(battle, wait, blackBoard => { return blackBoard.GetValue<bool>("EndWave") == true; });
            stateMachine.AddTransition(battle, gameOver, blackBoard => { return blackBoard.GetValue<bool>("GameOver") == true; });

            //�u���b�N�{�[�h�̏�����
            blackBoard.SetValue<bool>("Wait", true);
            blackBoard.SetValue<bool>("EndWave", false);
            blackBoard.SetValue<bool>("GameOver", false);

            //������
            stateMachine.Initialize(wait);
        }


        private void Update()
        {
            stateMachine.Tick();
        }
    }
}
