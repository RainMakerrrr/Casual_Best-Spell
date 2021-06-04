using GameMap.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap.Locations
{
    public class LocationPoint : MonoBehaviour
    {
        [SerializeField] private LocationData _locationData;
        [SerializeField] private LocationPanel _locationPanel;

        [SerializeField] private Sprite _openLocationButton;
        [SerializeField] private Sprite _closeLocationButton;
        [SerializeField] private Sprite _passedLocationButton;

        private Button _button;
        private Image _point;

        private bool _isLocked;

        public bool IsLocked
        {
            set
            {
                _isLocked = value;
                SetLockButton(_isLocked);
            }
        }

        public LocationData LocationData => _locationData;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _point = GetComponent<Image>();

            _button.onClick.AddListener(LocationPointClickHandler);
        }

        private void LocationPointClickHandler()
        {
            _locationPanel.gameObject.SetActive(true);
            _locationPanel.SetLocationData(_locationData);
        }

        private void SetLockButton(bool isLocked)
        {
            if (_locationData.IsPassed)
            {
                _point.sprite = _passedLocationButton;
                return;
            }

            _button.interactable = !isLocked;
            _point.sprite = isLocked ? _closeLocationButton : _openLocationButton;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(LocationPointClickHandler);
        }
    }
}