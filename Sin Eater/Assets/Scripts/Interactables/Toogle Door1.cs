using UnityEngine;

public class ToogleDoorN : Interactable
{
    public override void Interact()
    {
        //I normally use https://dotween.demigiant.com did not make any animation for that reason

        transform.rotation *= Quaternion.Euler(0, 90, 0);

    }
}
