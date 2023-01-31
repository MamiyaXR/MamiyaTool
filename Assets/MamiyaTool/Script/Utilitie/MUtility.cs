using System;
using System.Threading;
using System.Collections;
using UnityEngine;

namespace MamiyaTool
{
    public static class MUtility
    {
        public static bool IsInScreen(Vector3 position)
        {
            Vector3 vPos = Camera.main.WorldToViewportPoint(position);
            if(vPos.x > 0 && vPos.x < 1 && vPos.y > 0 && vPos.y < 1)
                return true;
            else
                return false;
        }
        public static bool IsInScreenIgnoreY(Vector3 position)
        {
            Vector3 vPos = Camera.main.WorldToViewportPoint(position);
            if(vPos.x > 0 && vPos.x < 1)
                return true;
            else
                return false;
        }
        public static bool IsInScreenIgnoreForward(Vector3 position)
        {
            Vector3 vPos = Camera.main.WorldToViewportPoint(position);
            if(vPos.x > 0)
                return true;
            else
                return false;
        }
        public static bool IsInScreenWithBound(Vector3 position, float bound)
        {
            if(!IsInScreen(position + new Vector3(bound, bound, 0)))
                return false;
            if(!IsInScreen(position + new Vector3(bound, -bound, 0)))
                return false;
            if(!IsInScreen(position + new Vector3(-bound, bound, 0)))
                return false;
            if(!IsInScreen(position + new Vector3(-bound, -bound, 0)))
                return false;
            return true;
        }
        public static IEnumerator TimeCount(float time, Action callback)
        {
            float timer = 0f;
            while(timer < time) {
                yield return null;
                timer += Time.deltaTime;
            }
            callback?.Invoke();
        }
        public static Coroutine MStopCoroutine(this MonoBehaviour owner, Coroutine ptr)
        {
            if(ptr != null) {
                owner.StopCoroutine(ptr);
                ptr = null;
            }
            return ptr;
        }
        public static Transform FindChildByName(this Transform parent, string name)
        {
            Transform result = null;
            for(int i = 0; i < parent.childCount; i++) {
                result = parent.GetChild(i);
                if(result.name == name)
                    break;
                result = result.FindChildByName(name);
                if(result != null)
                    break;
                result = null;
            }
            return result;
        }
        public static T GetComponentInChildren<T>(this GameObject parentObj, string name) where T : Component
        {
            Transform child = parentObj.transform.FindChildByName(name);
            if(child == null)
                return null;
            return child.GetComponent<T>();
        }
        public static void ClearChildren(this Transform transform)
        {
            for(int i = 0; i < transform.childCount; i++) {
                UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
            }
        }
        public static CancellationTokenSource MCancel(this CancellationTokenSource token, bool newToken = true)
        {
            if(token != null) {
                token.Cancel();
                token.Dispose();
            }
            if(newToken)
                return new CancellationTokenSource();
            return null;
        }
    }
}