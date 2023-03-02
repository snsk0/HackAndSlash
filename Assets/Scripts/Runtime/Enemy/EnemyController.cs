using System;

using UnityEngine;

using UniRx;

using Runtime.Enemy.Component;
using Runtime.Enemy.Parameter;


namespace Runtime.Enemy 
{
    public class EnemyController : MonoBehaviour, IEnemyDamagable, IEnemyEventSender
    {
        //�G�̃R���|�[�l���g�Q
        [SerializeField] private EnemyHealth health;
        [SerializeField] private EnemyHate hate;
        [SerializeField] private EnemyKnock knock;
        [SerializeField] private EnemyMove move;
        [SerializeField] private EnemyAttack attack;

        //�p�����[�^
        [SerializeField] private EnemyParameter _parameter;
        public EnemyParameter parameter => _parameter;


        //�C�x���g
        private Subject<Unit> _onMove;
        public IObservable<Unit> onMove => _onMove;
        private Subject<Unit> _onAttack;
        public IObservable<Unit> onAttack => _onAttack;
        private Subject<EnemyDamageEvent> _onDamage;
        public IObservable<EnemyDamageEvent> onDamage => _onDamage;



        //�_���[�W
        public void Damage(float damage, float knock, float hate, GameObject cause)
        {
            //�_���[�W
            float damaged = health.Damage(damage);
            _onDamage.OnNext(new EnemyDamageEvent(health.maxHealth, health.currentHealth, damage));

            //�m�b�N�o�b�N
            float knocked = this.knock.KnockBack(transform.position - cause.transform.position, knock);

            //�w�C�g�l
            this.hate.AddHate(hate, cause);
        }

        //�ړ�
        public bool MoveToTarget()
        {
            _onMove.OnNext(Unit.Default);
            return move.MoveToTarget(hate.GetMaxHateObject().transform.position);
        }


        //�U�� �U������ɂ����鎞�Ԃ�Ԃ�
        public float Attack(int index)
        {
            _onAttack.OnNext(Unit.Default);
            return attack.AttackToTarget(hate.GetMaxHateObject(), index);
        }



        //������
        public void Initialize(int level)
        {
            //�p�����[�^������
            parameter.Initialize(level);
        }



        //�C�x���g�̏������͕ʓr�����ōs��
        private void OnEnable()
        {
            //�C�x���g������
            _onMove = new Subject<Unit>();
            _onAttack = new Subject<Unit>();
            _onDamage = new Subject<EnemyDamageEvent>();
        }
        private void OnDisable()
        {
            _onMove.Dispose();
            _onAttack.Dispose();
            _onDamage.Dispose();
        }
    }
}
