using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatsTower.Scripts
{
    [DefaultExecutionOrder(-100)]
    public class ModelsHandler : MonoBehaviour
    {
        public static ModelsHandler Instance;

        public List<Hat> Hats { get; set; } = new();

        private int hp;
        public int HP
        {
            get { return hp; }
            set
            {
                OnHPChangedEvent.Invoke(value);
                hp = value;
            }
        }
        public delegate void OnHPChanged(int hp);
        public event OnHPChanged OnHPChangedEvent;
        
        private float topTowerPos;
        public float TopTowerPos
        {
            get { return topTowerPos; }
            set
            {
                OnTopTowerPosChangedEvent.Invoke(value);
                topTowerPos = value;
            }
        }
        public delegate void OnTopTowerPosChanged(float pos);
        public event OnTopTowerPosChanged OnTopTowerPosChangedEvent;

        public string FastMessage
        {
            set
            {
                OnFastMessageSpawnEvent.Invoke(value);
            }
        }
        public delegate void OnFastMessageSpawn(string message);
        public event OnFastMessageSpawn OnFastMessageSpawnEvent;

        private void Awake()
        {
            OnHPChangedEvent += (hp) => Debug.Log("OnHPChangedEvent " + hp);
            OnTopTowerPosChangedEvent += (topTowerPos) => Debug.Log("OnTopTowerPosChangedEvent");
            OnFastMessageSpawnEvent += Debug.Log;

            HP = Utils.START_HP;
            Instance = this;
        }
    }
}
