using System.Collections.Generic;

using UnityEngine;

using UniRx;
using UniRx.Triggers;


namespace Runtime.Enemy
{
    public class EnemyPool : MonoBehaviour, IEnemyGenerator
    {
        //�p�����[�^
        [SerializeField] private float maxInitializeAmount;
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private EnemyType _enemyType;

        //type�Q��
        public EnemyType enemyType => _enemyType;

        //Pool�p
        private Stack<EnemyController> disableEnemyStack;



        //������
        private void Awake()
        {
            //�X�^�b�N�𐶐�
            disableEnemyStack = new Stack<EnemyController>();

            //�������񐔕�Stack�ɐς�
            for(int i = 0; i < maxInitializeAmount; i++)
            {
                PushInitialPrefab();
            }
        }


        //�X�^�b�N�ɐV����Prefab��ς�
        private void PushInitialPrefab()
        {
            EnemyController enemy = Instantiate(enemyPrefab);
            enemy.gameObject.SetActive(false);
            enemy.transform.parent = gameObject.transform;
            disableEnemyStack.Push(enemy);
        }


        //�����֐�
        public EnemyController Generate(Transform transform)
        {
            //�X�^�b�N�̎c�肪�Ȃ��ꍇ
            if(disableEnemyStack.Count == 0)
            {
                PushInitialPrefab();
            }

            //�X�^�b�N������o���ėL����
            EnemyController enemy = disableEnemyStack.Pop();
            enemy.transform.position = transform.transform.position;
            enemy.transform.rotation = transform.transform.rotation;
            enemy.gameObject.SetActive(true);

            //Disable���m
            SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
            disposable.Disposable = enemy.gameObject.OnDisableAsObservable()
                .Subscribe(_ =>
                {
                    //�C�x���g�o�^�̉���
                    disposable.Dispose();

                    //�X�^�b�N�ɐς݂Ȃ���
                    disableEnemyStack.Push(enemy);
                });

            return enemy;
        }


    }
}
