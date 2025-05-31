using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingItemView : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI expText;

    public void SetData(RankingEntry entry)
    {
        nameText.text = entry.userName;
        rankText.text = $"#{entry.userRank}";
        expText.text = $"{entry.totalexp:N0}";
    }
}