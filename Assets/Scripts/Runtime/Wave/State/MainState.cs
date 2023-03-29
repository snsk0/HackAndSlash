using UnityEngine;

using System;
using System.Linq;

using StateMachines;
using StateMachines.BlackBoards;

using Runtime.Enemy;


namespace Runtime.Wave.State
{
    public class MainState : StateBase<WaveManager>
    {
        //�R���|�[�l���g
        private EnemyManager enemyManager;

        //�ϐ�
        public float waveTime { get; set; }    //�t�F�[�Y�̎���
        public float currentTime { get; private set; }  //���݂̌o�ߎ���
        public float generateSpanTime { private get; set; } //�G�̐����Ԋu
        public int generateNumber { private get; set; } //�G�̓���������
        public int[] enemiesNumber { get; private set; }    //�G�̐�������
        private float spanCounter;  //�����Ԋu�̃J�E���g


        //�R���X�g���N�^
        public MainState(WaveManager manager, IBlackBoard blackBoard) : base(manager, blackBoard)
        {
            enemyManager = owner.GetComponent<EnemyManager>();

            enemiesNumber = new int[Enum.GetValues(typeof(EnemyType)).Cast<int>().Max() + 1];
        }



        public override void Start()
        {
            currentTime = 0;
            spanCounter = 0;
        }

        public override void Update()
        {
            currentTime += Time.deltaTime;
            spanCounter += Time.deltaTime;


            if (currentTime > waveTime)
            {
                blackBoard.SetValue<bool>("Boss", true);
                return;
            }


            //�������Ȃ��Ƃ��͕Ԃ�
            if (!(spanCounter > generateSpanTime)) return;

            //spanCounter��������
            spanCounter = 0;

            //�w��񐔐���
            for (int i = 0; i < generateNumber; i++)
            {
                //��������
                int index = UnityEngine.Random.Range(1, enemiesNumber.Sum() + 1);

                //��������G������
                int valueSum = 0;
                foreach (EnemyType type in Enum.GetValues(typeof(EnemyType)))
                {
                    valueSum += enemiesNumber[(int)type];

                    if (valueSum <= index)
                    {
                        //��������
                        enemyManager.GetInitialEnemy(type);
                        break;
                    }
                }
            }
        }



    }
}
