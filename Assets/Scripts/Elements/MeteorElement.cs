using Planet;
using UnityEngine;

public class MeteorElement : MonoBehaviour
{
    [SerializeField] private Land _targetLand;
    private bool _hit;
    private const float Epsilon = 0.1f;
    [SerializeField] private float speed = .5f;
    [SerializeField] private GameObject explosion;
    private void Update()
    {
        if(!_targetLand) return;
        if ((_targetLand.Position - transform.position).magnitude < Epsilon)
        {
            if (_hit) return;
            _hit = true;
            _targetLand.DestroyBuilding();
            Instantiate(explosion, transform.position, Quaternion.identity);
            ShakeCamera.Instance.Shake();
            Destroy(gameObject);
            return;
        };
        transform.Translate(Time.deltaTime * speed * (_targetLand.Position - transform.position).normalized);
    }
    
    public void SetLand(Land land)
    {
        _targetLand = land;
    }
}
