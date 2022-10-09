using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HatsTower.Scripts
{
    public class TopUI : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> HPList;
        [SerializeField] private Transform ListItemsContent;
        [SerializeField] private GameObject HealthPointPF;
        [SerializeField] private Button startButton;
        [SerializeField] private Slider slider;

        [SerializeField] private GameObject MessageObject;
        private TextMeshProUGUI text;
        private int startHP;
        private Sequence Seq;
        private float maxCount;
        void Start()
        {
            startButton.onClick.AddListener(() =>
            {
                GeneralControl.Instance.OnGameStart.Invoke();
                startButton.gameObject.SetActive(false);
            });
            text = MessageObject.GetComponent<TextMeshProUGUI>();
            text.text = "Start";
            Seq = DOTween.Sequence();

            Seq.SetAutoKill(false)
                .SetEase(Ease.Linear)
               .Join(text.DOFade(255, 0.1f))
               .Join(MessageObject.transform.DOScale(3, 0.5f))
               .Insert(0.5f, MessageObject.transform.DOScale(0, 0.5f))
               .Append(text.DOFade(0, 0.5f));

            startHP = ModelsHandler.Instance.HP;
            for (int i = 0; i < startHP; i++)
            {
                var obj = Instantiate(HealthPointPF, ListItemsContent);
                HPList.Add(obj);
            }

            maxCount = ModelsHandler.Instance.Hats.Count;
            ModelsHandler.Instance.OnHatsChangedEvent += (count) => slider.value = count/maxCount;
            ModelsHandler.Instance.OnHPChangedEvent += HPChanged;
            ModelsHandler.Instance.OnFastMessageSpawnEvent += SpawnFastMessage;
        }

        private void HPChanged(int hp)
        {
            if (hp < startHP)
            {
                if (HPList.Count > 0)
                {
                    var id = HPList.IndexOf(HPList.Last());
                    Destroy(HPList[id]);
                    HPList.RemoveAt(id);
                }
            }
            else Instantiate(HealthPointPF, ListItemsContent);

            if (hp == 0)
            {
                GeneralControl.Instance.OnGameEnd(Utils.GameEndState.Lose);
                ModelsHandler.Instance.OnHPChangedEvent -= HPChanged;
                return;
            }
        }

        private void SpawnFastMessage(string message)
        {
            text.text = message;

            Seq.Restart();
        }
    }
}
