using UnityEngine;
using UnityEngine.EventSystems;

namespace Figures
{
    public class FigureDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Figure _clonePrefab;
        
        private Figure _clone;
        private Figure _figure;
        
        private void Start()
        {
            _figure = GetComponent<Figure>();
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_figure.IsLocked) return;

            _clone = Instantiate(_clonePrefab, transform.position, Quaternion.identity);
            _clone.transform.SetParent(transform.root);
            _clone.FigureData = _figure.FigureData;
            
            _clone.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_clone == null) return;
            _clone.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_clone == null) return;

            _clone.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy(_clone.gameObject);
        }

    }
}