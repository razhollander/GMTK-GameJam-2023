using System;
using Planet;
using Unity.Mathematics;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    private const float Epsilon = 0.1f;
    
    [SerializeField] private float speed = .5f;
    [SerializeField] private Land targetLand;
    [SerializeField] private float timeToSelfDestruct = 5;
    [SerializeField] private GameObject ground;
    private bool _hit;

    private void Update()
    {
        if(!targetLand) return;
        if ((targetLand.Position - transform.position).magnitude < Epsilon)
        {
            if (_hit) return;
            _hit = true;
            Instantiate(ground, transform.position, quaternion.identity);
            Destroy(gameObject);
            return;
        };
        transform.Translate(Time.deltaTime * speed * (targetLand.Position - transform.position).normalized);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject,timeToSelfDestruct);
    }

    #region getters&setters

        public void SetLand(Land land){targetLand = land;}
        
    #endregion
}
