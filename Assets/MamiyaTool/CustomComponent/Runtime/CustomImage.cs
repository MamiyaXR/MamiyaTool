using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomImage : Image
{
    [HideInInspector] public bool needLocalization;
    [HideInInspector] public string imageName;
    protected override void Start()
    {
        base.Start();
        Localization();
    }
    public virtual void Localization()
    {
        if(!needLocalization)
            return;
    }
}