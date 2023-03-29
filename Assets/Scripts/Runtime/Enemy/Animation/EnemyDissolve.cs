using UnityEngine;


namespace Runtime.Enemy.Animation
{
    public class EnemyDissolve : MonoBehaviour
    {
        //�p�����[�^��
        private static readonly string parameterName = "_Cutoff";


        //�R���|�[�l���g
        [SerializeField] private Material dissolveMaterial;
        [SerializeField] private new Renderer renderer;

        //�ݒ�
        [SerializeField] private float time;
        [SerializeField] private AnimationCurve curve;

        //�}�e���A���̕ێ�
        private Material defaultMaterial;

        //�t���O
        public bool isExcuting { get; private set; }
        private float timer;



        //����������
        public void Awake()
        {
            //�f�t�H���g�}�e������sharedMaterial�Ŏ擾����
            defaultMaterial = renderer.sharedMaterial;
            dissolveMaterial.SetFloat(parameterName, 0.0f); //�f�B�]���u��������
            timer = 0;
            isExcuting = false;
        }

        //������鎞�ɏ���������
        public void OnDisable()
        {
            isExcuting = false;
            dissolveMaterial.SetFloat(parameterName, 0.0f);
            renderer.material = defaultMaterial;
            timer = 0;
        }

        //���s
        public void StartDissolve()
        {
            isExcuting = true;
            renderer.material = dissolveMaterial;
        }




        private void Update()
        {
            if (isExcuting)
            {
                timer += Time.deltaTime;

                if(timer >= time)
                {
                    timer = time;
                    isExcuting = false;
                }
                renderer.material.SetFloat(parameterName, curve.Evaluate(timer/time));
            }
        }

    }
}
