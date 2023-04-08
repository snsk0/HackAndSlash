using Review.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TransitionCondition))]
public class TransitionConsitionDrawer : PropertyDrawer
{
    private class PropertyData
    {
        public SerializedProperty useKeyProperty;
        public SerializedProperty keyQueryTypeProperty;
        public SerializedProperty intValueProperty;
        public SerializedProperty floatValueProperty;

        public SerializedProperty valueTypeProperty;
        public SerializedProperty blackboardSettingProperty;
    }

    private Dictionary<string, PropertyData> _propertyDataPerPropertyPath = new Dictionary<string, PropertyData>();
    private PropertyData _property;

    private float LineHeight { get { return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; } }

    private void Init(SerializedProperty property)
    {
        if (_propertyDataPerPropertyPath.TryGetValue(property.propertyPath, out _property))
        {
            return;
        }

        _property = new PropertyData();
        _property.useKeyProperty = property.FindPropertyRelative("useKey");
        _property.keyQueryTypeProperty = property.FindPropertyRelative("keyQueryType");
        _property.intValueProperty = property.FindPropertyRelative("intValue");
        _property.floatValueProperty = property.FindPropertyRelative("floatValue");

        _property.valueTypeProperty = property.FindPropertyRelative("valueType");
        _property.blackboardSettingProperty = property.FindPropertyRelative("blackboardSetting");
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

                using (new EditorGUI.IndentLevelScope())
                {
                    var blackboardSetting = (BlackboardSetting)_property.blackboardSettingProperty.objectReferenceValue;

                    // Name��`��
                    fieldRect.y += LineHeight;
                    var keyStringList = blackboardSetting.keyStrings.ToArray();
                    var keyIndex = EditorGUI.Popup(new Rect(fieldRect), "Key", _property.useKeyProperty.intValue, keyStringList);
                    _property.useKeyProperty.intValue = keyIndex;

                    var valueType = blackboardSetting.GetBlackBoardValueType(keyStringList[keyIndex]);

                    string[] popupList;
                    int selectIndex;

                    switch (valueType)
                    {
                        case BlackboardValueType.Boolean:
                            fieldRect.y += LineHeight;
                            popupList = Enum.GetNames(typeof(TransitionKeyQueryType)).Skip(2).Take(2).ToArray();
                            selectIndex = EditorGUI.Popup(new Rect(fieldRect), "Query Type", _property.keyQueryTypeProperty.enumValueIndex, popupList);
                            _property.keyQueryTypeProperty.enumValueIndex = selectIndex;
                            break;
                        case BlackboardValueType.Float:
                            fieldRect.y += LineHeight;
                            popupList = Enum.GetNames(typeof(TransitionKeyQueryType)).Skip(4).ToArray();
                            selectIndex = EditorGUI.Popup(new Rect(fieldRect), "Query Type", _property.keyQueryTypeProperty.enumValueIndex, popupList);
                            _property.keyQueryTypeProperty.enumValueIndex = selectIndex;

                            fieldRect.y += LineHeight;
                            EditorGUI.PropertyField(new Rect(fieldRect), _property.floatValueProperty);
                            break;
                        case BlackboardValueType.Integer:
                            fieldRect.y += LineHeight;
                            popupList = Enum.GetNames(typeof(TransitionKeyQueryType)).Skip(4).ToArray();
                            selectIndex = EditorGUI.Popup(new Rect(fieldRect), "Query Type", _property.keyQueryTypeProperty.enumValueIndex, popupList);
                            _property.keyQueryTypeProperty.enumValueIndex = selectIndex;

                            fieldRect.y += LineHeight;
                            EditorGUI.PropertyField(new Rect(fieldRect), _property.intValueProperty);
                            break;
                        case BlackboardValueType.MonoBehaviour:
                        case BlackboardValueType.GameObject:
                        case BlackboardValueType.Transform:
                            fieldRect.y += LineHeight;
                            popupList = Enum.GetNames(typeof(TransitionKeyQueryType)).Take(2).ToArray();
                            selectIndex = EditorGUI.Popup(new Rect(fieldRect), "Query Type", _property.keyQueryTypeProperty.enumValueIndex, popupList);
                            _property.keyQueryTypeProperty.enumValueIndex = selectIndex;
                            break;
                    }
                }
            }
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Init(property);
        // (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) x �s�� �ŕ`��̈�̍��������߂�
        return LineHeight * (property.isExpanded ? 4 : 1);
    }
}
