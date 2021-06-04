using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameMap.Locations
{
    public class LocationPanel : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        private Button _enterLocationButton;
        private LocationData _locationData;

        private void Start()
        {
            _enterLocationButton = GetComponentInChildren<Button>();
            _enterLocationButton.onClick.AddListener(LoadLocationScene);
            _closeButton.onClick.AddListener(DisablePanel);
        }

        public void SetLocationData(LocationData locationData)
        {
            _locationData = locationData;
            _rewardText.text = _locationData.LocationReward.ToString();
        }

        private void LoadLocationScene() => SceneManager.LoadScene(_locationData.LocationScene);
        private void DisablePanel() => gameObject.SetActive(false);
    }
}