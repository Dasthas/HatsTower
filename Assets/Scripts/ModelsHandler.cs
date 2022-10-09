using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HatsTower.Scripts
{
    [DefaultExecutionOrder(-100)]
    public class ModelsHandler : MonoBehaviour
    {
        public static ModelsHandler Instance;

        private List<Hat> hats;
        public List<Hat> Hats
        {
            get { return hats; }
            set
            {
                OnHatsChangedEvent.Invoke(value.Count);
                hats = value;
            }
        }
        public delegate void OnHatsChanged(int count);
        public event OnHatsChanged OnHatsChangedEvent;

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
            hats = new();
            OnHPChangedEvent += (hp) => Debug.Log("OnHPChangedEvent " + hp);
            OnTopTowerPosChangedEvent += (topTowerPos) => Debug.Log("OnTopTowerPosChangedEvent");
            OnFastMessageSpawnEvent += Debug.Log;

            HP = Utils.START_HP;
            Instance = this;
        }
    }
}
