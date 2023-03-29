using System;

using UnityEngine;

using UniRx;

using Runtime.UI.View;
using Runtime.Enemy.Component;

namespace Runtime.UI.Presenter
{
    public class HealthPresenter : MonoBehaviour
    {
        //�R���|�[�l���g
        [SerializeField] private EnemyHealth health;
        [SerializeField] private Canvas canvas;
        [SerializeField] private HealthView view;

        //dispose
        private IDisposable disposable;

        //�ŏ��Ɉ�x�����o�^
        private void Start()
        {
            disposable = health.currentHealth.Subscribe(health =>
            {
                view.SetValue(health / this.health.maxHealth);
            });
        }

        //�L�����o�X���J�����Ɍ�����
        private void Update()
        {
            canvas.worldCamera = Camera.main;
            canvas.transform.rotation = canvas.worldCamera.transform.rotation;
        }

        //dispose�ɂȂ������ɉ�������
        private void OnDisable()
        {
            if (disposable != null) disposable.Dispose();
        }
    }
}
