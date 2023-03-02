using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using InGame.Damages;
using InGame.Enemies;

namespace InGame.Players.Fighters
{
    public class FighterAttacker : PlayerAttacker
    {
        [SerializeField] private FighterAnimationPlayer fighterAnimationPlayer;
        [SerializeField] private FighterAttackCollider fighterAttackCollider;

        private void Start()
        {
            fighterAttackCollider.OnTriggerEnterAsObservable()
                .Select(trigger => trigger.GetComponent<IEnemyDamagable>())
                .Where(enemy => enemy != null)
                .Subscribe(enemy =>
                {
                    var attackValue = (playerParameter.baseAttackValue + playerParameter.addAttackValue) * playerParameter.attackMagnification;
                    var damage = new Damage(attackValue);
                    enemy.ApplyDamage(damage);
                })
                .AddTo(this);
        }

        public override void Attack()
        {
            if (fighterAnimationPlayer.IsConnectableSecondAttack)
            {
                //二段目の攻撃につなげる
                fighterAnimationPlayer.PlaySecondAttackAnimation(this.GetCancellationTokenOnDestroy(), fighterAttackCollider.EnableCollider).Forget();
            }
            else if (fighterAnimationPlayer.IsConnectableThirdAttack)
            {
                //三段目の攻撃につなげる
                fighterAnimationPlayer.PlayThirdAttackAnimation(this.GetCancellationTokenOnDestroy(), fighterAttackCollider.EnableCollider).Forget();
            }
            else
            {
                //最初の攻撃
                fighterAnimationPlayer.PlayFirstAttackAnimation(this.GetCancellationTokenOnDestroy(), fighterAttackCollider.EnableCollider).Forget();
            }
        }
    }
}