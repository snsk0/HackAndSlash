using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;



namespace Runtime.UI.View
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Slider mainSlider;
        [SerializeField] private Slider animationSlider;


        public void SetValue(float value)
        {
            //���C���X���C�_�[�͑����ɕύX
            mainSlider.value = value;

            //�T�u�X���C�_�[���A�j���[�V����
            DOTween.To(() => animationSlider.value,
                n => animationSlider.value = n,
                value,
                duration: 1.0f);
        }
    }
}
