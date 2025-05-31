using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private Image previewImage;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private TextMeshProUGUI characterNameText;

    [SerializeField] private List<CharacterDataSO> allCharacters;

    private List<CharacterDataSO> availableCharacters = new List<CharacterDataSO>();
    private int currentIndex = 0;
    private AudioUnit unit;
    
    private void Start()
    {
      GameManager.Instance.OnUpdateCharacterInfo +=BuildAvailableCharacterList;
        UpdatePreview();
        unit = GetComponent<AudioUnit>();

        leftButton.onClick.AddListener(PreviousCharacter);
        rightButton.onClick.AddListener(NextCharacter);
    }

    private void BuildAvailableCharacterList()
    {
        availableCharacters.Clear(); // 중복 방지
        var ownedItems = GameManager.Instance.userItems;    
                Debug.Log(ownedItems.Count());
        foreach (var character in allCharacters)
        {
            if (character.isDefault)
            {
                availableCharacters.Add(character);
                continue;
            }

            bool owned = false;

            for (int i = 0; i < ownedItems.Count; i++)
            {
                var item = ownedItems[i];

                if (item.itemName == character.skinName)
                {
                    owned = true;
                    break;
                }
            }

            if (owned)
            {
                availableCharacters.Add(character);
            }
        }
    }

    private void UpdatePreview()
    {
        if (availableCharacters.Count == 0) return;

        var selected = availableCharacters[currentIndex];

        previewImage.sprite = selected.previewSprite;
        characterNameText.text = selected.skinName;
        
        GameManager.Instance.selectedSkin = selected.skinName;
    }

    private void PreviousCharacter()
    {
        unit.PlaySFX(SFX.Button);
        currentIndex = (currentIndex - 1 + availableCharacters.Count) % availableCharacters.Count;
        UpdatePreview();
    }

    private void NextCharacter()
    {
        unit.PlaySFX(SFX.Button);
        currentIndex = (currentIndex + 1) % availableCharacters.Count;
        UpdatePreview();
    }    
}


