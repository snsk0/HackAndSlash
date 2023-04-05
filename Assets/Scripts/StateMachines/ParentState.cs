using StateMachines.BlackBoards;

namespace StateMachines
{
    //Parent�ł��邱�Ƃ𖾋L����K�v����H
    public abstract class ParentStateBase<T> : StateBase<T>
    {
        //�����X�e�[�g�}�V��
        public StateMachine<T> innerStateMachine { get; }

        //StateMachine�𑱂�����ĊJ���邩
        public bool isResume = false;

        //�R���X�g���N�^
        public ParentStateBase(T owner, IBlackBoard blackBoard) : base(owner, blackBoard)
        {
            innerStateMachine = new StateMachine<T>(owner, blackBoard);
        }


        //Update
        public override sealed void Update()
        {
            SelfUpdate();
            innerStateMachine.Tick();
        }

        //Start
        public override sealed void Start()
        {
            if (!isResume) innerStateMachine.Reset();
            SelfStart();
        }

        //End
        public override sealed void End()
        {
            if(innerStateMachine.currentState != null) innerStateMachine.currentState.End();    //GuardChangeState�Ɠ������R
            SelfEnd();
        }

        //�K�[�h
        public override sealed bool GuardChangeState(StateBase<T> nextState)
        {
            //���g���`�F�b�N
            bool notGuard = SelfGuardChangeState(nextState);

            //���g��true�̂Ƃ��q���`�F�b�N(�q����ɐ؂�ւ���Ă������A�܂�start��tick����Ă΂�Ă��Ȃ��̂�null�ɂȂ��Ă���)
            if (innerStateMachine.currentState != null && notGuard) notGuard = innerStateMachine.currentState.GuardChangeState(nextState);

            //�Ԃ�
            return notGuard;
        }


        //�p���p
        protected virtual void SelfUpdate() { }
        protected virtual void SelfStart() { }
        protected virtual void SelfEnd() { }
        protected virtual bool SelfGuardChangeState(StateBase<T> nextState) { return true; }
    }
}
