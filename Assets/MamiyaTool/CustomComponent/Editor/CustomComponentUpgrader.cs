using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public static class CustomComponentUpgrader
{
    #region Common
    private static void CopyComponent<T1, T2>(T1 source, T2 target)
    {
        PropertyInfo[] t1Infos = source.GetType().GetProperties();
        Type type2 = target.GetType();
        foreach(PropertyInfo info in t1Infos) {
            PropertyInfo tInfo = type2.GetProperty(info.Name);
            if(tInfo == null || !tInfo.CanWrite)
                continue;
            tInfo.SetValue(target, info.GetValue(source));
        }
    }
    private static void GetAllComponentsInChildren<T>(Transform transform, bool includeInactive, ref List<T> result)
    {
        if(transform.childCount > 0) {
            for(int i = 0; i < transform.childCount; ++i) {
                Transform newTarget = transform.GetChild(i);
                GetAllComponentsInChildren<T>(newTarget, includeInactive, ref result);
            }
        }

        result.AddRange(transform.GetComponentsInChildren<T>(includeInactive));
    }
    #endregion

    #region Custom Text
    [MenuItem("CONTEXT/Text/Change To CustomText")]
    private static void TextToCustomText(MenuCommand cmd)
    {
        if(cmd.context.GetType() != typeof(Text))
            return;
        TextToCustomText(cmd.context as Text);
    }
    private static void TextToCustomText(Text text)
    {
        if(text == null)
            return;
        GameObject gameObject = text.gameObject;

        GameObject tempObj = new GameObject("Temp", typeof(CustomText));
        CopyComponent(text, tempObj.GetComponent<CustomText>());

        GameObject.DestroyImmediate(text);
        CustomText customText = gameObject.AddComponent<CustomText>();
        CopyComponent(tempObj.GetComponent<CustomText>(), customText);
        GameObject.DestroyImmediate(tempObj);
        EditorUtility.SetDirty(gameObject);
    }

    [MenuItem("CONTEXT/CustomText/Change To Text")]
    private static void CustomTextToText(MenuCommand cmd)
    {
        if(cmd.context.GetType() != typeof(CustomText))
            return;
        CustomTextToText(cmd.context as CustomText);
    }
    private static void CustomTextToText(CustomText text)
    {
        if(text == null)
            return;
        GameObject gameObject = text.gameObject;

        GameObject tempObj = new GameObject("Temp", typeof(Text));
        CopyComponent(text, tempObj.GetComponent<Text>());

        GameObject.DestroyImmediate(text);
        Text customText = gameObject.AddComponent<Text>();
        CopyComponent(tempObj.GetComponent<Text>(), customText);
        GameObject.DestroyImmediate(tempObj);
        EditorUtility.SetDirty(gameObject);
    }
    #endregion

    #region Custom Image
    [MenuItem("CONTEXT/Image/Change To CustomImage")]
    private static void ImageToCustomImage(MenuCommand cmd)
    {
        if(cmd.context.GetType() != typeof(Image))
            return;
        ImageToCustomImage(cmd.context as Image);
    }
    private static void ImageToCustomImage(Image image)
    {
        if(image == null)
            return;
        GameObject gameObject = image.gameObject;

        GameObject tempObj = new GameObject("Temp", typeof(CustomImage));
        CopyComponent(image, tempObj.GetComponent<CustomImage>());

        GameObject.DestroyImmediate(image);
        CustomImage customImage = gameObject.AddComponent<CustomImage>();
        CopyComponent(tempObj.GetComponent<CustomImage>(), customImage);
        GameObject.DestroyImmediate(tempObj);
        EditorUtility.SetDirty(gameObject);
    }
    [MenuItem("CONTEXT/CustomImage/Change To Image")]
    private static void CustomImageToImage(MenuCommand cmd)
    {
        if(cmd.context.GetType() != typeof(CustomImage))
            return;
        CustomImageToImage(cmd.context as CustomImage);
    }
    private static void CustomImageToImage(CustomImage image)
    {
        if(image == null)
            return;
        GameObject gameObject = image.gameObject;

        GameObject tempObj = new GameObject("Temp", typeof(Image));
        CopyComponent(image, tempObj.GetComponent<Image>());

        GameObject.DestroyImmediate(image);
        Image customImage = gameObject.AddComponent<Image>();
        CopyComponent(tempObj.GetComponent<Image>(), customImage);
        GameObject.DestroyImmediate(tempObj);
        EditorUtility.SetDirty(gameObject);
    }
    #endregion

    #region Custom Button
    [MenuItem("CONTEXT/Button/Change to CustomButton")]
    private static void ButtonToCustomButton(MenuCommand cmd)
    {
        if(cmd.context.GetType() != typeof(Button))
            return;
        ButtonToCustomButton(cmd.context as Button);
    }
    private static void ButtonToCustomButton(Button button)
    {
        if(button == null)
            return;
        GameObject gameObject = button.gameObject;

        GameObject tempObj = new GameObject("Temp", typeof(CustomButton));
        CopyComponent(button, tempObj.GetComponent<CustomButton>());

        GameObject.DestroyImmediate(button);
        CustomButton customButton = gameObject.AddComponent<CustomButton>();
        CopyComponent(tempObj.GetComponent<CustomButton>(), customButton);
        GameObject.DestroyImmediate(tempObj);
        EditorUtility.SetDirty(gameObject);
    }
    [MenuItem("CONTEXT/CustomButton/Change to Button")]
    private static void CustomButtonToButton(MenuCommand cmd)
    {
        if(cmd.context.GetType() != typeof(CustomButton))
            return;
        CustomButtonToButton(cmd.context as CustomButton);
    }
    private static void CustomButtonToButton(CustomButton button)
    {
        if(button == null)
            return;
        GameObject gameObject = button.gameObject;

        GameObject tempObj = new GameObject("Temp", typeof(Button));
        CopyComponent(button, tempObj.GetComponent<Button>());

        GameObject.DestroyImmediate(button);
        Button customButton = gameObject.AddComponent<Button>();
        CopyComponent(tempObj.GetComponent<Button>(), customButton);
        GameObject.DestroyImmediate(tempObj);
        EditorUtility.SetDirty(gameObject);
    }
    #endregion

    #region Custom TextMeshPro
    [MenuItem("CONTEXT/TextMeshProUGUI/Change To CustomTMP")]
    private static void TMPToCustomTMP(MenuCommand cmd)
    {
        if(cmd.context.GetType() != typeof(TextMeshProUGUI))
            return;
        TMPToCustomTMP(cmd.context as TextMeshProUGUI);
    }
    private static void TMPToCustomTMP(TextMeshProUGUI text)
    {
        if(text == null)
            return;
        GameObject gameObject = text.gameObject;

        GameObject tempObj = new GameObject("Temp", typeof(CustomTMP));
        CopyComponent(text, tempObj.GetComponent<CustomTMP>());

        GameObject.DestroyImmediate(text);
        CustomTMP customTMP = gameObject.AddComponent<CustomTMP>();
        CopyComponent(tempObj.GetComponent<CustomTMP>(), customTMP);
        GameObject.DestroyImmediate(tempObj);
        EditorUtility.SetDirty(gameObject);
    }

    [MenuItem("CONTEXT/CustomTMP/Change To TextMeshProUGUI")]
    private static void CustomTMPToTMP(MenuCommand cmd)
    {
        if(cmd.context.GetType() != typeof(CustomTMP))
            return;
        CustomTMPToTMP(cmd.context as CustomTMP);
    }
    private static void CustomTMPToTMP(CustomTMP text)
    {
        if(text == null)
            return;
        GameObject gameObject = text.gameObject;

        GameObject tempObj = new GameObject("Temp", typeof(TextMeshProUGUI));
        CopyComponent(text, tempObj.GetComponent<TextMeshProUGUI>());

        GameObject.DestroyImmediate(text);
        TextMeshProUGUI customTMP = gameObject.AddComponent<TextMeshProUGUI>();
        CopyComponent(tempObj.GetComponent<TextMeshProUGUI>(), customTMP);
        GameObject.DestroyImmediate(tempObj);
        EditorUtility.SetDirty(gameObject);
    }
    #endregion

    #region Menu
    [MenuItem("Edit/Custom Component/Upgrade all Text to CustomText")]
    private static void UpgradeAllTextToCustomText()
    {
        GameObject selection = Selection.activeGameObject;
        if(selection == null) {
            Debug.Log("未选中GameObject");
            return;
        }

        List<Text> texts = new List<Text>();
        if(selection.GetComponent<Text>() != null)
            texts.Add(selection.GetComponent<Text>());
        GetAllComponentsInChildren(selection.transform, true, ref texts);
        for(int i = 0; i < texts.Count; ++i)
            TextToCustomText(texts[i]);
        texts.Clear();
    }
    [MenuItem("Edit/Custom Component/Upgrade all Image to CustomImage")]
    private static void UpgradeAllImageToCustomImage()
    {
        GameObject selection = Selection.activeGameObject;
        if(selection == null) {
            Debug.Log("未选中GameObject");
            return;
        }

        List<Image> images = new List<Image>();
        if(selection.GetComponent<Image>() != null)
            images.Add(selection.GetComponent<Image>());
        GetAllComponentsInChildren(selection.transform, true, ref images);
        for(int i = 0; i < images.Count; ++i)
            ImageToCustomImage(images[i]);
        images.Clear();
    }
    [MenuItem("Edit/Custom Component/Upgrade all Button to CustomButton")]
    private static void UpgradeAllButtonToCustomButton()
    {
        GameObject selection = Selection.activeGameObject;
        if(selection == null) {
            Debug.Log("未选中GameObject");
            return;
        }

        List<Button> buttons = new List<Button>();
        if(selection.GetComponent<Button>() != null)
            buttons.Add(selection.GetComponent<Button>());
        GetAllComponentsInChildren(selection.transform, true, ref buttons);
        for(int i = 0; i < buttons.Count; ++i)
            ButtonToCustomButton(buttons[i]);
        buttons.Clear();
    }
    [MenuItem("Edit/Custom Component/Upgrade all TextMeshProUGUI to CustomTMP")]
    private static void UpgradeAllTMPToCustomTMP()
    {
        GameObject selection = Selection.activeGameObject;
        if(selection == null) {
            Debug.Log("未选中GameObject");
            return;
        }

        List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        if(selection.GetComponent<TextMeshProUGUI>() != null)
            texts.Add(selection.GetComponent<TextMeshProUGUI>());
        GetAllComponentsInChildren(selection.transform, true, ref texts);
        for(int i = 0; i < texts.Count; ++i)
            TMPToCustomTMP(texts[i]);
        texts.Clear();
    }
    #endregion
}