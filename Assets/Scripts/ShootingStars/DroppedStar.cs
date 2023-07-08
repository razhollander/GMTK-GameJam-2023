using UnityEngine;

public class DroppedStar : MonoBehaviour
{
    [SerializeField] private float timeToSelfDestruct = 5;
    private void Start()
    {
        Destroy(gameObject, timeToSelfDestruct);
    }
    
    private void OnMouseUpAsButton()
    {
        Debug.Log("Clicked");
    }
    
}
