using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatsTower.Scripts
{
    public static class Utils
    {
        public const int START_HP = 5;
        public enum Direction
        {
            Left, Right
        }
        public enum GameEndState
        {
            Win, Lose
        }
        public static IEnumerator InvokeWithDelay(float t, Action action)
        {
            yield return new WaitForSeconds(t);
            action.Invoke();
        }

    }
}
