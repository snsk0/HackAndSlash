using System.Collections.Generic;
using UnityEngine;
using BehaviorTree.Tasks;

namespace BehaviorTree
{
    public class SimpleBehaviorTree : MonoBehaviour
    {
        //���^�X�N
        [SerializeField] private BaseTask rootTask;

        //ActiveTask��Index
        private Stack<int> activeStack = new Stack<int>();      //���݃A�N�e�B�u�̃^�X�N�̃X�^�b�N
        private int activeTaskIndex                             //���݃A�N�e�B�u�̃^�X�N
        {
            get
            {
                if(activeStack.Count > 0) return activeStack.Peek();
                return -1;
            }
        }

        //List
        private List<BaseTask> taskList = new List<BaseTask>();

        //BTStatus
        private bool isRootTaskExcuted = false;
        [SerializeField] private bool isLoop;



        //Stack�Ǘ�
        //Push
        private void PushTask(BaseTask task)
        {
            if(activeStack.Count == 0 || activeTaskIndex != task.index)
            {
                //�X�^�b�N�ɐς�
                activeStack.Push(task.index);

                //�X�^�[�g�Ăяo��
                task.OnStart();
            }
        }

        //Pop
        private void PopTask()
        {
            if (activeStack.Count > 0)
            {
                //�X�^�b�N������o��
                int index = activeStack.Pop();

                //�I���Ăяo��
                taskList[index].OnEnd();
            }
        }
        




        //���N���X����Initialize��Awake���Ă� �C���f�b�N�X�ԍ��̐U�蕪�����s��
        private void CallOnAwake(BaseTask task)
        {
            //�^�X�N�̏�����
            task.Initialize(gameObject, taskList.Count);

            //���X�g
            taskList.Add(task);

            //Awake
            task.OnAwake();

            //Composite�͍ċA����
            BaseCompositeTask compositeTask = task as BaseCompositeTask;
            if(compositeTask != null)
            {
                foreach(BaseTask child in compositeTask.children)
                {
                    CallOnAwake(child);
                }
            }
        }



        //�^�X�N�̎��s
        private TaskStatus Excute(BaseTask task)
        {
            //ActiveStack�Ƀv�b�V��
            PushTask(task);

            //�X�e�[�^�X
            TaskStatus status = TaskStatus.Inactive;    //�f�t�H���g��Inactive��Ԃ�


            //Composite
            if (task is BaseCompositeTask)
            {
                BaseCompositeTask compositeTask = task as BaseCompositeTask;


                //���s�\����������s��������
                while (compositeTask.CanExcute())
                {
                    //TODO �ĕ]�����X�g�ɓo�^

                    //�e��OnStarted�Ăяo��
                    compositeTask.OnChildStarted();

                    //�q���ċA�Ŏ��s����
                    BaseTask child = compositeTask.children[compositeTask.currentChildIndex];
                    TaskStatus childStatus = Excute(child);

                    //Runnning�̏ꍇ���f
                    if (childStatus == TaskStatus.Running) return TaskStatus.Running;

                    status = childStatus;
                }
            }

            //Task
            else
            {
                //Update
                status = task.OnUpdate();

                //Runnning�̏ꍇ�͒��f
                if (status == TaskStatus.Running) return TaskStatus.Running;
            }


            //�I������
            PopTask();

            //Pop���CompositeTask�������ꍇ�͎q�̏I���ʒm
            if (activeStack.Count > 0)
            {
                BaseCompositeTask compositeActiveTask = taskList[activeTaskIndex] as BaseCompositeTask;
                if (compositeActiveTask != null) compositeActiveTask.OnChildExecuted(status);
            }

            return status;
        }





        //UnityEngine�œ�����
        private void Start()
        {
            CallOnAwake(rootTask);
        }

        //TODO �^�X�N���s�̃t���[���Ǘ�
        //����A�q�^�X�N��Runnning���o���ꍇ�A���ꂪ���t���[����Success���Ă����̂܂܋A���Ă��Ď��̎q�͍X�Ɏ��t���[���Ƃ����d�l�ɂȂ��Ă���
        //���s�����t���[���⏇�Ԃ͋ɗׂ͍������䂷��K�v������
        private void Update()
        {
            if (activeTaskIndex > -1)
            {
                BaseTask task = taskList[activeTaskIndex];
                Excute(task);
            }
            else if(!isRootTaskExcuted || isLoop)
            {
                Excute(rootTask);
                isRootTaskExcuted = true;
            }
        }




        //�q�^�X�N�Ɛe�^�X�N�ɂ���...
        //�܂��A�e�^�X�N���q�^�X�N�����Ă邱�Ƃɂ��āA�q�^�X�N�ɂ��Ă͕K��Index�łȂ����̂����K�v������
        //����͐e�^�X�N���q�^�X�N�ɉ��炩�̉e����^���邱�Ƃ��ł���悤�ɂ��邽�߂ł���
        //�Ⴆ��Decorator�Ȃǂ�100����s����A�Ȃǂ�����ɊY������B�q�^�X�N�����s�ł��Ȃ��Ⴂ���Ȃ��B
        //�e�^�X�N�ɂ��āA�q����e�������ł��Ȃ��ƍĕ]�����ł��Ȃ�
        //����ɂ��āA�q�^�X�N�͐e�^�X�N��m�炸��BT���ŊǗ�����悤��....�ł��悢��
        //�q����e���Ȃ�炩�̎�i�Ō����ł���悤�ɂ��Ă����Ȃ��ƍ���̂ł���̎������s��


        //���ꂩ...
        //�Œ჌�x��API�Ƃ���1��BehaviorTree�ɕK��Index�ԍ���Task�̏Ɖ���s����N���X�������BT�Ɏ�������
        //�^�X�N�����������ɃA�N�Z�X�ł���悤�ɂ��ėv�f�Ƃ��ĕK���C���f�b�N�X�����悤���ʉ����Ă���
        //�����^�X�N�̖��������Ȃ̂��悭�킩��Ȃ��Ȃ����肷��....

    }
}
