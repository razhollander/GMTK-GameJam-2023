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
        Destroy(gameObject);
        
        ElementsManager.Instance.SetElement(GetRandomElement());
    }

    private ElementEffect GetRandomElement()
    {
        return Random.Range(0, 2) >0 ? ElementEffect.Meteor : ElementEffect.Tornado;
    }
    
}
