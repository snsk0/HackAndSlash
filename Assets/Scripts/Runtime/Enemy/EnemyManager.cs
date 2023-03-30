using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;

using UniRx;
using UniRx.Triggers;

using Runtime.Enemy.State;

using Cysharp.Threading.Tasks;


namespace Runtime.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        //�ݒ荀��
        [SerializeField] private List<Transform> _generateLocationList;
        [SerializeField] private float sameLocationDelay;

        //Enemy������
        private IEnemyGenerator[] generatorList;

        //���W�Ǘ�
        private List<Transform> generateLocationList;
        private ConcurrentBag<Transform> awaitLocationList;

        //�G�̊Ǘ�
        private List<EnemyController> _livingEnemyList;
        public IReadOnlyList<EnemyController> livingEnemyList => _livingEnemyList;
        public IEnumerable<Transform> LivingEnemyTransformList => _livingEnemyList.Select(x => x.transform);

        //�����C�x���g
        private Subject<EnemyController> onGenerateSubject;
        public IObservable<EnemyController> onGenerateEventHandler => onGenerateSubject;


        //������
        private void Awake()
        {
            generateLocationList = new List<Transform>(_generateLocationList);
            generatorList = new IEnemyGenerator[Enum.GetValues(typeof(EnemyType)).Cast<int>().Max() + 1];
            awaitLocationList = new ConcurrentBag<Transform>();
            _livingEnemyList = new List<EnemyController>();
            onGenerateSubject = new Subject<EnemyController>();

            //generator��S�擾
            foreach(IEnemyGenerator generator in GetComponents<IEnemyGenerator>())
            {
                generatorList[(int)generator.enemyType] = generator;
            }
        }
        private void OnDestroy()
        {
            onGenerateSubject.Dispose();
        }


        //����̓G�𐶐����ēn��
        public EnemyController GetInitialEnemy(EnemyType type)
        {
            //�v�f���Ȃ��Ȃ�܂Ŏ��s
            Transform awaitTransform;
            while (awaitLocationList.Count != 0)
            {
                //�v�f���폜����
                bool isSuccess = awaitLocationList.TryTake(out awaitTransform);

                //���s���Ă�����break
                if (!isSuccess) break;

                //�폜�ł��Ă�����generateLocation�ɖ߂�
                generateLocationList.Add(awaitTransform);
            }

            
            //�����\���W���Ȃ��ꍇnull��Ԃ�
            if (generateLocationList.Count == 0) return null;

            //��������
            int index = UnityEngine.Random.Range(0, generateLocationList.Count);

            //���W�����X�g����폜
            Transform transform = generateLocationList[index];
            generateLocationList.Remove(transform);

            //�G�𐶐�
            EnemyController enemy = generatorList[(int)type].Generate(transform);
            onGenerateSubject.OnNext(enemy);
            _livingEnemyList.Add(enemy);

            //�f�X���Ď�����
            SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
            disposable.Disposable = enemy.UpdateAsObservable()
                .Where(_ => { return enemy.currentState is DeathState; })
                .Subscribe(_ =>
                {
                    //�C�x���g�o�^�̉���
                    disposable.Dispose();

                    //���X�g�������
                    _livingEnemyList.Remove(enemy);
                });


            //��莞�Ԍ�Ƀ��X�g�ɖ߂�(�񓯊�)
            UniTask task = WaitLocationCoolTime(transform);

            return enemy;
        }

        //���P�[�V�����̃N�[���^�C���ҋ@
        private async UniTask WaitLocationCoolTime(Transform transform)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(sameLocationDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
            awaitLocationList.Add(transform);
        }
    }
}
