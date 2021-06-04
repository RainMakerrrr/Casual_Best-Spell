using UnityEngine;

namespace Data.Figures
{
    [CreateAssetMenu(fileName = "New Figure Data", menuName = "Data / Figure Data")]
    public class FigureData : ScriptableObject
    {
        [SerializeField] private FigureType _figureType;
        [SerializeField] private Sprite _figureSprite;

        public FigureType FigureType => _figureType;
        public Sprite FigureSprite => _figureSprite;
    }
}