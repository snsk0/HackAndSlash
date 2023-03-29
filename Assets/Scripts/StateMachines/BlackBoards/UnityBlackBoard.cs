using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.BlackBoards
{
    public class UnityBlackBoard : MonoBehaviour, IBlackBoard
    {
        //�u���b�N�{�[�h�̃f�[�^
        private Dictionary<string, object> valueList;

        //������
        private void Awake()
        {
            valueList = new Dictionary<string, object>();
        }


        //�Ȃ������ꍇ�f�t�H���g�l��Ԃ�
        public T GetValue<T>(string name)
        {
            if (valueList.ContainsKey(name))
            {
                object value = valueList[name];
                if (value is T) return (T)value;
            }
            return default(T);
        }


        public bool SetValue<T>(string name, T value)
        {
            if (!valueList.ContainsKey(name))
            {
                valueList.Add(name, value);
            }
            return false;
        }
    }
}
