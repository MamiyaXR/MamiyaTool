using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomTMP : TextMeshProUGUI
{
    [HideInInspector] public bool needLocalization;
    [HideInInspector] public string key;
    protected override void Start()
    {
        base.Start();
        if(Application.isPlaying)
            Localization();
    }
    public virtual void Localization()
    {
        if(!needLocalization)
            return;
    }
}
