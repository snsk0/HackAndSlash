using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;

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


        //������
        private void Awake()
        {
            generateLocationList = new List<Transform>(_generateLocationList);
            generatorList = new IEnemyGenerator[Enum.GetValues(typeof(EnemyType)).Cast<int>().Max() + 1];
            awaitLocationList = new ConcurrentBag<Transform>();

            //generator��S�擾
            foreach(IEnemyGenerator generator in GetComponents<IEnemyGenerator>())
            {
                generatorList[(int)generator.enemyType] = generator;
            }
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
