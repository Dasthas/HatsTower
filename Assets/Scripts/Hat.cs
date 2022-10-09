using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HatsTower.Scripts
{
    public class Hat : MonoBehaviour
    {
        public enum HatQuality
        {
            Good,
            Bad
        }

        public enum HatState
        {
            Sleeping,
            WaitForSwipe,
            WaitForDestroy
        }

        [Serializable]
        public class HatSetting
        {
            public HatQuality quality;
            public Color color;
        }

        [SerializeField] private List<HatSetting> hatsSettings = new();

        public HatsCollection ParentCollection { get; set; }
        public HatQuality Quality { get; set; }

        [Header("Animation Settings")]
        [SerializeField] private Ease ease;
        [SerializeField] private float flyX;
        [SerializeField] private float timeFlyX;
        [SerializeField] private float flyY;
        [SerializeField] private float timeFlyY;

        private Image hatImage;
        private HatState State = HatState.Sleeping;

        public void ChangeState(HatState state)
        {
            State = state;
            switch (state)
            {
                case HatState.Sleeping: 
                    break;
                case HatState.WaitForDestroy:
                    GeneralControl.Instance.OnSwipeEvent -= FlyAway;
                    break;
                case HatState.WaitForSwipe:
                    GeneralControl.Instance.OnSwipeEvent += FlyAway;
                    break;
            }
        }
        public void InitHat(HatQuality quality)
        {
            hatImage = GetComponent<Image>();
            Quality = quality;

           var sett = hatsSettings.Where(obj => obj.quality == quality).FirstOrDefault();

            hatImage.color = sett.color;
        }

        public GeneralControl.SwipeResult FlyAway(Utils.Direction dir)
        {
            ChangeState(HatState.WaitForDestroy);
            var t = transform;
            ParentCollection.RemoveHat(this);
            GeneralControl.SwipeResult result;
            switch (dir)
            {
                case Utils.Direction.Left:
                    flyX *= -1;
                    if (Quality == HatQuality.Good)
                        result = GeneralControl.SwipeResult.True;
                    else
                        result = GeneralControl.SwipeResult.False;
                    break;
                case Utils.Direction.Right:
                    if (Quality == HatQuality.Bad)
                        result = GeneralControl.SwipeResult.True;
                    else
                        result = GeneralControl.SwipeResult.False;
                    break;
                default:
                    Debug.LogWarning("Void Swipe Result");
                    result = GeneralControl.SwipeResult.Void;
                    break;
            }
            Sequence SeqNull = DOTween.Sequence();
            SeqNull.SetEase(ease)
                      .Join(t.DOMoveX(t.position.x + flyX, timeFlyX / 4))
                      .Join(t.DOMoveY(t.position.y + 1, timeFlyX / 4))
                      .Join(t.DORotate(new Vector3(0, 0, 360), timeFlyX / 4, RotateMode.FastBeyond360))
                      .Append(t.DOMoveY(t.position.y - (flyY * 3), timeFlyY))
                      .Join(t.DORotate(new Vector3(0, 0, 360), timeFlyX / 2, RotateMode.FastBeyond360))
                      .Join(t.DOMoveX(t.position.x + (flyX * 3), timeFlyX))
                      .OnComplete(() => Destroy(gameObject));
            return result;
        }
    }
}
