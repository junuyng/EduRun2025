using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIBase
{
     private AudioUnit unit;
     
     //UI Elements
     [SerializeField]private Slider masterVol;
     [SerializeField]private Slider bgmVol;
     [SerializeField]private Slider sfxVol;
     
     [SerializeField] private Button closeBtn;
     [SerializeField] private Button saveBtn;
     
     private void Start()
     {
          unit = GetComponent<AudioUnit>();
          Init();
     }

     private void Init()
     {
          masterVol.onValueChanged.AddListener(AudioMixerController.Instance.SetMasterVolume);
          bgmVol.onValueChanged.AddListener(AudioMixerController.Instance.SetBGMVolume);
          sfxVol.onValueChanged.AddListener(AudioMixerController.Instance.SetSFXVolume);
          
          closeBtn.onClick.AddListener(OnClickedCloseBtn);
          saveBtn.onClick.AddListener(OnClickedSaveBtn);
     }


     private void OnClickedSaveBtn()
     {
          unit.PlaySFX(SFX.ButtonLow);
     }

     private void OnClickedCloseBtn()
     {
          unit.PlaySFX(SFX.ButtonLow);
          UIManager.Instance.Hide<UISettings>();
     }

}
