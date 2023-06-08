using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MamiyaTool {
    public class FrameEventReceiver : MonoBehaviour {
        [SerializeField] private FrameEventKeyValue events = new FrameEventKeyValue();
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public void OnEvent(string key) {
            UnityEvent evt;
            if(events.TryGetValue(key, out evt) && evt != null) {
                evt.Invoke();
            }
        }
        public void AddEvent(string evtName, UnityEvent evt) {
            if(evtName == null)
                throw new ArgumentNullException("event name");
            if(events.EventNames.Contains(evtName))
                throw new ArgumentException("EventName already used.");
            events.Append(evtName, evt);
        }
        public int AddEmptyEvent(UnityEvent evt) {
            events.Append(null, evt);
            return events.Events.Count - 1;
        }
        public void Remove(string evtName) {
            if(!events.EventNames.Contains(evtName)) {
                throw new ArgumentException("The EventName is not registered with this receiver.");
            }
            events.Remove(evtName);
        }
        public IEnumerable<string> GetRegisteredEvents() {
            return events.EventNames;
        }
        public UnityEvent GetEvent(string evtName) {
            UnityEvent ret;
            if(events.TryGetValue(evtName, out ret))
                return ret;
            return null;
        }
        public int Count() {
            return events.EventNames.Count;
        }
        public void ChangeEventNameAtIndex(int idx, string newKey) {
            if(idx < 0 || idx > events.EventNames.Count - 1)
                throw new IndexOutOfRangeException();
            if(events.EventNames[idx] == newKey)
                return;
            var alreadyUsed = events.EventNames.Contains(newKey);
            if(newKey == null || events.EventNames[idx] == null || !alreadyUsed)
                events.EventNames[idx] = newKey;
            if(newKey != null && alreadyUsed)
                throw new ArgumentException("EventName already used.");
        }
        public void RemoveAtIndex(int idx) {
            if(idx < 0 || idx > events.EventNames.Count - 1)
                throw new IndexOutOfRangeException();
            events.Remove(idx);
        }
        public void ChangeReactionAtIndex(int idx, UnityEvent evt) {
            if(idx < 0 || idx > events.Events.Count - 1)
                throw new IndexOutOfRangeException();
            events.Events[idx] = evt;
        }
        public UnityEvent GetEventAtIndex(int idx) {
            if(idx < 0 || idx > events.Events.Count - 1)
                throw new IndexOutOfRangeException();
            return events.Events[idx];
        }
        public string GetEventNameAtIndex(int idx) {
            if(idx < 0 || idx > events.EventNames.Count - 1)
                throw new IndexOutOfRangeException();
            return events.EventNames[idx];
        }
        /******************************************************************
         *
         *      class define
         *
         ******************************************************************/
        [Serializable]
        private class FrameEventKeyValue {
            [SerializeField] private List<string> eventNames = new List<string>();
            [SerializeField, CustomFrameEventDrawer]
            private List<UnityEvent> events = new List<UnityEvent>();

            public List<string> EventNames => eventNames;
            public List<UnityEvent> Events => events;

            public bool TryGetValue(string key, out UnityEvent value) {
                int index = eventNames.IndexOf(key);
                if(index != -1) {
                    value = events[index];
                    return true;
                }
                value = null;
                return false;
            }
            public void Append(string key, UnityEvent value) {
                eventNames.Add(key);
                events.Add(value);
            }
            public void Remove(int index) {
                if(index != -1) {
                    eventNames.RemoveAt(index);
                    events.RemoveAt(index);
                }
            }
            public void Remove(string key) {
                var idx = eventNames.IndexOf(key);
                if(idx != -1) {
                    eventNames.RemoveAt(idx);
                    events.RemoveAt(idx);
                }
            }
        }
    }
}