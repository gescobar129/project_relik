using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventSystem
{
    private static GlobalEventSystem eventSystem;

    private GlobalEventSystem()
    { }

    public static GlobalEventSystem GetInstance()
    {
        if (eventSystem == null)
        {
            eventSystem = new GlobalEventSystem();
        }

        return eventSystem;
    }

    public void AddListener<EventArgT>(GlobalEvent<EventArgT> globalEvent, EventHandler<EventArgT> handler)
    {
        globalEvent.AddListener(handler);
    }

    public void RemoveListener<EventArgT>(GlobalEvent<EventArgT> globalEvent, EventHandler<EventArgT> handler)
    {
        globalEvent.RemoveListener(handler);
    }

    public void TriggerEvent<EventArgT>(GlobalEvent<EventArgT> globalEvent, EventArgT args)
    {
        globalEvent.Trigger(args);
    }
}

public class GlobalEvent<EventArgT>
{
    private static GlobalEvent<EventArgT> globalEvent;

    public static event EventHandler<EventArgT> Event;

    protected GlobalEvent()
    { }

    public static GlobalEvent<EventArgT> GetInstance()
    {

        if (globalEvent == null)
        {
            globalEvent = new GlobalEvent<EventArgT>();
        }

        return globalEvent;
    }

    public void AddListener(EventHandler<EventArgT> handler)
    {
        Event += handler;
    }

    public void RemoveListener(EventHandler<EventArgT> handler)
    {
        Event -= handler;
    }

    public void Trigger(EventArgT args)
    {
        if (Event != null)
        {
            Event.Invoke(this, args);
        }
    }
}

public class ObjectKilledGlobalEvent : GlobalEvent<ObjectKilledGlobalEvent.EventArgs>
{

    private ObjectKilledGlobalEvent()
    { }

    public class EventArgs
    {
        public GameObject killed;
        public GameObject killer;
    }

}

public class ItemCollectedGlobalEvent : GlobalEvent<ItemCollectedGlobalEvent.EventArgs>
{
    private ItemCollectedGlobalEvent()
    { }

    public class EventArgs
    {
        public GameObject collector;
        public GameObject collected;
    }
}