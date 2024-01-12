namespace MamiyaTool {
    public interface IFrameTrack<T> : IGetDatasable<T> where T : FrameDataBase {
        bool Enable { get; }
        string ComponentPath { get; }
    }
}