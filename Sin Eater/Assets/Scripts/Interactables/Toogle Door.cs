using UnityEngine;

public class ToogleDoor : Interactable
{
    public override void Interact()
    {
        //I normally use https://dotween.demigiant.com did not make any animation for that reason

        transform.rotation *= Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 90, 0), 90);

    }
}
