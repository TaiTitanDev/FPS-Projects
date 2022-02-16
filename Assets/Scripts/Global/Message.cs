using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    private readonly SystemEventType messageType;

    public object Data { get; private set; }

    public SystemEventType MessageType => messageType;

    public Message() { }

    public Message(SystemEventType type)
    {
        messageType = type;
    }

    public void UpdateNewData(object data)
    {
        this.Data = data;
    }

    public Message(SystemEventType type, object data)
    {
        messageType = type;
        this.Data = data;
    }
}
