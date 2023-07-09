using Audio;
using UnityEngine;
using DG.Tweening;
public class DroppedStar : MonoBehaviour
{
    [SerializeField] private float timeToSelfDestruct = 5;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _arrowY;
    private void Start()
    {
        if (_arrow != null)
        {
            transform.LookAt(Vector3.zero);
            _arrow.transform.DOLocalMoveZ(_arrow.transform.localPosition.z + _arrowY, timeToSelfDestruct * 0.2f).SetLoops(-1, LoopType.Yoyo);
        }

        Destroy(gameObject, timeToSelfDestruct);
    }
    
    private void OnMouseUpAsButton()
    {
        if (GameButtons.Instance != null && GameButtons.Instance.IsPaused)
        {
            return;
        }
        
        Destroy(gameObject);
        AudioManager.Instance.Play(AudioManager.SoundsType.StartCollect);
        ElementsManager.Instance.SetElement(GetRandomElement());
    }

    private ElementEffect GetRandomElement()
    {
        return Random.Range(0, 2) >0 ? ElementEffect.Meteor : ElementEffect.Tornado;
    }
    
}
