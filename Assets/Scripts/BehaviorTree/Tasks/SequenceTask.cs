namespace BehaviorTree.Tasks
{
    public class SequenceTask : BaseCompositeTask
    {
        //���݂̏��
        TaskStatus status = TaskStatus.Inactive;


        public override bool CanExcute() 
        {
            return children.Count > currentChildIndex && status != TaskStatus.Failure; 
        }





        public override void OnChildExecuted(TaskStatus childStatus) 
        {
            currentChildIndex++;
            status = childStatus;
        }




        public override void OnEnd()
        {
            currentChildIndex = 0;
            status = TaskStatus.Inactive;
        }




        //�ĕ]���̒��f���ɌĂяo�����
        //���g�̎q�����f���ꂽ���ɌĂяo�����
        //childIndex�����g�̊Y������q�܂Ŗ߂�
        //�܂��A���f���ꂽ���A���f����Conditional�^�X�N����ċA�I�ɋ��ʑc��܂�childIndex��߂��K�v������
        public override void OnConditionalAbort(int childIndex)
        {
            //TODO
        }
    }
}
