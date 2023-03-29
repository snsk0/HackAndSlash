using StateMachines;
using StateMachines.BlackBoards;

using Runtime.Enemy.Component;


namespace Runtime.Enemy.State
{
    public class SeekStateByComponent : StateBase<EnemyController>
    {
        //�R���|�[�l���g
        private EnemySeek seek;
        private ITargetProvider targetProvider; //�ǐՕs�\�ȏꍇ�w�C�g�����������邽��

        //�p�����[�^
        public float stoppingDistance { private get; set; }


        //�R���X�g���N�^
        public SeekStateByComponent(EnemyController owner, IBlackBoard blackBoard) : base(owner, blackBoard)
        {
            seek = owner.GetComponent<EnemySeek>();
            targetProvider = owner.GetComponent<ITargetProvider>();
        }



        public override void Start()
        {
            //stoppingDistance���ɐݒ肷��
            seek.stoppingDistance = stoppingDistance;
            seek.StartSeek(targetProvider.target.Value.transform);
        }

        public override void Update()
        {
            //�o�H�T���Ɏ��s�����ꍇ
            if (seek.isFailed) blackBoard.SetValue<bool>("SeekFailed", true);
            else if (!seek.isSeeking) blackBoard.SetValue<bool>("Seek", false);
        }

        public override void End()
        {
            //���f���ꂽ����Seek�����f����
            seek.EndSeek();
        }
    }
}

