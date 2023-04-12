using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Review.StateMachines
{
    [Serializable]
    public class BaseState
    {
        public ResultType result { get; protected set; }

        public bool IsRepeatable { get; protected set; } = false;

        public virtual void Execute(Blackboard blackboard)
        {
            //�q�N���X�ŋ�̓I�Ȏ���
        }

        public virtual void Abort()
        {
            //�q�N���X�ŋ�̓I�Ȏ���
        }

        protected virtual void FinishState()
        {
            //�q�N���X�ŋ�̓I�Ȏ���
        }
    }
}

