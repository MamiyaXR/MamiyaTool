using UnityEngine;

namespace MamiyaTool {
    internal struct AnimationKeyTime{
        private float m_FrameRate;
        private int m_Frame;
        private float m_Time;

        public float frameRate => m_FrameRate;
        public int frame => m_Frame;
        public float time => m_Time;

        public float frameFloor => (frame - 0.5f) / frameRate;
        public float frameCeiling => (frame + 0.5f) / frameRate;
        public float timeRound => frame / frameRate;

        public static AnimationKeyTime Time(float time, float frameRate) {
            AnimationKeyTime key = new AnimationKeyTime();
            key.m_Time = Mathf.Max(time, 0f);
            key.m_FrameRate = frameRate;
            key.m_Frame = Mathf.RoundToInt(key.m_Time * frameRate);
            return key;
        }
        public static AnimationKeyTime Frame(int frame, float frameRate) {
            AnimationKeyTime key = new AnimationKeyTime();
            key.m_Frame = (frame < 0) ? 0 : frame;
            key.m_Time = key.m_Frame / frameRate;
            key.m_FrameRate = frameRate;
            return key;
        }
        public bool ContainsTime(float time) {
            return time >= frameFloor && time < frameCeiling;
        }
        public bool Equals(AnimationKeyTime key) {
            return m_Frame == key.m_Frame &&
                m_FrameRate == key.m_FrameRate &&
                Mathf.Approximately(m_Time, key.m_Time);
        }
    }
}