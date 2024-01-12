using UnityEngine;
using UnityEngine.UI;

public class CustomText : Text
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