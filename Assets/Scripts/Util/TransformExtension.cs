using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    /// <summary>
    /// �w�肵���������܂ޖ��O�̎q�����݂���ΕԂ��B���݂��Ȃ��Ƃ�null��Ԃ��B
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static Transform GetChild(this Transform transform, string keyword)
    {
        var count = transform.childCount;
        if (count == 0)
        {
            if(transform.parent==null)
                Debug.LogWarning("�q�����݂��܂���");
            return null;
        }

        for (int i = 0; i < count; i++)
        {
            
            var child = transform.GetChild(i);

            var result = child.GetChild(keyword);
            if (result != null)
                return result;

            var IsContain = child.name.Contains("Head");
            if (IsContain)
                return child;
        }

        if(transform.parent==null)
            Debug.LogWarning($"�u{keyword}�v���܂ގq�͌�����܂���ł���");
        return null;
    }
}
