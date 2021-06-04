using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Player Items", menuName = "Player Items")]
    public class PlayerItems : ScriptableObject
    {
        [SerializeField] private List<QuestItem> _items = new List<QuestItem>();

        public List<QuestItem> Items => _items;
        
        
    }
}