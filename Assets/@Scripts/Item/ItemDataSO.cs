using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/ItemData")]
public class ItemDataSO : ScriptableObject
{
    public int itemId;
    public string itemName;
    public int price;
}