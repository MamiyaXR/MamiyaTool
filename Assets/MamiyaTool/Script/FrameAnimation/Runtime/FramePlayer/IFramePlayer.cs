using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFramePlayer {
    bool Enable { get; }
    int BeginFrame { get; }
    int EndFrame { get; }
    void SetFrame(int frameCount);
    void Reset();
}
