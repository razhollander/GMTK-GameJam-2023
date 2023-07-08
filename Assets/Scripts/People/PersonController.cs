using Planet;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Land target;
    [SerializeField] private Transform renderer;
    [SerializeField] private Transform rayPos;
    [SerializeField] private float yOffset = .1f;
    [SerializeField] private float radius = 5;
    [SerializeField] private float speed = 1;
    [SerializeField] private LayerMask world;
    [SerializeField] private MeshRenderer humanMeshRenderer;
    private Vector3 center = Vector3.zero;
    private Vector3 _lastPos;

    private void Start()
    {
        var myTransform = transform;
        renderer = myTransform.GetChild(0);
        rayPos = myTransform.GetChild(1);
        SetTarget(target.Neighbors[Random.Range(0, target.Neighbors.Count)]);
    }

    private void LateUpdate()
    {
        _lastPos = transform.position;
    }

    private void Update ()
    {
       if (target) MoveTowardsTarget(target);
       
       var ray = new Ray(rayPos.position, -transform.up);
       if (!Physics.Raycast(ray, out var hit, 1, world)) return;
       var newPos = hit.point;
       newPos += transform.up * yOffset;
       renderer.position = newPos;
    }

    private void MoveTowardsTarget(Land target)
    {
        var direction = target.Position - transform.position;
        direction.Normalize();
        if (direction.magnitude < 0.1) return;
        
        var myTransform = transform;
        var pos = myTransform.position; //get position
        pos += direction * (speed * Time.deltaTime); //move forward
        var v = pos - center; //get new position relative to center of sphere
        pos = center + v.normalized * radius; //constrain position to surface of sphere
        myTransform.position = pos; //set position

        if (transform.position == _lastPos) return;

        if (myTransform.position - _lastPos != Vector3.zero)
        {
            myTransform.rotation = Quaternion.FromToRotation(transform.up, myTransform.position) * transform.rotation;
        }

    }

    public void FindNextTarget()
    {
        SetTarget(target.Neighbors[Random.Range(0, target.Neighbors.Count)]);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayPos.position, -transform.up);
    }

    #region getters&setters

        public void SetSpeed(float _speed) {speed = _speed;}
        
        public float GetSpeed() {return speed;}
        
        public void SetTarget(Land _target) {target = _target;}

        public void SetLook(Material material)
        {
            humanMeshRenderer.material = material;
        }

    #endregion
}
