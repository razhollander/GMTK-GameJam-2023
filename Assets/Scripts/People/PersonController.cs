using System;
using Cysharp.Threading.Tasks;
using Planet;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private const float Epsilon = 0.1f;

    [SerializeField] private Land target;
    [SerializeField] private Transform renderer;
    [SerializeField] private Transform rayPos;
    [SerializeField] private float yOffset = .1f;
    [SerializeField] private float radius = 5;
    [SerializeField] private float speed = 1;
    [SerializeField] private int probability = 3;
    [SerializeField] private LayerMask world;
    [SerializeField] private MeshRenderer humanMeshRenderer;
    private Vector3 wayUp;
    private Vector3 center = Vector3.zero;
    private Vector3 _lastPos;
    private bool _arrived;
    
    private void Start()
    {
        var myTransform = transform;
        renderer = myTransform.GetChild(0);
        rayPos = myTransform.GetChild(1);
        var neighbor = target.Neighbors[Random.Range(0, target.Neighbors.Count)];
        if (neighbor.Vertex == 5)
        {
            neighbor = target.Neighbors[Random.Range(0, target.Neighbors.Count)];
        }
        SetTarget(neighbor);
    }

    private void LateUpdate()
    {
        _lastPos = transform.position;
    }

    private void Update ()
    {
        var ray = new Ray(rayPos.position, -transform.up);
       Physics.Raycast(ray, out var hit, 1, world);
       wayUp = hit.normal;
       var newPos = hit.point;
       newPos += transform.up * yOffset;
       renderer.position = newPos;
       if (target) MoveTowardsTarget(target);

    }

    private void MoveTowardsTarget(Land target)
    {
        var direction = target.Position - transform.position;
        var cross =  Vector3.Cross(direction, wayUp);
        cross.Normalize();
        var newDirection = direction + cross;
        direction.Normalize();
        if (newDirection.magnitude < 0.1) return;
        
        var myTransform = transform;
        var pos = myTransform.position; //get position
        pos += newDirection * (speed * Time.deltaTime); //move forward
        var v = pos - center; //get new position relative to center of sphere
        pos = center + v.normalized * radius; //constrain position to surface of sphere
        myTransform.position = pos; //set position

        if ((transform.position - target.Position).magnitude < Epsilon && !_arrived)
        {
            _arrived = true;
            DecideNextMove();
        }

        if (myTransform.position - _lastPos != Vector3.zero)
        {
            myTransform.rotation = Quaternion.FromToRotation(transform.up, myTransform.position) * transform.rotation;
        }

    }

    private void DecideNextMove()
    {
        switch (target.BuildingType)
        {
            case BuildingType.Forest:
                target.HitForest();
                WaitAndContinue(false, 2);
                break;
            
            case BuildingType.None:
                
                var rnd = Random.Range(0, probability);
                if (rnd == 0)
                {
                    target.BuildBuilding(1);
                }
                else
                {
                    FindNextTarget();
                }
                break;
            
            case BuildingType.Building:
                var rnd2 = Random.Range(0, probability);
                if (rnd2 == 0)
                {
                    WaitAndContinue(true, 5);
                }
                else
                {
                    FindNextTarget();
                }
                break;

        }

    }

    private async UniTask WaitAndContinue(bool next, int timeToWait)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(timeToWait));
        if(next) FindNextTarget();
        else { DecideNextMove();}

    }

    public void FindNextTarget()
    {
        SetTarget(target.Neighbors[Random.Range(0, target.Neighbors.Count)]);
        _arrived = false;
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
