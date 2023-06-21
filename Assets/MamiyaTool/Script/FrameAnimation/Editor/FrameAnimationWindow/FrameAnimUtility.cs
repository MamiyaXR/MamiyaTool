using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEditor;

namespace MamiyaTool {
    internal static class FrameAnimUtility {
        private static List<TrackTimeData> temp = new List<TrackTimeData>();
        /*****************************************************************
         * 
         *      static method
         * 
         *****************************************************************/
        public static float GetStartTime(FrameAnimationAsset asset) {
            if(asset == null)
                return -1;

            foreach(var track in asset.Tracks) {
                TrackTimeData tempData = new TrackTimeData();
                Type type = track.GetType();
                PropertyInfo dataProperty = type.GetProperty("Datas");
                MethodInfo dataGet = dataProperty.GetGetMethod();
                IEnumerable<FrameDataBase> datas = dataGet.Invoke(track, null) as IEnumerable<FrameDataBase>;
                tempData.beginTime = datas.Count() > 0 ? datas.Select(i => i.FrameIndex).Min() : 0;
                tempData.endTime = datas.Count() > 0 ? datas.Select(i => i.FrameIndex).Max() : 0;
                temp.Add(tempData);
            }

            float result = temp.Select(i => i.beginTime).Min() * 1f / asset.SampleRate;
            temp.Clear();
            return result;
        }
        public static float GetStopTime(FrameAnimationAsset asset) {
            if(asset == null)
                return -1;

            foreach(var track in asset.Tracks) {
                TrackTimeData tempData = new TrackTimeData();
                Type type = track.GetType();
                PropertyInfo dataProperty = type.GetProperty("Datas");
                MethodInfo dataGet = dataProperty.GetGetMethod();
                IEnumerable<FrameDataBase> datas = dataGet.Invoke(track, null) as IEnumerable<FrameDataBase>;
                tempData.beginTime = datas.Count() > 0 ? datas.Select(i => i.FrameIndex).Min() : 0;
                tempData.endTime = datas.Count() > 0 ? datas.Select(i => i.FrameIndex).Max() : 0;
                temp.Add(tempData);
            }

            float result = (temp.Select(i => i.endTime).Max() + 1) * 1f / asset.SampleRate;
            temp.Clear();
            return result;
        }
        public static void SaveSampleRate(FrameAnimationAsset asset, int sampleRate) {
            if(asset == null)
                return;

            SerializedObject obj = new SerializedObject(asset);
            SerializedProperty property = obj.FindProperty(PROPERTY_SAMPLE_RATE_NAME);
            property.intValue = sampleRate;
            obj.ApplyModifiedProperties();
        }
        /*****************************************************************
         * 
         *      string define
         * 
         *****************************************************************/
        private const string PROPERTY_SAMPLE_RATE_NAME = "sampleRate";
        /*****************************************************************
         * 
         *      struct define
         * 
         *****************************************************************/
        internal struct TrackTimeData {
            internal int beginTime;
            internal int endTime;
        }
    }
}