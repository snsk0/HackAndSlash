using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace InGame.Players.Fighters
{
    public class FighterAnimationPlayer : PlayerAnimationPlayer
    {
        public async override UniTask PlayAttackAnimation(CancellationToken token, Action<bool> attackCallback = null)
        {
            //TODO:�U������̃^�C�~���O�܂ő҂�
            //�U������L�����̃R�[���o�b�N
            attackCallback?.Invoke(true);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            attackCallback?.Invoke(false);
        }
    }
}

