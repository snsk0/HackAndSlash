using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Review.StateMachines.Editor
{
    public class StateTemplateModifier : UnityEditor.AssetModificationProcessor
    {
        const string StateObjectTemplate =
            "using UnityEngine;\n" +
            "\n" +
            "namespace Review.StateMachines.States.StateObjects" +
            "{\n" +
            "   public class #CLASSNAME# : BaseStateObject\n" +
            "   {\n" +
            "       public override string stateName { get; protected set; } = \"#STATENAME#\";\n" +
            "       public override BaseState state { get; protected set; } = new #STATENAME#();\n" +
            "   }\n" +
            "}\n" +
            "";

        const string StateTemplate =
            "\n" +
            "namespace Review.StateMachines.States" +
            "{\n" +
            "   public class #CLASSNAME# : BaseState\n" +
            "   {\n" +
            "\n" +
            "   }\n" +
            "}\n" +
            "";

        const string StateObjectFilePath = "Assets/Scripts/Review/StateMachines/States/StataObjects/";
        const string StateFilePath = "Assets/Scripts/Review/StateMachines/States/";
        const string StateScriptableObjectFilePath = "Assets/Scripts/Review/StateMachines/States/StataObjects/ScriptableObjects/";

        private static void OnWillCreateAsset(string path)
        {
            //�t�@�C���̍쐬�ꏊ���m�F����
            if (path.StartsWith(StateObjectFilePath))
            {
                //�쐬���ꂽ�t�@�C����.cs�t�@�C�����ǂ����𒲂ׂ�
                path = path.Replace(".meta", "");
                int index = path.LastIndexOf(".");
                if (index < 0) return;
                string fileExtension = path.Substring(index);
                if (fileExtension != ".cs") return;

                //�N���X���̖�����StateObject���܂܂�Ă��邩�m�F����
                index = path.LastIndexOf("/");
                string className = path.Substring(index + 1).Replace(".cs", "");
                if (!className.EndsWith("StateObject")) return;

                index = Application.dataPath.LastIndexOf("Assets");
                path = Application.dataPath.Substring(0, index) + path;

                index = className.LastIndexOf("Object");
                string stateName = className.Substring(0, index);
                //StateClass���쐬����
                CreateStateClassFile(stateName);

                string content = StateObjectTemplate.Replace("#CLASSNAME#", className).Replace("#STATENAME#", stateName);
                File.WriteAllText(path, content);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else if (path.StartsWith(StateFilePath))
            {
                //�쐬���ꂽ�t�@�C����.cs�t�@�C�����ǂ����𒲂ׂ�
                path = path.Replace(".meta", "");
                int index = path.LastIndexOf(".");
                if (index < 0) return;
                string fileExtension = path.Substring(index);
                if (fileExtension != ".cs") return;

                //�N���X���̖�����State���܂܂�Ă��邩�m�F����
                index = path.LastIndexOf("/");
                string className = path.Substring(index + 1).Replace(".cs", "");
                if (!className.EndsWith("State")) return;

                index = Application.dataPath.LastIndexOf("Assets");
                path = Application.dataPath.Substring(0, index) + path;
                //StateObjectClass���쐬����
                CreateStateObjectFile(className);

                string content = StateTemplate.Replace("#CLASSNAME#", className);
                File.WriteAllText(path, content);
                AssetDatabase.Refresh();
            }
        }

        private static bool CreateStateClassFile(string className)
        {
            if (File.Exists($"{StateFilePath}{className}.cs"))
            {
                return false;
            }

            string content = StateTemplate.Replace("#CLASSNAME#", className);
            //TextAsset asset = new TextAsset(content);
            string path = $"{StateFilePath}{className}.cs";
            File.WriteAllText(path, content);
            AssetDatabase.ImportAsset(path);
            return true;
        }

        private static bool CreateStateObjectFile(string stateName)
        {
            if (File.Exists($"{StateObjectFilePath}{stateName}Object.cs"))
            {
                return false;
            }

            string content = StateObjectTemplate.Replace("#CLASSNAME#", $"{stateName}Object").Replace("#STATENAME#", stateName);
            //TextAsset asset = new TextAsset(content);
            string path = $"{StateObjectFilePath}{stateName}Object.cs";
            File.WriteAllText(path, content);
            AssetDatabase.ImportAsset(path);
            return true;
        }
    }
}

