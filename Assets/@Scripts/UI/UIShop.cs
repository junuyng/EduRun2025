    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIShop : UIBase
    {
        [SerializeField]private Button closeBtn;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            closeBtn.onClick.AddListener(()=>UIManager.Instance.Hide<UIShop>());
        }

        private void BuyItem()
        {
            
        }

    }
