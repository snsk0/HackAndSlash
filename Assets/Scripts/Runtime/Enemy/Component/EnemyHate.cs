using System.Collections.Generic;
using UnityEngine;


namespace Runtime.Enemy.Component
{
    public class EnemyHate : MonoBehaviour
    {
        //�w�C�g�l�Ǘ�
        private Dictionary<GameObject, float> hateMap;



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
        }



        //�ő�w�C�g�l�̃I�u�W�F�N�g�擾
        public GameObject GetMaxHateObject()
        {
            //��r�p
            GameObject gameObject = null;
            float maxHate = 0;

            //�S����
            foreach(KeyValuePair<GameObject, float> hatePair in hateMap)
            {
                if(maxHate < hatePair.Value)
                {
                    maxHate = hatePair.Value;
                    gameObject = hatePair.Key;
                }
            }

            //���ʂ�Ԃ�
            return gameObject;
        }



        //����������
        private void OnEnable()
        {
            hateMap = new Dictionary<GameObject, float>();
        }
    }
}
