using System;
using UnityEngine;
using UnityEditor;

namespace MamiyaTool {
    [Serializable]
    internal class GameObjectSelectionItem : FrameAnimWinSelectionItem {
        public static GameObjectSelectionItem Create(GameObject gameObject) {
            var selectionItem = new GameObjectSelectionItem();

            

            return selectionItem;
        }
    }
}