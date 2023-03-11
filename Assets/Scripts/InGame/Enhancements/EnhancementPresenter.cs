using InGame.Players;
using InGame.Players.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using VContainer.Unity;
using VContainer;
using InGame.Enemies;

namespace InGame.Enhancements
{
    public class EnhancementPresenter : ControllerBase, IStartable
    {
        private readonly PlayerManager playerManager;
        private readonly EnhancementView enhancementView;
        private readonly EnemyManager enemyManager;
        private readonly PlayerBackpack playerBackpack;

        private readonly PlayerInput playerInput = new PlayerInput();

        [Inject]
        public EnhancementPresenter(PlayerManager playerManager, EnhancementView enhancementView, EnemyManager enemyManager, PlayerBackpack playerBackpack)
        {
            this.playerManager = playerManager;
            this.enhancementView = enhancementView;
            this.enemyManager = enemyManager;
            this.playerBackpack = playerBackpack;
        }

        public void Start()
        {
            //�p�����[�^�A�b�v�̃{�^���������ꂽ�Ƃ��̏���
            enhancementView.parameterUpButtonClickObservable
                .Subscribe(pair =>
                {
                    playerManager.playerParameter.IncreaseAddParameter(pair.Key, pair.Value);
                    playerBackpack.DecreaseEnhancementPoint(pair.Value);
                })
                .AddTo(this);

            //������ʂ��J�����Ƃ��̏���
            playerInput.ObserveEveryValueChanged(x => x.HadPushedEnhance)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    enhancementView.ViewPanel();
                })
                .AddTo(this);

            //�G���狭���|�C���g���擾�����Ƃ��̏���
            enemyManager.DropedEnhancementPointObservable
                .Subscribe(value =>
                {
                    playerBackpack.AddEnhancementPoint(value);
                })
                .AddTo(this);

            //���݂̋����|�C���g��\��
            playerBackpack.ObserveEveryValueChanged(x => x.enhancementPoint)
                .Subscribe(point =>
                {
                    enhancementView.SetPointText(point);
                    if (point >= 10)
                    {
                        enhancementView.SetIntaractableOneUpButton(true);
                        enhancementView.SetIntaractableTenUpButton(true);
                    }
                    else if (point >= 1)
                    {
                        enhancementView.SetIntaractableOneUpButton(true);
                        enhancementView.SetIntaractableTenUpButton(false);
                    }
                    else
                    {
                        enhancementView.SetIntaractableOneUpButton(false);
                        enhancementView.SetIntaractableTenUpButton(false);
                    }
                })
                .AddTo(this);
        }
    }
}

