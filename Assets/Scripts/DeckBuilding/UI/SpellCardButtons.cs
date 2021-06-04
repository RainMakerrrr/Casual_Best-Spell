using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeckBuilding.UI
{
    public class SpellCardButtons : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _useButton;

        [SerializeField] private TextMeshProUGUI _upgradeButtonText;

        [SerializeField] private SpellInfo _spellInfo;

        private SpellCard _spellCard;


        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            
            _spellCard = GetComponent<SpellCard>();

            _infoButton.onClick.AddListener(DisplaySpellInfo);
            _removeButton.onClick.AddListener(RemoveSpellFromDeck);
            _upgradeButton.onClick.AddListener(UpgradeSpell);
            _useButton.onClick.AddListener(AddSpellToDeck);

            _upgradeButtonText.text =
                $"UPGRADE \n {_spellCard.SpellData.CurrentUpgradeCost}";
        }

        private void DisplaySpellInfo()
        {
            _spellInfo.gameObject.SetActive(true);
            _spellInfo.SetSpellData(_spellCard.SpellData, false);
            DisableAllButtons();
        }

        private void RemoveSpellFromDeck()
        {
            _spellCard.MoveToCollection();
            DisableAllButtons();
        }

        private void UpgradeSpell()
        {
            _spellInfo.gameObject.SetActive(true);
            _spellInfo.SetSpellData(_spellCard.SpellData, true);
            DisableAllButtons();
        }

        private void AddSpellToDeck()
        {
            _spellCard.MoveToDeck();
            DisableAllButtons();
        }

        private void EnableDeckButtons(bool isDeckCard, bool isUpgradable)
        {
            _removeButton.gameObject.SetActive(isDeckCard);
            _useButton.gameObject.SetActive(!isDeckCard);

            if (isUpgradable)
            {
                _upgradeButton.gameObject.SetActive(true);
                _infoButton.gameObject.SetActive(false);
            }
            else
            {
                _infoButton.gameObject.SetActive(true);
                _upgradeButton.gameObject.SetActive(false);
            }
        }

        private void DisableAllButtons()
        {
            _infoButton.gameObject.SetActive(false);
            _removeButton.gameObject.SetActive(false);
            _upgradeButton.gameObject.SetActive(false);
            _useButton.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var isDeckCard = GetComponentInParent<PlayerDeck>() != null;
            var isCollectionCard = GetComponentInParent<CardsCollection>() != null;

            if (isDeckCard)
            {
                EnableDeckButtons(true, _spellCard.SpellData.EnoughDuplicates);
            }
            else if (isCollectionCard)
            {
                EnableDeckButtons(false, _spellCard.SpellData.EnoughDuplicates);
            }
        }

        // private void OnDisable()
        // {
        //     _infoButton.onClick.RemoveListener(DisplaySpellInfo);
        //     _removeButton.onClick.RemoveListener(RemoveSpellFromDeck);
        //     _upgradeButton.onClick.RemoveListener(UpgradeSpell);
        //     _useButton.onClick.RemoveListener(AddSpellToDeck);
        // }
    }
}