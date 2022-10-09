using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatsTower.Scripts
{
    [DefaultExecutionOrder(-99)]
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float moveTime;
        bool waitForGameStart = true;
        private float cachedPos;

        void Start()
        {
            ModelsHandler.Instance.OnTopTowerPosChangedEvent += MoveCamera;
            GeneralControl.Instance.OnGameStart += () => { waitForGameStart = false; MoveCamera(cachedPos); };
        }

        private void MoveCamera(float posY)
        {
            cachedPos = posY;
            if (waitForGameStart) return;
            transform.DOMoveY(posY, moveTime);
        }
    }
}
