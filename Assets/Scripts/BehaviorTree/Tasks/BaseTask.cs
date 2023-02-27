using UnityEngine;

namespace BehaviorTree.Tasks
{
    //�^�X�N�̊��N���X
    public abstract class BaseTask : MonoBehaviour
    {
        //�t�B�[���h
        public GameObject owner { get; private set; }       //BehaviorTree�̃A�^�b�`��
        public int index { get; private set; }              //Task��Index�ԍ�
        //public BaseTask parent { get; private set; }      //���̃^�X�N�̐e�^�X�N null�Ȃ�root
        [SerializeField] private string taskName;           //�f�o�b�O�p �^�X�N�̖��O



        

        //�T�u�N���X�p
        public virtual void OnAwake() { }       //Tree�N����
        public virtual void OnStart() { }       //Task�N����
        public virtual void OnEnd() { }         //Task�I����(Success or Failure ��Ԃ��Ƃ�)
        public virtual TaskStatus OnUpdate() { return TaskStatus.Inactive; }    //���t���[��





        //������
        public void Initialize(GameObject owner, int index)
        {
            this.owner = owner;
            this.index = index;
        }
    }
}
