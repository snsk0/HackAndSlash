using Cysharp.Threading.Tasks;
using InGame.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

namespace InGame.Targets
{
    public class TargetManager : IDisposable
    {
        private readonly ReactiveProperty<Transform> currentTargetTransform = new ReactiveProperty<Transform>();
        public IReadOnlyReactiveProperty<Transform> TargetedTransform => currentTargetTransform;

        public IEnemyDamagable currentTargetEnemy { get; private set; }

        private readonly TargetSearcher targetSearcher;
        private readonly CancellationTokenSource tokenSource;

        public TargetManager(TargetSearcher targetSearcher)
        {
            this.targetSearcher = targetSearcher;

            tokenSource = new CancellationTokenSource();
            UpdateTarget(tokenSource.Token).Forget();
        }

        private async UniTaskVoid UpdateTarget(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;

                //�^�[�Q�b�g�̎擾
                Transform target = targetSearcher.SerchTarget();
                if (target == currentTargetTransform.Value)
                {
                    //�^�[�Q�b�g���ω����Ȃ��Ȃ牽�����Ȃ�
                    await UniTask.DelayFrame(1, cancellationToken: token);
                    continue;
                }

                //�^�[�Q�b�g�̍X�V
                currentTargetTransform.Value = target;
                currentTargetEnemy = target == null ? null : target.GetComponent<IEnemyDamagable>();

                await UniTask.DelayFrame(1, cancellationToken: token);
            }
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}

