using StateMachines;


namespace Runtime.Util
{
    public static class Util
    {
        public static StateBase<T> GetCurrentLeafState<T>(this StateMachine<T> stateMachine)
        {
            StateBase<T> state = stateMachine.currentState;

            if (state == null) return state;

            //�X�e�[�g���e�ł���ꍇ
            if(state is ParentStateBase<T>)
            {
                //�e�ɃL���X�g���āA�C���i�[�X�e�[�g�}�V�����猻�݃X�e�[�g���ċA�Ŏ擾
                ParentStateBase<T> parentState = (ParentStateBase<T>)state;
                state = parentState.innerStateMachine.GetCurrentLeafState<T>();
            }

            return state;
        }

    }
}

