using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Review.StateMachines.Transition;

namespace Review.StateMachines.Editor
{
    [CustomPropertyDrawer(typeof(Transition))]
    public class TransitionDrawer : PropertyDrawer
    {
        private class PropertyData
        {
            public SerializedProperty beforeStateProperty;
            public SerializedProperty afterStateProperty;

            public SerializedProperty stateObjectsProperty;
            public SerializedProperty subStateMachinesProperty;

            public SerializedProperty conditionsProperty;

            public int beforeStateIndex = 0;
            public int afterStateIndex = 0;
            public int stateMachineIndex = 0;
        }

        private Dictionary<string, PropertyData> _propertyDataPerPropertyPath = new Dictionary<string, PropertyData>();
        private PropertyData _property;

        private float LineHeight { get { return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; } }

        private bool IsStateEmpty = false;

        private void Init(SerializedProperty property)
        {
            if (_propertyDataPerPropertyPath.TryGetValue(property.propertyPath, out _property))
            {
                return;
            }

            _property = new PropertyData();
            _property.beforeStateProperty = property.FindPropertyRelative("beforeState");
            _property.afterStateProperty = property.FindPropertyRelative("afterState");

            var serializedObject = property.serializedObject;
            _property.stateObjectsProperty = serializedObject.FindProperty("stateObjects");
            _property.subStateMachinesProperty = serializedObject.FindProperty("subStateMachineSettings");

            _property.conditionsProperty = property.FindPropertyRelative("consitions");

            _propertyDataPerPropertyPath.Add(property.propertyPath, _property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);
            var fieldRect = position;
            // �C���f���g���ꂽ�ʒu��Rect���~������΂��������g��
            var indentedFieldRect = EditorGUI.IndentedRect(fieldRect);
            fieldRect.height = LineHeight;


            // Prefab��������v���p�e�B�ɕύX���������ۂɑ����ɂ����肷��@�\�������邽��PropertyScope���g��
            using (new EditorGUI.PropertyScope(fieldRect, label, property))
            {
                // �v���p�e�B����\�����Đ܂��ݏ�Ԃ𓾂�
                property.isExpanded = EditorGUI.Foldout(new Rect(fieldRect), property.isExpanded, label);
                if (property.isExpanded)
                {

                    if (_property.stateObjectsProperty.arraySize == 0)
                    {
                        IsStateEmpty = true;
                        fieldRect.y += LineHeight;
                        EditorGUI.HelpBox(new Rect(fieldRect.x, fieldRect.y, fieldRect.width, fieldRect.height * 2), "�X�e�[�g�����݂��܂���", MessageType.Warning);
                        return;
                    }

                    IsStateEmpty = false;

                    using (new EditorGUI.IndentLevelScope())
                    {
                        if (_property.stateObjectsProperty == null || !_property.stateObjectsProperty.isArray)
                            return;

                        //�J�ڌ���State�̃��X�g���쐬
                        var stateObjectsSize = _property.stateObjectsProperty.arraySize;
                        string[] stateNameArray = new string[stateObjectsSize + 1];
                        for (int i = 0; i < stateObjectsSize; i++)
                        {
                            stateNameArray[i+1] = ((BaseStateObject)_property.stateObjectsProperty.GetArrayElementAtIndex(i).objectReferenceValue)?.stateName;
                        }
                        stateNameArray[0] = "Any";

                        // �J�ڌ��̑I�𗓂�`��
                        fieldRect.y += LineHeight;
                        _property.beforeStateIndex = EditorGUI.Popup(new Rect(fieldRect), "beforeState", _property.beforeStateIndex, stateNameArray);
                        _property.beforeStateProperty.objectReferenceValue = _property.beforeStateIndex == stateObjectsSize ? null : _property.stateObjectsProperty.GetArrayElementAtIndex(_property.beforeStateIndex).objectReferenceValue;

                        //�J�ڐ�̃X�e�[�g�}�V���̖��O�̃��X�g���쐬
                        var subStatemachineSize = _property.subStateMachinesProperty.arraySize;
                        string[] stateMachineNameArray = new string[subStatemachineSize + 1];
                        for(int i = 0; i < subStatemachineSize; i++)
                        {
                            stateMachineNameArray[i + 1] = ((StateMachineSetting)_property.subStateMachinesProperty.GetArrayElementAtIndex(i).objectReferenceValue).name;
                        }
                        stateMachineNameArray[0] = "This";

                        //�J�ڐ�̃X�e�[�g�}�V�[���̑I�𗓂�`��
                        fieldRect.y += LineHeight;
                        _property.stateMachineIndex = EditorGUI.Popup(new Rect(fieldRect), "stateMachine", _property.stateMachineIndex, stateMachineNameArray);

                        //�J�ڌ��State�̃��X�g���쐬
                        string[] afterStateNameArray;
                        if (_property.stateMachineIndex == 0)
                        {
                            afterStateNameArray = stateNameArray.Skip(1).Take(stateObjectsSize).ToArray();
                        }
                        else
                        {
                            StateMachineSetting subStateMachineSetting = (StateMachineSetting)_property.subStateMachinesProperty.GetArrayElementAtIndex(_property.stateMachineIndex - 1).objectReferenceValue;
                            afterStateNameArray = subStateMachineSetting.StateObjects.Select(x => x.stateName).ToArray();
                        }

                        // �J�ڐ�̑I�𗓂�`��
                        fieldRect.y += LineHeight;
                        _property.afterStateIndex = EditorGUI.Popup(new Rect(fieldRect), "afterState", _property.afterStateIndex, afterStateNameArray);
                        if(_property.stateMachineIndex == 0)
                        {
                            _property.afterStateProperty.objectReferenceValue = _property.stateObjectsProperty.GetArrayElementAtIndex(_property.afterStateIndex).objectReferenceValue;
                        }
                        else
                        {
                            StateMachineSetting subStateMachineSetting = (StateMachineSetting)_property.subStateMachinesProperty.GetArrayElementAtIndex(_property.stateMachineIndex - 1).objectReferenceValue;
                            _property.afterStateProperty.objectReferenceValue = subStateMachineSetting.StateObjects.ElementAt(_property.afterStateIndex);
                        }

                        //�J�ڏ�����`��
                        fieldRect.y += LineHeight;
                        EditorGUI.PropertyField(new Rect(fieldRect), _property.conditionsProperty);
                    }
                }
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);

            if (property.isExpanded)
            {
                if (IsStateEmpty)
                {
                    return LineHeight * 3;
                }

                float height = LineHeight * 4;
                height += EditorGUI.GetPropertyHeight(_property.conditionsProperty);
                return height;
            }
            else
            {
                return LineHeight;
            }
        }
    }
}

