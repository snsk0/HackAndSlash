using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Review.StateMachines
{
    public abstract class StateMachineController
    {
        protected StateMachineType stateMachineType;

        protected abstract string settingFilePath { get; set; }

        private StateMachine usingStateMachine;

        public GameObject targetObject { get; private set; }

        protected StateMachineController(GameObject targetObject, StateMachineFactory stateMachineFactory)
        {
            this.targetObject = targetObject;

            if(settingFilePath == "")
            {
                Debug.LogWarning("�X�e�[�g�}�V���̐ݒ�t�@�C���p�X���ݒ肳��Ă��܂���");
                return;
            }

            Addressables.LoadAssetAsync<StateMachineSetting>(settingFilePath).Completed += setting =>
            {
                if (setting.Result == null)
                {
                    Debug.LogError($"�X�e�[�g�}�V���̐ݒ�t�@�C���p�X������������܂���\nPath{settingFilePath}");
                    return;
                }
                usingStateMachine=stateMachineFactory.CreateStateMachine(setting.Result);
            };
        }
    }
}

