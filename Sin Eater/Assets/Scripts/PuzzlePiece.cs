using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{



    [SerializeField] private LayerMask _artifactLayers;






    private void OnCollisionEnter(Collision col)
    {
        if (Helpers.IsInLayerMask(_artifactLayers, col.gameObject.layer))
        {
            Destroy(col.gameObject);
            PieceCompleted();
        }
    }





    private void PieceCompleted()
    {

        PuzzleManager.Instance.UpdateList();
        Destroy(gameObject);
    }
}
