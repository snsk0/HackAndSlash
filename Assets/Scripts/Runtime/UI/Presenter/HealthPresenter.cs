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

        //�ŏ��Ɉ�x�����o�^
        private void Start()
        {
            health.currentHealth.Subscribe(health =>
            {
                view.SetValue(health / this.health.maxHealth);
            }).AddTo(this);
        }

        //�L�����o�X���J�����Ɍ�����
        private void Update()
        {
            canvas.worldCamera = Camera.main;
            canvas.transform.rotation = canvas.worldCamera.transform.rotation;
        }
    }
}
