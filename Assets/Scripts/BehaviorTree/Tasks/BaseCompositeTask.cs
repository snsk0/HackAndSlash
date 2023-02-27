using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Tasks
{
    //�ĕ]���`��
    public enum AbortType
    {
        None,           //����
        Self,           //���g�̎q�̂�
        LowPriority,    //���g�̌�Ɏ��s����Ă��鎞�̂�
        Both            //Self��LowPriority
    }



    public abstract class BaseCompositeTask : BaseTask
    {
        //�q�^�X�N
        [SerializeField] private List<BaseTask> _children = new List<BaseTask>();
        public List<BaseTask> children => _children;
        public int currentChildIndex{ get; protected set; }

        //�ĕ]���^�C�v
        [SerializeField] private AbortType abortType;




        //�T�u�N���X�p
        public virtual bool CanExcute() { return true; }                            //���s�\��
        public virtual void OnChildStarted() { }                                    //�q���J�n�������ɌĂяo�����
        public virtual void OnChildExecuted(TaskStatus childStatus) { }             //�q�����s����I������Ƃ��ɌĂяo��
        public virtual void OnConditionalAbort(int childIndex) { }                  //�ĕ]���Œ��f���ꂽ��
    }
}
