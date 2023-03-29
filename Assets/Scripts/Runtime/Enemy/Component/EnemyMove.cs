using UnityEngine;

using Runtime.Enemy.Parameter;
using Runtime.Enemy.Animation;

namespace Runtime.Enemy.Component
{
    public class EnemyMove : MonoBehaviour
    {
        //�R���|�[�l���g
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new Collider collider;
        [SerializeField] private PhysicMaterial material;
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private EnemyParameter parameter;

        //�萔
        private const float moveForceMultiplier = 15.0f;

        //Input
        public bool sprint { get; set; } = false;
        private Vector3 direction;

        //�f�t�H���g�}�e���A��
        private PhysicMaterial defaultMaterial;


        //�ړ��֐�
        public void MoveByLocalDir(Vector3 direction)
        {
            //�x�N�g���𐳋K���ĕێ�
            this.direction = direction.normalized;
        }
        public void MoveByWorldDir(Vector3 direction)
        {
            //�t��]���ĕ�����ێ�
            direction = Quaternion.Inverse(transform.rotation) * direction;
            this.direction = direction.normalized;
        }



        //�ړ�����
        public void FixedUpdate()
        {
            //�^����ꂽ�����x�N�g������]���Ĉړ��x�N�g�������߂�
            Vector3 vector = transform.rotation * direction;

            //�͂�������
            float magnitude = parameter.speed - rigidbody.velocity.magnitude;
            rigidbody.AddForce(moveForceMultiplier * (vector * magnitude - rigidbody.velocity), ForceMode.Acceleration);

            //�A�j���[�V�������Đ�
            animator.PlayRun(Quaternion.Inverse(transform.rotation) * rigidbody.velocity, parameter.speed);

            //���͂�������
            direction = Vector3.zero;
        }



        //���C�Ǘ�
        private void OnEnable()
        {
            defaultMaterial = collider.material;
            collider.material = material;
            rigidbody.isKinematic = false;
        }
        private void OnDisable()
        {
            collider.material = defaultMaterial;
            rigidbody.isKinematic = true;
        }
    }
}