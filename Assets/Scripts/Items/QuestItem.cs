using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Quest Item", menuName = "Items / New Quest Item")]
    public class QuestItem : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _itemIcon;

        public string Name => _name;
        public Sprite ItemIcon => _itemIcon;
    }
}