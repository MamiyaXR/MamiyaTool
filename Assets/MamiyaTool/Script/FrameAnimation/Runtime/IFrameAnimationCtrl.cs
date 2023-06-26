namespace MamiyaTool {
    public interface IFrameAnimationCtrl {
        void Play();
        void Stop();
        bool GetBool(string param);
        float GetFloat(string param);
        int GetInt(string param);
        void SetBool(string param, bool value);
        void SetFloat(string param, float value);
        void SetInt(string param, int value);
        void SetTrigger(string param);
    }
}