using UnityEngine;


namespace Runtime.Enemy.Animation
{
    public class EnemyAnimator : MonoBehaviour
    {
        //�A�j���[�^
        [SerializeField] protected Animator animator;


        //�������[�V����
        public virtual void PlayIdle()
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }


        //����
        public virtual void PlayWalk(Vector3 vector, float maxVelocity)
        {
            vector = vector / maxVelocity;

            animator.SetBool("Walk", true);
            animator.SetFloat("Walk_X", vector.x);
            animator.SetFloat("Walk_Z", vector.z);
        }

        
        //����
        public virtual void PlayRun(Vector3 vector, float maxVelocity)
        {
            vector = vector / maxVelocity;

            animator.SetBool("Run", true);
            animator.SetFloat("Run_X", vector.x);
            animator.SetFloat("Run_Z", vector.z);
        }


        //�U��
        public virtual void PlayAttack(int index)
        {
            animator.SetTrigger("Attack_" + index);
        }
        

        //�m�b�N�o�b�N
        public virtual void PlayDamaged(int index)
        {
            animator.SetTrigger("Damaged_" + index);
        }


        //���S
        public virtual void PlayDeath()
        {
            animator.SetTrigger("Death");
        }


        //���̑��A�j���[�V����
        public virtual void PlayUnique(int index)
        {
            animator.SetTrigger("Unique_" + index);
        }
    }
}
