using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameEventPlayer : FramePlayerBase<FrameEventReceiver, FrameEventData> {
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        public override void SetFrame(int frameCount) {
            curFrame = frameCount;
            foreach(var data in datas) {
                if(data.FrameIndex == curFrame) {
                    if(ObjUsable)
                        Invoke(data);
                    return;
                }
            }
        }
        protected override void Invoke(FrameEventData data) {
            m_Object.OnEvent(data.EventKey);
        }
    }
}