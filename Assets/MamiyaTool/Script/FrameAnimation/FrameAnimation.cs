using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [CreateAssetMenu(fileName = "Frame Animation")]
    public class FrameAnimation : ScriptableObject {
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        [SerializeField] private int frame = 1;
        [SerializeField] private int offset = 0;
        [SerializeField] private bool loop = false;
        [SerializeField] private bool playOnAwake = true;

        public List<Sprite> Sprites => sprites;
        public int Frame => frame;
        public int Offset {
            get {
                return Mathf.Clamp(offset, 0, sprites.Count - 1);
            }
        }
        public bool Loop => loop;
        public bool PlayOnAwake => playOnAwake;
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public Sprite GetSprite(int index) {
            if(sprites == null || sprites.Count <= 0)
                return null;
            return sprites[Mathf.Clamp(index, 0, sprites.Count - 1)];
        }
    }
}