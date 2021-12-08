using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    // ���Ѥ@�� dictionary ���ƥ�Ĳ�o�� & �����̥i�H listen & notify
    public class EventStore
    {
        public static EventStore instance = new EventStore();

        private Dictionary<string, List<LisenterInfo>> EventDictionary = new Dictionary<string, List<LisenterInfo>>();

        struct LisenterInfo
        {
            public Component owner;
            public System.Action<Component, object> action;
        }

        // �� class �ߤ@
        public EventStore Instance
        {
            get { return instance; }
        }

        // ���U�ƥ�: �j�w�ƥ�(eventName)�� listener(owner)�H��Ĳ�o�ɹ����� action(func)
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

        // �����ƥ�: �q dictionary �����Y�ƥ�(eventName)
        public void RemoveEvent(string eventName)
        {
            if (EventDictionary.ContainsKey(eventName))
            {
                EventDictionary.Remove(eventName);
                return;
            }

            Debug.LogError("Event '" + eventName + "' doesn't exist.");
        }

        // Ĳ�o�ƥ�: ���j�w�ƥ�(eventName)�� listener(listener)�|�b���ɦ���Ĳ�o��(sender)����s��T(param)
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

        // ����������: �����s�b�� dictionary ���Y listener(owner)
        private void RemoveLisenterFromAllEvent(Component owner)
        {
            foreach (string eventName in EventDictionary.Keys)
            {
                RemoveListenerFromEvent(eventName, owner);
            }
        }

        // ����������: �������j�w�Y event(eventName) ���Y listener(owner)
        private void RemoveListenerFromEvent(string eventName, Component owner)
        {
            EventDictionary[eventName].RemoveAll(c => owner == c.owner);
        }
    }
}
