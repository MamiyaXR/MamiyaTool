using UnityEngine;
using UnityEditor;

namespace MamiyaTool {
    [CustomEditor(typeof(FrameAnimationAsset))]
    public class FrameAnimationAssetInspector : Editor {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
            FrameAnimationAsset _target = (FrameAnimationAsset)target;
            if(_target != null && _target.Tracks != null) {
                foreach(var track in _target.Tracks) {
                    if(!(track is FrameSpriteTrack))
                        continue;
                    var _track = (FrameSpriteTrack)track;
                    if(_track.Datas == null || _track.Datas.Count <= 0)
                        continue;
                    foreach(var data in _track.Datas) {
                        if(data.Frame != null) {
                            //Texture2D tex = new Texture2D((int)data.Frame.textureRect.width, (int)data.Frame.textureRect.height);
                            //var pixels = data.Frame.texture.GetPixels(
                            //                                    (int)data.Frame.textureRect.x,
                            //                                    (int)data.Frame.textureRect.y,
                            //                                    (int)data.Frame.textureRect.width,
                            //                                    (int)data.Frame.textureRect.height);
                            //tex.SetPixels(pixels);
                            //tex.Apply();
                            //return tex;
                            return data.Frame.texture;
                        }
                    }
                }
            }
            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }
    }
}