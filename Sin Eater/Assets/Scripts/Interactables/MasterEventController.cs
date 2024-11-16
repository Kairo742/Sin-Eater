using System.Collections.Generic;
using UnityEngine;

public class MasterEventController : MonoBehaviour, IGameEventListener
{
    public GameEvent combinedEvents;

    public List<GameEvent> WaitedGameEvents;
    public List<GameEvent> UnWantedGameEvents;

    private void OnEnable()
    {
        foreach (var gameEvent in WaitedGameEvents)
        {
            gameEvent.AddListener(this);
        }
        foreach (var gameEvent in UnWantedGameEvents)
        {
            gameEvent.AddListener(this);
        }
    }

    private void OnDisable()
    {
        foreach (var gameEvent in WaitedGameEvents)
        {
            gameEvent.RemoveListener(this);
        }
        foreach (var gameEvent in UnWantedGameEvents)
        {
            gameEvent.RemoveListener(this);
        }
    }
    public void GatherOtherEvents()
    {
        bool allTriggered = true;
        foreach (var gameEvent in WaitedGameEvents)
        {
            allTriggered &= gameEvent.IsTriggeredBefore();
        }
        foreach (var gameEvent in UnWantedGameEvents)
        {
            allTriggered &= !gameEvent.IsTriggeredBefore();
        }

        if (allTriggered) combinedEvents.TriggerEvent();
    }

    public void OnEventTriggered()
    {
        GatherOtherEvents();
    }

    public void OnEventTriggered(GameObject go)
    {
        throw new System.NotImplementedException();
    }
}
