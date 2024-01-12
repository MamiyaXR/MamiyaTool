using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MamiyaTool {
    public static class FrameAnimationAssetCreator {
        /******************************************************************
         *
         *      method
         *
         ******************************************************************/
        [MenuItem(CM, false, -1000)]
        private static void Create() {
            string path = GetPath();
            path = AssetDatabase.GenerateUniqueAssetPath(path + '/' + ASSET_NAME);
            var asset = ScriptableObject.CreateInstance<FrameAnimationAsset>();
            try {
                SetAsset(asset, GetSprites());
            } finally {
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        private static string GetPath() {
            var obj = Selection.activeObject;
            string path = AssetDatabase.GetAssetPath(obj);
            if(obj.GetType() == typeof(DefaultAsset))
                return path;
            int pathPos = path.LastIndexOf('/');
            return path.Substring(0, pathPos);
        }
        private static Sprite[] GetSprites() {
            var objs = Selection.objects;
            if(objs == null)
                return null;
            List<Sprite> col = new List<Sprite>();
            foreach(var obj in objs) {
                if(obj.GetType() == typeof(Sprite))
                    col.Add(obj as Sprite);
            }
            return col.Count > 0 ? col.ToArray() : null;
        }
        private static void SetAsset(FrameAnimationAsset asset, Sprite[] sprites) {
            if(asset == null)
                return;
            if(sprites == null)
                return;
            var track = new FrameSpriteTrack();
            ReflectionUtility.SetField(FIELD_FRAMES, track, new List<FrameSpriteData>());
            for(int i = 0; i < sprites.Length; ++i) {
                var data = new FrameSpriteData();
                ReflectionUtility.SetField(FIELD_FRAME_INDEX, data, i);
                ReflectionUtility.SetField(FIELD_FRAME, data, sprites[i]);
                track.Datas.Add(data);
            }
            ReflectionUtility.SetField(FIELD_TRACKS, asset, new List<FrameTrackBase>());
            asset.Tracks.Add(track);
        }
        /******************************************************************
         *
         *      const define
         *
         ******************************************************************/
        private const string CM = "Assets/Create/Frame Animation Asset";
        private const string ASSET_NAME = "FrameAnimation.asset";

        private const string FIELD_FRAMES = "frames";
        private const string FIELD_FRAME_INDEX = "frameIndex";
        private const string FIELD_FRAME = "frame";
        private const string FIELD_TRACKS = "tracks";
    }
}