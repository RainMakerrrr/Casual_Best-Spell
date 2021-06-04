using System;
using GameEconomy.Data;
using UnityEngine;

namespace GameEconomy
{
    public class PlayerGold : MonoBehaviour
    {
        public event Action<int> OnGoldChanged;
        
        [SerializeField] private Gold _gold;

        private void Awake()
        {
            OnGoldChanged?.Invoke(_gold.PlayerGold);   
        }

        public void AddGold(int gold)
        {
            _gold.AddGold(gold);
            OnGoldChanged?.Invoke(_gold.PlayerGold);
        }

        public bool RemoveGold(int gold)
        {
            var isRemovingGold = _gold.RemoveGold(gold);
            OnGoldChanged?.Invoke(_gold.PlayerGold);

            return isRemovingGold;
        }
    }
}