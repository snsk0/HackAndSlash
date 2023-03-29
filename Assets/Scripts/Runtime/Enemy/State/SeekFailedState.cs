using UnityEngine;

using StateMachines;
using StateMachines.BlackBoards;

using Runtime.Enemy.Animation;
using Runtime.Enemy.Component;


namespace Runtime.Enemy.State
{
    public class SeekFailedState : StateBase<EnemyController>
    {
        //�R���|�[�l���g
        private EnemyHate hate;
        private EnemyAnimator animator;


        //�R���X�g���N�^
        public SeekFailedState(EnemyController owner, IBlackBoard blackBoard) : base(owner, blackBoard)
        {
            hate = owner.GetComponent<EnemyHate>();
            animator = owner.GetComponent<EnemyAnimator>();
        }



        public override void Start()
        {
            //�v���C���[�̃w�C�g��0�ɂ���
            GameObject player = GameObject.FindWithTag("Player");
            hate.ClearHate(player);

            //�������[�V�������Đ�����
            animator.PlayIdle();
        }

        public override void Update()
        {
            //�w�C�g�l��ύX���ĉ��P����Ȃ��ꍇ�AIdle�ɖ߂�
            //blackBoard.SetValue<bool>("SeekFailed", false);
        }

        public override void End()
        {
            //�ύX�����ꍇ�K���u���b�N�{�[�h�̃t���O��false��
            blackBoard.SetValue<bool>("SeekFailed", false);
        }
    }
}
