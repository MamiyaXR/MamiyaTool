using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public interface IUnRegister {
        void UnRegister();
    }
    public interface IUnRegisterList {
        List<IUnRegister> UnregisterList { get; }
    }
    public static class IUnRegisterListExtension {
        public static void AddToUnregisterList(this IUnRegister self, IUnRegisterList unRegisterList) {
            unRegisterList.UnregisterList.Add(self);
        }

        public static void UnRegisterAll(this IUnRegisterList self) {
            foreach(var unRegister in self.UnregisterList) {
                unRegister.UnRegister();
            }

            self.UnregisterList.Clear();
        }
    }
    /// <summary>
    /// �Զ����ע������
    /// </summary>
    public struct CustomUnRegister : IUnRegister {
        /// <summary>
        /// ί�ж���
        /// </summary>
        private Action mOnUnRegister { get; set; }

        /// <summary>
        /// ���ι��캯��
        /// </summary>
        /// <param name="onDispose"></param>
        public CustomUnRegister(Action onUnRegsiter) {
            mOnUnRegister = onUnRegsiter;
        }

        /// <summary>
        /// ��Դ�ͷ�
        /// </summary>
        public void UnRegister() {
            mOnUnRegister.Invoke();
            mOnUnRegister = null;
        }
    }
    public static class UnRegisterExtension {
        public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject) {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if(!trigger) {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            trigger.AddUnRegister(unRegister);

            return unRegister;
        }

        public static IUnRegister UnRegisterWhenGameObjectDestroyed<T>(this IUnRegister self, T component)
            where T : Component {
            return self.UnRegisterWhenGameObjectDestroyed(component.gameObject);
        }
    }
}