using UnityEngine;

public interface IGameEventListener
{
    public void OnEventTriggered();
    public void OnEventTriggered(GameObject go);

}