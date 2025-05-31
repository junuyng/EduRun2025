using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIRanking : UIBase
{
    public Transform contentRoot;
    public GameObject firstRankGO;
    public GameObject secondRankGO;
    public GameObject thirdRankGO;
    public GameObject itemPrefab;

    [SerializeField]private Button closeButton;

    private void OnEnable()
    {
        if (UserDataManager.Instance == null)
        {
            return;
        }

        StartCoroutine(UserDataManager.Instance.GetRankingList(OnRankingReceived));
    }

    private void Start()
    {
        closeButton.onClick.AddListener(()=>UIManager.Instance.Hide<UIRanking>());
    }

    private void OnRankingReceived(List<RankingEntry> rankings)
    {
        if (rankings == null) return;
        PopulateRanking(rankings);
    }

    public void PopulateRanking(List<RankingEntry> rankings)
    {
        // 기존 아이템 제거
        foreach (Transform child in contentRoot)
        {
            if (child != firstRankGO.transform && child != secondRankGO.transform && child != thirdRankGO.transform)
                Destroy(child.gameObject);
        }

        for (int i = 0; i < rankings.Count; i++)
        {
            GameObject targetGO = null;

            if (i == 0) targetGO = firstRankGO;
            else if (i == 1) targetGO = secondRankGO;
            else if (i == 2) targetGO = thirdRankGO;
            else targetGO = Instantiate(itemPrefab, contentRoot);

            var view = targetGO.GetComponent<RankingItemView>();
            view.SetData(rankings[i]);
        }
    }
}