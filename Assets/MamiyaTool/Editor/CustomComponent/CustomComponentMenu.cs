using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class CustomComponentMenu
{
    #region Define
    private const string StandardSpritePath = "UI/Skin/UISprite.psd";
    private const string BackgroundSpritePath = "UI/Skin/Background.psd";
    private const string InputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
    private const string KnobPath = "UI/Skin/Knob.psd";
    private const string CheckmarkPath = "UI/Skin/Checkmark.psd";
    private const string DropdownArrowPath = "UI/SkinDropdownArrow.psd";
    private const string MaskPath = "UI/Skin/UIMask.psd";
    #endregion

    #region Common
    private static Transform GetParent()
    {
        Transform parent = Selection.activeTransform;
        Canvas canvas;

        if(parent == null) {
            canvas = GameObject.FindObjectOfType<Canvas>();
            if(canvas == null)
                canvas = GetCanvas(parent);
            parent = canvas.transform;
        } else {
            canvas = parent.GetComponentInParent<Canvas>();
            if(canvas == null) {
                canvas = GetCanvas(parent);
                parent = canvas.transform;
            }
        }
        return parent;
    }
    private static Canvas GetCanvas(Transform parent)
    {
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
        GameObject canvasObj = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObj.transform.SetParent(parent);
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        if(eventSystem == null) {
            GameObject eventSystemObj = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem = eventSystemObj.GetComponent<EventSystem>();
        }
        return canvas;
    }
    #endregion

    #region Custom Text
    [MenuItem("GameObject/UI/Custom/Text (不推荐)")]
    private static void AddCustomText()
    {
        Transform parent = GetParent();
        AddCustomText(parent);
    }
    private static void AddCustomText(Transform parent)
    {
        GameObject newObj = new GameObject("CustomText", typeof(CustomText));
        newObj.transform.SetParent(parent, false);
        newObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160f);
        newObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
        newObj.GetComponent<CustomText>().text = "New Text";
        newObj.GetComponent<CustomText>().color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 255f / 255f);
        Selection.activeObject = newObj;
    }
    #endregion

    #region Custom Image
    [MenuItem("GameObject/UI/Custom/Image")]
    private static void AddCustomImage()
    {
        Transform parent = GetParent();
        AddCustomImage(parent);
    }
    private static void AddCustomImage(Transform parent)
    {
        GameObject newObj = new GameObject("CustomImage", typeof(CustomImage));
        newObj.transform.SetParent(parent, false);
        Selection.activeObject = newObj;
    }
    #endregion

    #region Custom Button
    [MenuItem("GameObject/UI/Custom/Button")]
    private static void AddCustomButton()
    {
        Transform parent = GetParent();
        AddCustomButton(parent);
    }
    private static void AddCustomButton(Transform parent)
    {
        GameObject newObj = new GameObject("CustomButton", typeof(CustomImage), typeof(CustomButton));
        newObj.transform.SetParent(parent, false);
        newObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160f);
        newObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
        newObj.GetComponent<CustomImage>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(StandardSpritePath);
        newObj.GetComponent<CustomImage>().type = Image.Type.Sliced;

        GameObject newTextObj = new GameObject("CustomText", typeof(CustomText));
        newTextObj.transform.SetParent(newObj.transform);
        newTextObj.transform.localPosition = Vector3.zero;
        newTextObj.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        newTextObj.GetComponent<RectTransform>().anchorMax = Vector2.one;
        newTextObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160f);
        newTextObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
        newTextObj.GetComponent<CustomText>().text = "Button";
        newTextObj.GetComponent<CustomText>().color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 255f / 255f);
        newTextObj.GetComponent<CustomText>().alignment = TextAnchor.MiddleCenter;

        Selection.activeObject = newObj;
    }
    #endregion

    #region Custom TextMeshPro
    [MenuItem("GameObject/UI/Custom/Text - TextMeshPro")]
    private static void AddCustomTMP()
    {
        Transform parent = GetParent();
        AddCustomTMP(parent);
    }
    private static void AddCustomTMP(Transform parent)
    {
        GameObject newObj = new GameObject("CustomText (TMP)", typeof(CustomTMP));
        newObj.transform.SetParent(parent, false);
        newObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200f);
        newObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50f);
        newObj.GetComponent<CustomTMP>().text = "New Text";
        //SerializedObject
        Selection.activeObject = newObj;
    }
    #endregion
}