using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    // 提供一個 dictionary 讓事件觸發者 & 接收者可以 listen & notify
    public class EventStore
    {
        public static EventStore instance = new EventStore();

        private Dictionary<string, List<LisenterInfo>> EventDictionary = new Dictionary<string, List<LisenterInfo>>();

        struct LisenterInfo
        {
            public Component owner;
            public System.Action<Component, object> action;
        }

        // 讓 class 唯一
        public EventStore Instance
        {
            get { return instance; }
        }

        // 註冊事件: 綁定事件(eventName)的 listener(owner)以及觸發時對應的 action(func)
        public void Register(string eventName, Component owner, System.Action<Component, object> func)
        {
            var lisenter = new LisenterInfo
            {
                owner = owner,
                action = func
            };

            if (EventDictionary.ContainsKey(eventName))
            {
                int registeredID = EventDictionary[eventName].FindIndex(c => c.owner == owner);
                if (registeredID == -1)
                {
                    EventDictionary[eventName].Add(lisenter);
                    return;
                }
                Debug.Log("bind " + eventName + " ok");
                EventDictionary[eventName][registeredID] = lisenter;
                return;
            }
            Debug.Log("bind " + eventName + " ok too");
            EventDictionary.Add(eventName, new List<LisenterInfo> { lisenter });
        }

        // 移除事件: 從 dictionary 移除某事件(eventName)
        public void RemoveEvent(string eventName)
        {
            if (EventDictionary.ContainsKey(eventName))
            {
                EventDictionary.Remove(eventName);
                return;
            }

            Debug.LogError("Event '" + eventName + "' doesn't exist.");
        }

        // 觸發事件: 有綁定事件(eventName)的 listener(listener)會在此時收到觸發者(sender)的更新資訊(param)
        public void Notify(string eventName, Component sender, object param = null)
        {
            if (!EventDictionary.ContainsKey(eventName))
            {
                Debug.LogError("Event '" + eventName + "' doesn't exist.");
                return;
            }
            foreach (var listener in EventDictionary[eventName])
            {
                listener.action(sender, param);
            }
        }

        // 移除接收者: 移除存在於 dictionary 的某 listener(owner)
        private void RemoveLisenterFromAllEvent(Component owner)
        {
            foreach (string eventName in EventDictionary.Keys)
            {
                RemoveListenerFromEvent(eventName, owner);
            }
        }

        // 移除接收者: 移除有綁定某 event(eventName) 的某 listener(owner)
        private void RemoveListenerFromEvent(string eventName, Component owner)
        {
            EventDictionary[eventName].RemoveAll(c => owner == c.owner);
        }
    }
}
