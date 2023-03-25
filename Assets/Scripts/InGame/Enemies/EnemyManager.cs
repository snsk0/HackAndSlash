using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using UniRx;
using System;
using UniRx.Triggers;
using System.Linq;

namespace InGame.Enemies
{
    public class EnemyManager : ControllerBase
    {
        private readonly EnemyGenerator enemyGenerator;

        private readonly List<GameObject> currentEnemyObjects = new List<GameObject>();
        public IEnumerable<GameObject> CurrentEnemyObjects => currentEnemyObjects.Where(x=>!x.GetComponent<EnemyHealth>().HadDeadReactiveProperty.Value);

        private readonly ISubject<int> dropedEnhancementPointSubject = new Subject<int>();
        public IObservable<int> DropedEnhancementPointObservable => dropedEnhancementPointSubject;

        [Inject]
        public EnemyManager(EnemyGenerator enemyGenerator)
        {
            this.enemyGenerator = enemyGenerator;
        }

        public void GenerateEnemy()
        {
            var enemy = enemyGenerator.GenerateEnemy();
            currentEnemyObjects.Add(enemy);
            ObserveEnemyDeath(enemy.GetComponent<EnemyHealth>());

            enemy.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    currentEnemyObjects.Remove(enemy);
                })
                .AddTo(this);
        }

        //�v���C���[�̎��S���Ď�
        public void ObserveEnemyDeath(EnemyHealth enemyHealth)
        {
            enemyHealth.HadDeadReactiveProperty
                .Where(value => value)
                .Take(1)
                .Delay(TimeSpan.FromSeconds(2f))
                .Subscribe(_ =>
                {
                    //TODO: �G�̐����^�C�~���O�͕ʂŐݒ肷��
                    GenerateEnemy();
                    //TODO: Enemy���ɗ��Ƃ��|�C���g��ݒ肵�Ă���
                    dropedEnhancementPointSubject.OnNext(1);
                })
                .AddTo(this);
        }
    }
}

