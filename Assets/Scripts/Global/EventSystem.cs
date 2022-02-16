using System;
using System.Collections.Generic;

public class EventSystem : MonoSingleton<EventSystem>
{
    private readonly Dictionary<SystemEventType, List<Action<Message>>> listeners =
            new Dictionary<SystemEventType, List<Action<Message>>>(50);

    private bool isInit;

    private void Initialize()
    {
        if (!isInit)
        {
            isInit = true;
        }
    }

    public void Subscribe(SystemEventType type, Action<Message> handler)
    {
        Initialize();

        if (listeners.ContainsKey(type))
        {
            if (listeners[type] == null)
            {
                listeners[type] = new List<Action<Message>>(20);
            }
        }
        else
        {
            listeners.Add(type, new List<Action<Message>>(20));
        }
        listeners[type].Add(handler);
    }

    public void Unsubscribe(SystemEventType type, Action<Message> handler)
    {
        Initialize();
        if (listeners.ContainsKey(type))
        {
            if (listeners[type] != null)
            {
                listeners[type].Remove(handler);
            }
        }
    }

    /// <summary>
    /// Send a message to all subscribers listening to this certain message type
    /// </summary>
    /// <param name="message"></param>
    public void SendMessage(Message message)
    {
        Initialize();
        DispatchMessage(message);
    }

    /// <summary>
    /// Dispatch listener event 
    /// </summary>
    /// <param name="message"></param>
    private void DispatchMessage(Message message)
    {
        Initialize();

        if (listeners.TryGetValue(message.MessageType, out var listHandlers))
        {
            foreach (var listenItem in listHandlers)
            {
                listenItem?.Invoke(message);
            }
        }
    }


    /// <summary>
    /// Push a message directly into processing. Note that this call is synchronous and is not in framerate control
    /// </summary>
    /// <param name="message"></param>
    public void Announce(Message message)
    {
        DispatchMessage(message);
    }
}
