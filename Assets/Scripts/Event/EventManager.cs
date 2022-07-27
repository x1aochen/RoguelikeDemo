using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventName
{
    Transition,
    BeDamage

}


public class EventManager
{
    private static EventManager m_instance = null;
    public static EventManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new EventManager();
            }
            return m_instance;
        }
    }
    private EventManager() { }

    public Dictionary<EventName, Action<EventArgs>> events = new Dictionary<EventName, Action<EventArgs>>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public void AddListener(EventName eventName,Action<EventArgs> func)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] += func;
        }
        else
        {
            events.Add(eventName, func);
        }
    }

    /// <summary>
    /// 调用事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="args"></param>
    public void Call(EventName eventName,EventArgs args = null)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName](args);
        }
        else
        {
            Debug.LogError($"{eventName} 事件不存在");
        }
    }
}
