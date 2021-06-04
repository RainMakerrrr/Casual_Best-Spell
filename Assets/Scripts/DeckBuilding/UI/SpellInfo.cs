using System;
using System.Collections.Generic;
using System.Linq;
using Data.Spells;
using GameEconomy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding.UI
{
    public class SpellInfo : MonoBehaviour
    {
        public static event Action<SpellData> OnSpellUpgraded;

        [SerializeField] private Image _spellIcon;
        [SerializeField] private List<Image> _figuresImages = new List<Image>();
        [SerializeField] private TextMeshProUGUI _spellName;
        [SerializeField] private TextMeshProUGUI _spellLevel;
        [SerializeField] private TextMeshProUGUI _buttonText;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _closeButton;

        private SpellData _spellData;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(DisableInfoPanel);
            _upgradeButton.onClick.AddListener(UpgradeSpell);
        }

        private void DisableInfoPanel() => gameObject.SetActive(false);

        private void UpgradeSpell()
        {
            var playerGold = FindObjectOfType<PlayerGold>();
            if (!playerGold.RemoveGold(_spellData.CurrentUpgradeCost)) return;

            _spellData.UpgradeSpell();
            _spellData.DuplicatesCount = 1;

            OnSpellUpgraded?.Invoke(_spellData);

            gameObject.SetActive(false);
        }

        public void SetSpellData(SpellData spellData, bool isUpgrade)
        {
            _spellData = spellData;
            _upgradeButton.interactable = isUpgrade;
            _upgradeButton.GetComponent<Image>().color = isUpgrade ? Color.yellow : Color.grey;

            SetSpellInfo();
        }

        private void SetSpellInfo()
        {
            _spellIcon.sprite = _spellData.SpellIcon;
            _spellName.text = _spellData.Name;
            _spellLevel.text = $"Level {_spellData.CurrentUpgradeLevel + 1}";
            _buttonText.text = $"UPGRADE \n {_spellData.CurrentUpgradeCost}";

            for (int i = 0; i < _spellData.SpellRecipe.Count; i++)
            {
                _figuresImages[i].sprite = _spellData.SpellRecipe[i].FigureSprite;
            }

            foreach (var figureImage in _figuresImages.Where(figureImage => figureImage.sprite == null))
            {
                figureImage.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(DisableInfoPanel);
            _upgradeButton.onClick.RemoveListener(UpgradeSpell);
        }
    }
}