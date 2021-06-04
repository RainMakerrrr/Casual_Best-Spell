using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding
{
    public class DeckSelector : MonoBehaviour
    {
        [SerializeField] private Button[] _selectButtons;
        [SerializeField] private PlayerDeck[] _playerDecks;
        [SerializeField] private TextMeshProUGUI _currentDeckText;

        private Dictionary<Button, PlayerDeck> _decks = new Dictionary<Button, PlayerDeck>();

        private void Start()
        {
            for (int i = 0; i < _playerDecks.Length; i++)
            {
                _decks.Add(_selectButtons[i], _playerDecks[i]);
            }

            var firstButton = _decks.Keys.FirstOrDefault();
            EnablePlayerDeck(_decks[firstButton!]);
            DisplayDeckOrder(firstButton);


            foreach (var button in _decks.Keys)
            {
                button.onClick.AddListener(() =>
                {
                    EnablePlayerDeck(_decks[button]);
                    DisplayDeckOrder(button);
                });
            }
        }

        private void EnablePlayerDeck(PlayerDeck playerDeck)
        {
            playerDeck.gameObject.SetActive(true);

            foreach (var deck in _playerDecks.Except(new List<PlayerDeck> {playerDeck}))
            {
                deck.gameObject.SetActive(false);
            }
        }

        private void DisplayDeckOrder(Button button)
        {
            var order = int.Parse(button.GetComponentInChildren<TextMeshProUGUI>().text);
            _currentDeckText.text = $"Deck {order}";

            button.GetComponent<Image>().color = Color.red;

            foreach (var otherButton in _decks.Keys.Except(new List<Button> {button}))
            {
                otherButton.GetComponent<Image>().color = Color.green;
            }
        }
    }
}