using System;
using UnityEngine;
 
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform renderer;
    [SerializeField] private Transform rayPos;
    [SerializeField] private float yOffset = .1f;
    [SerializeField] private float radius = 5;
    [SerializeField] private LayerMask world;
    public float speed = 1;
    private Vector3 center = Vector3.zero;
    private Vector3 _lastPos;

    private void Start()
    {
        var myTransform = transform;
        renderer = myTransform.GetChild(0);
        rayPos = myTransform.GetChild(1);
    }

    private void LateUpdate()
    {
        _lastPos = transform.position;
    }

    private void Update ()
    {
       MoveTowardsTarget(target);
       
       var ray = new Ray(rayPos.position, -transform.up);
       if (!Physics.Raycast(ray, out var hit, 1, world)) return;
       var newPos = hit.point;
       newPos += transform.up * yOffset;
       renderer.position = newPos;
    }

    private void MoveTowardsTarget(Transform target)
    {
        var direction = target.position - transform.position;
        direction.Normalize();
        if (direction.magnitude < 0.1) return;
        
        var myTransform = transform;
        var pos = myTransform.position; //get position
        pos += direction * (speed * Time.deltaTime); //move forward
        var v = pos - center; //get new position relative to center of sphere
        pos = center + v.normalized * radius; //constrain position to surface of sphere
        
        myTransform.position = pos; //set position
        if (transform.position == _lastPos) return;
        
        var transform1 = transform;
        if (transform1.position - _lastPos != Vector3.zero) transform1.forward = -(transform1.position - _lastPos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayPos.position, -transform.up);
    }
}
