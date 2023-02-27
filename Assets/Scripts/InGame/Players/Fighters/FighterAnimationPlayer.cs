using Cysharp.Threading.Tasks;
using InGame.Players.Animators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace InGame.Players.Fighters
{
    public class FighterAnimationPlayer : PlayerAnimationPlayer
    {
        public bool IsConnectableSecondAttack { get; private set; }
        public bool IsConnectableThirdAttack { get; private set; }

        public async UniTask PlayFirstAttackAnimation(CancellationToken token, Action<bool> attackCallback = null)
        {
            animator.SetTrigger(AnimatorTriggerHashes.FirstAttack);
            //�U������̃^�C�~���O�܂ő҂�
            await AnimationTransitionWaiter.WaitStateTime(0.65f, (int)AnimatorLayerType.Base, AnimatorStateHashes.FirstAttack, animator, token);
            //�U������L�����̃R�[���o�b�N
            attackCallback?.Invoke(true);
            await AnimationTransitionWaiter.WaitStateTime(0.8f, (int)AnimatorLayerType.Base, AnimatorStateHashes.FirstAttack, animator, token);
            attackCallback?.Invoke(false);

            IsConnectableSecondAttack = true;
            await AnimationTransitionWaiter.WaitStateTime(1.03f, (int)AnimatorLayerType.Base, AnimatorStateHashes.FirstAttack, animator, token);
            IsConnectableSecondAttack = false;
        }

        public async UniTask PlaySecondAttackAnimation(CancellationToken token, Action<bool> attackCallback = null)
        {
            animator.SetTrigger(AnimatorTriggerHashes.SecondAttack);
            //�U������̃^�C�~���O�܂ő҂�
            await AnimationTransitionWaiter.WaitStateTime(0.36f, (int)AnimatorLayerType.Base, AnimatorStateHashes.SecondAttack, animator, token);
            //�U������L�����̃R�[���o�b�N
            attackCallback?.Invoke(true);
            await AnimationTransitionWaiter.WaitStateTime(0.72f, (int)AnimatorLayerType.Base, AnimatorStateHashes.SecondAttack, animator, token);
            attackCallback?.Invoke(false);

            IsConnectableThirdAttack = true;
            await AnimationTransitionWaiter.WaitStateTime(1.1f, (int)AnimatorLayerType.Base, AnimatorStateHashes.SecondAttack, animator, token);
            IsConnectableThirdAttack = false;
        }

        public async UniTask PlayThirdAttackAnimation(CancellationToken token, Action<bool> attackCallback = null)
        {
            animator.SetTrigger(AnimatorTriggerHashes.ThirdAttack);
            //�U������̃^�C�~���O�܂ő҂�
            await AnimationTransitionWaiter.WaitStateTime(0.28f, (int)AnimatorLayerType.Base, AnimatorStateHashes.ThirdAttack, animator, token);
            //�U������L�����̃R�[���o�b�N
            attackCallback?.Invoke(true);
            await AnimationTransitionWaiter.WaitStateTime(0.35f, (int)AnimatorLayerType.Base, AnimatorStateHashes.ThirdAttack, animator, token);
            attackCallback?.Invoke(false);
        }
    }
}

