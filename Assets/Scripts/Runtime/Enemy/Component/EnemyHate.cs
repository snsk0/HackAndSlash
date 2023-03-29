using System.Collections.Generic;

using UnityEngine;

using UniRx;


namespace Runtime.Enemy.Component
{
    public class EnemyHate : MonoBehaviour, ITargetProvider
    {
        //�w�C�g�l�Ǘ�
        private Dictionary<GameObject, float> hateMap;


        //target(�ύX�C�x���g�܂�)
        private ReactiveProperty<GameObject> _target;
        public IReadOnlyReactiveProperty<GameObject> target => _target;





        //������
        public void Initialize()
        {
            hateMap = new Dictionary<GameObject, float>();
            _target = new ReactiveProperty<GameObject>();
        }



        //�w�C�g�l�ǉ�
        public void AddHate(float hate, GameObject gameObject)
        {
            //�V�K�ǉ�
            if (!hateMap.ContainsKey(gameObject))
            {
                hateMap.Add(gameObject, hate);
            }
            //�w�C�g�l���Z
            else
            {
                float currentHate = hateMap[gameObject];
                hateMap[gameObject] = currentHate + hate;
            }

            //target�X�V
            _target.Value = GetMaxHateObject();
        }


        //�N���A
        public void ClearHate(GameObject gameObject)
        {
            if (hateMap.ContainsKey(gameObject)) hateMap.Remove(gameObject);

            //target�X�V
            _target.Value = GetMaxHateObject();
        }



        //�ő�w�C�g�l�̃I�u�W�F�N�g�擾
        private GameObject GetMaxHateObject()
        {
            //��r�p
            GameObject gameObject = null;
            float maxHate = 0;

            //�S����
            foreach (KeyValuePair<GameObject, float> hatePair in hateMap)
            {
                if (maxHate < hatePair.Value)
                {
                    maxHate = hatePair.Value;
                    gameObject = hatePair.Key;
                }
            }

            //���ʂ�Ԃ�
            return gameObject;
        }



        //�I�����ɔj��
        private void OnDisable()
        {
            hateMap = null;
            _target.Dispose();
        }
    }
}
