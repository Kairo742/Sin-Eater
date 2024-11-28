using System.Collections.Generic;
using UnityEngine;

public class SimpleUnwantedButton : Interactable
{
    public GameEvent OpenDoorEvent;

    public List<Material> Materials;

    private bool toogle;


    private MeshRenderer MeshRenderer;
    private void Awake()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
    }


    public override void Interact()
    {
        toogle = !toogle;
        if (toogle == false) OpenDoorEvent.ClearTriggered();
        MeshRenderer.material = toogle ? Materials[1] : Materials[0];
        OpenDoorEvent.TriggerEvent(!toogle);
    }

}
