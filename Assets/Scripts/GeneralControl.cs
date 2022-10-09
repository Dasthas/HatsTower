using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatsTower.Scripts
{
    [DefaultExecutionOrder(-101)]
    public class GeneralControl : MonoBehaviour
    {
        public enum SwipeResult
        {
            True,
            False,
            Void
        }

        public static GeneralControl Instance;
        private Vector2 startInputPos, endInputPos;
        private Touch touch;
        private bool mouseControl = false;
        private bool recieveSwipe = false;

        void Awake()
        {
            OnGameStart += GameStart;
            Instance = this;
            if (Input.mousePresent) mouseControl = true;
        }

        private void Swipe(Utils.Direction dir)
        {
            var result = OnSwipeEvent.Invoke(dir);
            Debug.Log("Swipe " + dir + ", " + result);
            if (result == SwipeResult.False)
            {
                ModelsHandler.Instance.HP = ModelsHandler.Instance.HP - 1;
                ModelsHandler.Instance.FastMessage = "Bad";
            }
            else if (result == SwipeResult.True)
            {
                ModelsHandler.Instance.FastMessage = "Yes!";
            }
        }
      
        void Update()
        {
            if (!recieveSwipe) return; 
            
            if (mouseControl)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startInputPos = Input.mousePosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    endInputPos = Input.mousePosition;
                    if (endInputPos.x > startInputPos.x)
                        Swipe(Utils.Direction.Right);
                    else if (endInputPos.x < startInputPos.x)
                        Swipe(Utils.Direction.Left);
                }
            }
            else if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startInputPos = touch.position;
                        break;
                    case TouchPhase.Ended:
                        endInputPos = touch.position;
                        if (endInputPos.x > startInputPos.x)
                            Swipe(Utils.Direction.Right);
                        else if (endInputPos.x < startInputPos.x)
                            Swipe(Utils.Direction.Left);
                        break;
                }

            }
            
        }
        public delegate SwipeResult OnSwipe(Utils.Direction dir);
        public event OnSwipe OnSwipeEvent; 
        public void OnGameEnd(Utils.GameEndState state)
        {
            recieveSwipe = false;
            switch (state)
            {
                case Utils.GameEndState.Win:
                    StartCoroutine(Utils.InvokeWithDelay(0.1f, () => ModelsHandler.Instance.FastMessage = "WIN!"));
                    break;
                case Utils.GameEndState.Lose:
                    StartCoroutine(Utils.InvokeWithDelay(0.1f, () => ModelsHandler.Instance.FastMessage = "Lose"));
                    break;
            }
        }
        public Action OnGameStart; 
        private void GameStart()
        {
            StartCoroutine(Utils.InvokeWithDelay(1, () => recieveSwipe = true));
        }
    }
}
