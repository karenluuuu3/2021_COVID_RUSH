using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStore : MonoBehaviour
{
    public static EventStore instance = null;

    private Dictionary<string, List<LisenterInfo>> EventDictionary = new Dictionary<string, List<LisenterInfo>>();

    struct LisenterInfo
    {
        public Component owner;
        public System.Action<Component, object> action;
    }

    public EventStore Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }

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
            
            EventDictionary[eventName][registeredID] = lisenter;
        }

        EventDictionary.Add(eventName, new List<LisenterInfo>{ lisenter });
    }

    public void RemoveEvent(Component owner, string eventName = "")
    {
        if (EventDictionary.ContainsKey(eventName))
        {
            EventDictionary.Remove(eventName);
            return;
        }

        Debug.LogError("Event '" + eventName + "' doesn't exist.");
    }

    public void Notify(string eventName, Component sender, object param = null)
    {
        if (EventDictionary.ContainsKey(eventName))
        {
            foreach (var listener in EventDictionary[eventName])
            {
                listener.action(sender, param);
            }
        }
    }

    private void RemoveAllLisenter(Component owner)
    {
        foreach (string eventName in EventDictionary.Keys)
        {
            RemoveListener(eventName, owner);
        }
    }

    private void RemoveListener(string eventName, Component owner)
    {
        EventDictionary[eventName].RemoveAll(c => owner == c.owner);
    }
}
