using System;
using System.Collections.Generic;

using StateMachines.BlackBoards;


namespace StateMachines
{
    public class StateMachine<T>
    {
        //�J�ڗp�N���X
        private class Transition
        {
            //�t�B�[���h
            public StateBase<T> nextState { get; }
            public Func<IReadOnlyBlackBoard, bool> condition { get; }       //�J�ڏ���


            public Transition(StateBase<T> nextState, Func<IReadOnlyBlackBoard, bool> condition)
            {
                this.nextState = nextState;
                this.condition = condition;
            }
        }

        //�t�B�[���h
        public readonly T owner;                                                                 //�I�[�i�[
        private readonly IReadOnlyBlackBoard blackBoard;                                         //�u���b�N�{�[�h
        private readonly Dictionary<StateBase<T>, List<Transition>> transitionList;              //�J�ڃ��X�g
        public StateBase<T> currentState { get; private set; }                                   //���݃X�e�[�g
        private StateBase<T> initialState;                                                       //�����X�e�[�g





        //�R���X�g���N�^
        public StateMachine(T owner, IReadOnlyBlackBoard blackBoard)
        {
            this.owner = owner;
            this.blackBoard = blackBoard;

            transitionList = new Dictionary<StateBase<T>, List<Transition>>();
        }





        //�����Z�b�g�A�b�v
        public void Initialize(StateBase<T> state)
        {
            initialState = state;
            //state��awake����������Ȃ炱���ňꊇ
        }
        //���Z�b�g
        public void Reset()
        {
            currentState = null;
            //state��awake����������Ȃ炱���ňꊇ
        }




        //�X�V���\�b�h
        public void Tick()
        {
            //���݃X�e�[�g��null�̏ꍇ
            if(currentState == null)
            {
                //����������start�Ăяo��
                currentState = initialState;
                currentState.Start();
            }


            //Update�̌Ăяo��
            currentState.Update();


            //�J�ڃ`�F�b�N
            StateBase<T> nextState = CanTransition(currentState);
            if (nextState != null) ChangeState(nextState);
        }





        //�J�ڒǉ�
        public void AddTransition(StateBase<T> state, StateBase<T> nextState, Func<IReadOnlyBlackBoard, bool> condition)
        {
            //�J�ڃI�u�W�F�N�g�̐���
            Transition transition = new Transition(nextState, condition);

            //�J�ڌ������邩�ǂ���
            if (transitionList.ContainsKey(state))
            {
                //���X�g���擾���Ēǉ�
                transitionList[state].Add(transition);
            }

            else
            {
                //���X�g�𐶐����Ēǉ�����
                List<Transition> valueList = new List<Transition>();
                valueList.Add(transition);
                transitionList.Add(state, valueList);
            }
        }






        //�X�e�[�g�ύX
        private void ChangeState(StateBase<T> nextState)
        {
            //���݃X�e�[�g�̏I����ʒm
            currentState.End();

            //���X�e�[�g�ɕύX����
            currentState = nextState;

            //�X�^�[�g�Ăяo��
            currentState.Start();
        }


        //�J�ڂ̊m�F �ł��Ȃ��Ȃ�null
        private StateBase<T> CanTransition(StateBase<T> state)
        {
            if (transitionList.ContainsKey(state))
            {
                //�S�Ă̏������ォ��`�F�b�N
                foreach (Transition transition in transitionList[state])
                {
                    //���̑J�ڐ�ɑ΂���K�[�h���Ȃ���
                    bool guard = state.GuardChangeState(transition.nextState);

                    //�J�ڂ��\���m�F
                    bool flag = transition.condition.Invoke(blackBoard);
                    if (flag && guard) return transition.nextState;
                }
            }

            return null;
        }
    }
}
