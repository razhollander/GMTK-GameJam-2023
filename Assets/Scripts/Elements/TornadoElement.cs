using Cysharp.Threading.Tasks;
using Planet;
using UniRx;
using UnityEngine;

namespace Elements
{
    public class TornadoElement : MonoBehaviour
    {
        private const float Epsilon = 0.1f;

        [SerializeField] private Transform rayPos;
        [SerializeField] private LayerMask world;
        [SerializeField] private float yOffset = .1f;
        [SerializeField] private Transform renderer;
        [SerializeField] private float speed = 1;
        [SerializeField] private float radius = 5;
        [SerializeField] private float _aliveSeconds = 10;
        private Vector3 wayUp;
        private Vector3 center = Vector3.zero;
        private Vector3 _lastPos;
        private float _secondsTimer;

        private Land _firstLand;
        private Land target;

        public void SetFirstLand(Land land)
        {
            _firstLand = land;
            var counterWhile = 0;
            target = null;
            while ((target == null || target == land || target.Vertex != 6) && counterWhile < 15)
            {
                counterWhile++;
                var randomIndex = Random.Range(0, land.Neighbors.Count);
                target = land.Neighbors[randomIndex];
            }

            if (target.Vertex == 5)
            {
                target = PlanetManager.Instance.Lands.Find(i => i.Vertex == 6);
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
            float secondsForEachLand = 2f;
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
                    _firstLand.HitBuilding();
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
                    target.HitBuilding();
                }

                await UniTask.DelayFrame(1);
            }
        }
    }
}