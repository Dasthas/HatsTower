using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HatsTower.Scripts
{
    [DefaultExecutionOrder(-10)]
    public class HatsCollection : MonoBehaviour
    {
        [SerializeField] private Hat hatPF;
        [SerializeField] private float hatsCount;
        [SerializeField] private Transform startPosTransform;
        [SerializeField] private float offsetBtw;
        private Vector2 topTowerPoint;
        private Sequence Seq;
        public void RemoveHat(Hat hat)
        {
            if (hat == null) return;
            if (ModelsHandler.Instance.Hats.Count == 0) return;

            if (ModelsHandler.Instance.Hats.Contains(hat))
            {
                topTowerPoint.y -= offsetBtw;
                ModelsHandler.Instance.TopTowerPos = topTowerPoint.y;
                var list = ModelsHandler.Instance.Hats;
                list.Remove(hat);
                if (list.Count == 0) GeneralControl.Instance.OnGameEnd(Utils.GameEndState.Win);
                else list.LastOrDefault().ChangeState(Hat.HatState.WaitForSwipe);
                ModelsHandler.Instance.Hats = list;
            }
        }

        private void SpawnHats()
        {
            for (int i = 0; i < hatsCount; i++)
            {
                topTowerPoint.y += offsetBtw;
                var hat = Instantiate(hatPF, topTowerPoint, default, transform);
                hat.ParentCollection = this;
                ModelsHandler.Instance.Hats.Add(hat);
                ModelsHandler.Instance.TopTowerPos = topTowerPoint.y;
                if (i % 2 == 1) hat.InitHat(Hat.HatQuality.Bad);
                else hat.InitHat(Hat.HatQuality.Good);
            }
        }

        private void Start()
        {
            Seq = DOTween.Sequence();
            Seq.SetEase(Ease.Linear)
               .SetAutoKill(false)
               .Append(transform.DORotate(new Vector3(0, 0, 5), 0, RotateMode.Fast))
               .Append(transform.DORotate(new Vector3(0, 0, -5), 4, RotateMode.Fast))
               .Append(transform.DORotate(new Vector3(0, 0, 5), 4, RotateMode.Fast))
               .OnComplete(() => Seq.Restart());

            topTowerPoint = startPosTransform.position;
            SpawnHats();

            ModelsHandler.Instance.Hats.LastOrDefault().ChangeState(Hat.HatState.WaitForSwipe);
        }
    }
}
