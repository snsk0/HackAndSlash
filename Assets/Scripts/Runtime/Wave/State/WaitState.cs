using UnityEngine;

using StateMachines;
using StateMachines.BlackBoards;


namespace Runtime.Wave.State
{
    public class WaitState : StateBase<WaveManager>
    {
        //�ϐ�
        public float waitTime { get; set; }
        public float currentTime { get; private set; }


        //�R���X�g���N�^
        public WaitState(WaveManager manager, IBlackBoard blackBoard) : base(manager, blackBoard){}



        public override void Start()
        {
            currentTime = 0;

            owner.wave++;  //wave��i�߂�
        }


        public override void Update()
        {
            currentTime += Time.deltaTime;

            if(currentTime > waitTime)
            {
                blackBoard.SetValue<bool>("Wait", false);
            }
        }
    }
}
