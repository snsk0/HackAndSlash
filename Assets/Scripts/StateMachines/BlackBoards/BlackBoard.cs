using System.Collections.Generic;

namespace StateMachines.BlackBoards
{
    public class BlackBoard : IBlackBoard
    {
        //�u���b�N�{�[�h�̃f�[�^
        //string���Enum������
        private readonly Dictionary<string, object> valueList;

        //������
        public BlackBoard()
        {
            valueList = new Dictionary<string, object>();
        }


        //�Ȃ������ꍇ�G���[��Ԃ�
        public T GetValue<T>(string name)
        {
            if (valueList.ContainsKey(name))
            {
                object value = valueList[name];
                if (value is T) return (T)value;
            }
            throw new System.Exception("BlackBoardError: " + name);
        }


        public bool SetValue<T>(string name, T value)
        {
            if (!valueList.ContainsKey(name))
            {
                valueList.Add(name, value);
            }
            else
            {
                valueList[name] = value;
            }
            return true;
        }
    }
}