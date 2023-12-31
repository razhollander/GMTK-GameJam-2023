using System;
using Audio;
using Cysharp.Threading.Tasks;
using Planet;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Elements
{
    public class TornadoElement : MonoBehaviour
    {
        private const float Epsilon = 0.1f;

        [SerializeField] private Transform rayPos;
        [SerializeField] private LayerMask world;
        [SerializeField] private float speed = 1;
        [SerializeField] private float radius = 5;
        [SerializeField] private float _aliveSeconds = 60;
        private Vector3 wayUp;
        private Vector3 center = Vector3.zero;
        private Vector3 _lastPos;
        private float _secondsTimer;

        private Land _firstLand;
        private Land target;

        private void Awake()
        {
            AudioManager.Instance.Play(AudioManager.SoundsType.Tornado);
        }

        public void SetFirstLand(Land land)
        {
            _firstLand = land;
            var counterWhile = 0;
            target = null;
            while ((target == null || target == land || target.IsSea) && counterWhile < 15)
            {
                counterWhile++;
                var randomIndex = Random.Range(0, land.Neighbors.Count);
                target = land.Neighbors[randomIndex];
            }

            if (target.IsSea)
            {
                target = PlanetManager.Instance.Lands.Find(i => !i.IsSea);
            }

            HitAsync();
        }

        private void Update()
        {
            _secondsTimer += Time.deltaTime;
            var ray = new Ray(rayPos.position, -transform.up);
            Physics.Raycast(ray, out var hit, 1, world);
            wayUp = hit.normal;
            if (target) MoveTowardsTarget(target);

            if ((transform.position - target.Position).magnitude < Epsilon)
            {
                UpdateTarget();
            }

            if (_secondsTimer > _aliveSeconds)
            {
                Destroy(gameObject);
            }
        }

        private void UpdateTarget()
        {
            SetFirstLand(target);
        }

        private void MoveTowardsTarget(Land target)
        {
            var direction = (target.Position - transform.position).normalized;
            var cross = Vector3.Cross(direction, wayUp);
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

            if (myTransform.position - _lastPos != Vector3.zero)
            {
                myTransform.rotation = Quaternion.FromToRotation(transform.up, myTransform.position) * transform.rotation;
            }
        }

        private async UniTask HitAsync()
        {
            float secondsForEachLand = 1.5f;
            float secondsForHit = 0.3f;
            float timerOnLand = 0;
            float timerHit = 0;

            while (timerOnLand < secondsForEachLand)
            {
                timerOnLand += Time.deltaTime;
                timerHit += Time.deltaTime;

                if (timerHit < secondsForHit)
                {
                    timerHit = 0;
                    if (_firstLand.BuildingType != BuildingType.Forest)
                    {
                        _firstLand.HitBuilding();
                    }
                }

                await UniTask.DelayFrame(1);
            }

            timerOnLand = 0;
            
            while (timerOnLand < secondsForEachLand)
            {
                timerOnLand += Time.deltaTime;
                timerHit += Time.deltaTime;

                if (timerHit < secondsForHit)
                {
                    timerHit = 0;
                    if (_firstLand.BuildingType != BuildingType.Forest)
                    {
                        _firstLand.HitBuilding();
                    }
                }

                await UniTask.DelayFrame(1);
            }
        }
    }
}