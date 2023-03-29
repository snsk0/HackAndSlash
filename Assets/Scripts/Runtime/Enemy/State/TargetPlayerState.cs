using System;

using UnityEngine;

using UniRx;

using Runtime.Enemy.Component;

using StateMachines;
using StateMachines.BlackBoards;


namespace Runtime.Enemy.State
{
    public class TargetPlayerState : ParentStateBase<EnemyController>
    {
        //�t�B�[���h
        private ITargetProvider targetProvider; //target
        private IDisposable disposable;         //�C�x���g�j���p


        //�R���X�g���N�^
        public TargetPlayerState(EnemyController owner, IBlackBoard blackBoard) : base(owner, blackBoard)
        {
            targetProvider = owner.GetComponent<ITargetProvider>();
        }



        //������
        protected override void SelfStart()
        {
            disposable = targetProvider.target.Subscribe(targetObject => OnChangeTarget(targetObject));
        }

        //�I����
        protected override void SelfEnd()
        {
            disposable.Dispose();   //�o�^����
        }


        //�o�^�p
        private void OnChangeTarget(GameObject targetObject)
        {
            if (targetObject.tag != "Player") blackBoard.SetValue<bool>("Player", false);
        }
    }
}