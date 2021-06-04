using System.Collections.Generic;
using System.Linq;
using Data.Characters;
using GameMap.Data;
using UnityEngine;

namespace GameMap.Quests.UI
{
    public class MapQuestsPanel : MonoBehaviour
    {
        [SerializeField] private List<CustomerPanel> _questCustomersPanels = new List<CustomerPanel>();
        [SerializeField] private MapData _mapData;
        
        private void Start()
        {
            _questCustomersPanels = GetComponentsInChildren<CustomerPanel>().ToList();
            SetCustomersPanel();
            
            PlayerQuests.OnCharacterAdded += CharacterAcceptHandler;
        }
        
        private void SetCustomersPanel()
        {
            for (int i = 0; i < _mapData.AcceptedCharacters.Count; i++)
            {
                _questCustomersPanels[i].SetCharacter(_mapData.AcceptedCharacters[i]);
            }
            
        }
        
        private void CharacterAcceptHandler(Character character)
        {
            var questPanel = _questCustomersPanels.FirstOrDefault(q => !q.IsPanelBusy);
            if (questPanel == null) return;

            questPanel.SetCharacter(character);
        }

        // private void OnDisable()
        // {
        //     PlayerQuests.OnCharacterAdded -= CharacterAcceptHandler;
        // }
        
    }
}