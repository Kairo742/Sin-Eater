using System.Collections.Generic;
using UnityEngine;

public class MasterEventController : MonoBehaviour
{
    public GameEvent combinedEvents;

    public List<GameEvent> WaitedGameEvents;


    public GameEventListener gameEventListener;

    private void Awake()
    {
        //gameEventListener = GetComponent<GameEventListener>();
    }

    private void Start()
    {
        foreach (var gameEvent in WaitedGameEvents)
        {
            gameEvent.AddListener(gameEventListener);
        }
    }
    public void GatherOtherEvents()
    {
        bool allTriggered = true;
        foreach (var gameEvent in WaitedGameEvents)
        {
            allTriggered &= gameEvent.IsTriggeredBefore();
        }

        if (allTriggered) combinedEvents.TriggerEvent();
    }
}
