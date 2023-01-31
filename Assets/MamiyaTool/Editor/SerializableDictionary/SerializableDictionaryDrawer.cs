using UnityEngine;
using UnityEditor;

namespace MamiyaTool
{
    [CustomPropertyDrawer(typeof(SerializableDictionary), true)]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        private SerializedProperty listProperty;
        private SerializedProperty GetListProperty(SerializedProperty property)
        {
            if(listProperty == null)
                listProperty = property.FindPropertyRelative("list");
            return listProperty;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty temp = GetListProperty(property);
            float height = EditorGUI.GetPropertyHeight(GetListProperty(property), label, true);
            return EditorGUI.GetPropertyHeight(GetListProperty(property), label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, GetListProperty(property), label, true);
        }
    }
}