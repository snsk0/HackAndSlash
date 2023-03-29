using StateMachines.BlackBoards;

namespace StateMachines
{
    public abstract class StateBase<T>
    {
        //�t�B�[���h
        protected readonly T owner;                     //�I�[�i�[
        protected readonly IBlackBoard blackBoard;      //�u���b�N�{�[�h


        //�R���X�g���N�^
        public StateBase(T owner, IBlackBoard blackBoard)
        {
            this.owner = owner;
            this.blackBoard = blackBoard;
        }



        //�p���p
        public virtual void Update() { }
        public virtual void Start() { }
        public virtual void End() { }
        public virtual bool GuardChangeState(StateBase<T> nextState) { return true; }
    }
}
