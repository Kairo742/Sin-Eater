public class SimpleTestButton : Interactable
{

    public GameEvent OpenDoorEvent;


    public override void Interact()
    {
        OpenDoorEvent.TriggerEvent();
    }

}
