using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MFramework {
    [CustomPropertyDrawer(typeof(CompSelectorAttribute))]
    public class CompSelectorDrawer : PropertyDrawer {
        private Component[] GetComponents(GameObject go) {
            var comps = go.GetComponents<Component>();
            var types = (attribute as CompSelectorAttribute).types;
            if(types != null) {
                var list = new List<Component>();
                foreach(var comp in comps) {
                    var compType = comp.GetType();
                    if(types.Contains(compType) || Array.Exists(types, (x) => x.IsAssignableFrom(compType))) {
                        list.Add(comp);
                    }
                }
                return list.ToArray();
            }
            return comps;
        }

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
            if(property.objectReferenceValue) {
                pos = new Rect(pos.x, pos.y, pos.width - 100, pos.height);
                EditorGUI.ObjectField(pos, property, label);

                var comp = property.objectReferenceValue as Component;
                if(comp) {
                    pos = new Rect(pos.xMax, pos.y, 100, pos.height);
                    var comps = GetComponents(comp.gameObject);
                    if(comps.Length > 0) {
                        var index = Array.IndexOf(comps, comp);
                        index = EditorGUI.Popup(pos, index, comps.Select(x => x.GetType().Name).ToArray());
                        if(index >= 0) {
                            property.objectReferenceValue = comps[index];
                        } else {
                            property.objectReferenceValue = comps[0];
                        }
                    }
                } else {
                    var go = property.objectReferenceValue as GameObject;
                    if(go) {
                        pos = new Rect(pos.xMax, pos.y, 100, pos.height);
                        var comps = GetComponents(go);
                        if(comps.Length > 0) {
                            var index = EditorGUI.Popup(pos, -1, comps.Select(x => x.GetType().Name).ToArray());
                            if(index >= 0) {
                                property.objectReferenceValue = comps[index];
                            } else {
                                property.objectReferenceValue = comps[0];
                            }
                        }
                    }
                }
            } else {
                EditorGUI.ObjectField(pos, property, label);

                var comp = property.objectReferenceValue as Component;
                if(comp) {
                    comp = TagHelper.GetCompByTag(comp);
                    property.objectReferenceValue = comp;
                }
            }
        }
    }
}