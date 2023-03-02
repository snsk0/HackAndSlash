using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Players.Animators
{
    public static class AnimatorStateHashes
    {
        //�^�O�̃n�b�V��
        public static int Attack => Animator.StringToHash("Attack");
        public static int Jump => Animator.StringToHash("Jump");
        public static int Idle => Animator.StringToHash("Idle");
        public static int Run => Animator.StringToHash("Run");

        //���O�̃n�b�V��
        public static int FirstAttack => Animator.StringToHash("FirstAttack");
        public static int SecondAttack => Animator.StringToHash("SecondAttack");
        public static int ThirdAttack => Animator.StringToHash("ThirdAttack");
    }
}

