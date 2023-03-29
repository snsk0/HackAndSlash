using UnityEngine;

using Runtime.Enemy.Animation;


namespace Runtime.Enemy.Component
{
    public class EnemyLook : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private float step;


        public void Look(Vector3 target)
        {
            //��]Quartanion�̌v�Z
            Vector3 direction = target - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);


            //�A�j���[�V����(rotation�����ȏ�)
            var signedAngle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);
            //Debug.Log(signedAngle);


            //��]����
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, step);
        }
    }

}