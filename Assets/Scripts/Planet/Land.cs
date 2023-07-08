using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Planet
{
    public class Land : MonoBehaviour
    {
        [SerializeField] private List<BuildingObjectsContainer> _buildingObjects;

        private int _amountNeighbors;
        private Material _material;
        private int _level;
        private BuildingType _buildingType;
        private List<GameObject> _objects = new();

        private Dictionary<int, int> _levelToBuildingsMaxHeartDits = new Dictionary<int, int>()
        {
            {1, 3},
            {2, 4},
            {3, 5},
            {4, 7},
        };
        
        [SerializeField] private int _forestMaxHearts = 3; 
        public int Id { get; set; }
        public List<Land> Neighbors { get; set; }
        public Vector3 Position { get; set; }
        public int Vertex { get; set; }
        [SerializeField] private float _maxSecondsBetweenHitsBeforeReset;

        public int Heart { get; set; }
        public Action BuildingTypeChangedEvent;
        private UniTask _resetHeartCountdownTask;
        private CancellationTokenSource _resetHeartCountdownCancellationToken;

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;

                UpdateColorView();
            }
        }

        
        
        public BuildingType BuildingType
        {
            get => _buildingType;
            set
            {
                _buildingType = value;
                BuildingTypeChangedEvent?.Invoke();
                UpdateColorView();
            }
        }

        private void UpdateColorView()
        {
            if (Vertex == 6)
            {
                switch (BuildingType)
                {
                    case BuildingType.None:
                        _material.SetFloat("_Color", 0.65f);
                        break;
                    case BuildingType.Forest:
                        _material.SetFloat("_Color", 0f);
                        break;
                    case BuildingType.Building:
                        if (Level == 4)
                        {
                            _material.SetFloat("_Color", 1f);
                        }
                        else if (_level == 3)
                        {
                            _material.SetFloat("_Color", 0.9f);
                        }
                        else if (_level == 2)
                        {
                            _material.SetFloat("_Color", 0.8f);
                        }
                        else if (_level == 1)
                        {
                            _material.SetFloat("_Color", 0.7f);
                        }
                        else
                        {
                            _material.SetFloat("_Color", 0.65f);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(BuildingType), BuildingType, null);
                }
            }
        }
        
        public int AmountNeighbors
        {
            get => _amountNeighbors;
            set { _amountNeighbors = value; }
        }

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        private void OnMouseEnter()
        {
            LandClickedManager.Instance.LandMouseEnter(this);
            _material.SetFloat("_OutLineOpacity", 1f);
        }

        private void OnMouseExit()
        {
            LandClickedManager.Instance.LandMouseExit(this);
            _material.SetFloat("_OutLineOpacity", 0f);
        }

        private void OnMouseUpAsButton()
        {
            Debug.Log("Clicked");
        }

        private void OnMouseDown()
        {
            LandClickedManager.Instance.LandMouseDown(this);
        }

        private void OnMouseUp()
        {
            LandClickedManager.Instance.LandMouseUp(this);
        }

        public void BuildForest()
        {
            Build(1, BuildingType.Forest);
        }

        public void BuildBuilding(int level)
        {
            Build(level, BuildingType.Building);
        }

        private void Build(int level, BuildingType buildType)
        {
            if (Vertex == 6)
            {
                var index = 0;
                Level = level;
                BuildingType = buildType;
                var buildPrefab = _buildingObjects.Find(i => i.Type == buildType).Objects[Level - 1];
                SetHeartsToMaximum();
                
                var inst = Instantiate(buildPrefab, this.transform);
                inst.transform.position = Vector3.zero;
                inst.transform.rotation = Quaternion.Euler(Vector3.zero);
                inst.transform.position = Position;

                inst.transform.LookAt(transform);
                
                _objects.ForEach(j => Destroy(j.gameObject));
                _objects.Clear();
                
                _objects.Add(inst);
            }
        }

        public void DestroyBuilding()
        {
            _objects.ForEach(j => Destroy(j.gameObject));
            _objects.Clear();
            _buildingType = BuildingType.None;
        }

        public async UniTask HitBuilding()
        {
            Heart--;

            if (_resetHeartCountdownCancellationToken != null)
            {
                _resetHeartCountdownCancellationToken.Cancel();
            }
            
            if (Heart == 0)
            {
                DestroyBuilding();
                return;
            }
            
            _resetHeartCountdownCancellationToken = new CancellationTokenSource();
            ResetHeartsAfterDelay().Forget();
            
            // TODO : play hit effect
        }

        private async UniTask ResetHeartsAfterDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_maxSecondsBetweenHitsBeforeReset), cancellationToken: _resetHeartCountdownCancellationToken.Token);
            
            if (!_resetHeartCountdownCancellationToken.IsCancellationRequested)
            {
                SetHeartsToMaximum();
            }
        }

        private void SetHeartsToMaximum()
        {
            if(BuildingType == BuildingType.Building)
            {
                Heart = _levelToBuildingsMaxHeartDits[Level];
            }
            else
            {
                Heart = _forestMaxHearts;
            }
        }

        public void DestroyForest()
        {
            _objects.ForEach(j => Destroy(j.gameObject));
            _objects.Clear();
            _buildingType = BuildingType.None;
        }

        public void HitForest()
        {
            Heart--;
            // TODO : play hit effect
        }

        [Serializable] private struct BuildingObjectsContainer
        {
            public BuildingType Type;
            public List<GameObject> Objects;
        }
    }
}