using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "SO/CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    public string skinName;        
    public RuntimeAnimatorController animatorController;
    public Sprite previewSprite;
    public bool isDefault;         
}