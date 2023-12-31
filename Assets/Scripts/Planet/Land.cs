using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using Unity.Mathematics;
using DG.Tweening;

namespace Planet
{
    public class Land : MonoBehaviour, IHeatProvider
    {
        [SerializeField] private List<BuildingObjectsContainer> _buildingObjects;
        [SerializeField] private bool _isSea;
        [SerializeField] private int _forestHeatt =2;
        [SerializeField] private LandHeatAddedText _landHeatAddedText;
        [SerializeField] private float curBuildTime;
        [SerializeField] private ParticleSystem poofDestroy;
        private float buildTime = 5;
        private int _amountNeighbors;
        private Material _material;
        private int _level;
        private BuildingType _buildingType;
        private List<GameObject> _objects = new();
        private float _timerSeconds;
        private const float _nextLevelSeconds = 30f;
        public bool IsSea => _isSea;
        public Action madeForest;
        public Action madeBuilding;

        private Dictionary<int, int> _levelToBuildingsMaxHeartDits = new Dictionary<int, int>()
        {
            {1, 1},
            {2, 2},
            {3, 2},
            {4, 3},
        };
        
        [SerializeField] private int _forestMaxHearts = 3; 
        public int Id { get; set; }
        [field: SerializeField]
        public List<Land> Neighbors { get; set; }
        public Vector3 Position { get; set; }
        [SerializeField] private float _maxSecondsBetweenHitsBeforeReset;

        private int _heart;
        public int Heart
        {
            get
            {
                return _heart;
            }
            private set
            {
                _heart = value;
            }
        }

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
        
        [SerializeField] float _shakeDuration = 0.3f;
        [SerializeField] float _shakeStrength = 1f;
        [SerializeField] int _shakeVib = 5;
        [SerializeField] float _shakeRandomness = 10f;
        
        private void Start()
        {
            HeatSystem.Instance.AddHeatProvider(this);
            if (_landHeatAddedText != null)
            {
                _landHeatAddedText = Instantiate(_landHeatAddedText, transform);
                _landHeatAddedText.transform.position = Position;
            }
        }
        
        private void OnDestroy()
        {
            HeatSystem.Instance.RemoveHeatProvider(this);
        }

        private void UpdateColorView()
        {
            if (!_isSea)
            {
                switch (BuildingType)
                {
                    case BuildingType.None:
                        _material?.SetFloat("_Color", 0.65f);
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
            if (_isSea)
            {
                _material.SetVector("_Center", Position);
            }
        }

        private void OnMouseEnter()
        {
            if (GameButtons.Instance != null && GameButtons.Instance.IsPaused)
            {
                return;
            }
            
            LandClickedManager.Instance.LandMouseEnter(this);
            _material.SetFloat("_OutLineOpacity", 1f);
        }

        private void OnMouseExit()
        {
            if (GameButtons.Instance != null && GameButtons.Instance.IsPaused)
            {
                return;
            }
            
            LandClickedManager.Instance.LandMouseExit(this);
            _material.SetFloat("_OutLineOpacity", 0f);
        }

        private void OnMouseUpAsButton()
        {
        }

        private void OnMouseDown()
        {
            if (GameButtons.Instance != null && GameButtons.Instance.IsPaused)
            {
                return;
            }
            
            LandClickedManager.Instance.LandMouseDown(this);
        }

        private void OnMouseUp()
        {
            if (GameButtons.Instance != null && GameButtons.Instance.IsPaused)
            {
                return;
            }
            
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
            if (!IsSea && _buildingObjects.Find(i => i.Type == buildType).Objects.Count >= level)
            {
                _timerSeconds = 0f;
                var index = 0;
                Level = level;
                BuildingType = buildType;
                var listPrefabs = _buildingObjects.Find(i => i.Type == buildType).Objects.Where(i => i.Level == Level).ToList();
                var randomIndex = Random.Range(0, listPrefabs.Count());
                var buildPrefab = listPrefabs[randomIndex].Prefab;
                SetHeartsToMaximum();
                
                var inst = Instantiate(buildPrefab, this.transform);
                inst.transform.position = Vector3.zero;
                inst.transform.rotation = Quaternion.Euler(Vector3.zero);
                inst.transform.position = Position;

                inst.transform.LookAt(transform);

                _objects.ForEach(j => Destroy(j.gameObject));
                _objects.Clear();

                _objects.Add(inst);

                if (buildType == BuildingType.Forest)
                {
                    ShakeCamera.Instance.Shake();
                    madeForest?.Invoke();
                    AudioManager.Instance.Play(AudioManager.SoundsType.PoofLevelUpBuilding);
                }
            }
        }

        public void DestroyBuilding()
        {
            _objects.ForEach(j => Destroy(j.gameObject));
            _objects.Clear();
            _buildingType = BuildingType.None;
            AudioManager.Instance.Play(AudioManager.SoundsType.BuildingDestroySuccess);
            Instantiate(poofDestroy,Position, quaternion.identity);
            _level = 0;
        }

        public async UniTask HitBuilding()
        {
            Heart--;
            _objects.ForEach(x => x.transform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVib, _shakeRandomness));
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
            Instantiate(poofDestroy,Position, quaternion.identity);
        }

        public bool HitForest()
        {
            Heart--;
            if (Heart == 0)
            {
                DestroyForest();
                return true;
            }

            return false;
        }

        [Serializable] private struct BuildingObjectsContainer
        {
            public BuildingType Type;
            public List<ObjectByLevel> Objects;
            
            [Serializable]
            public struct ObjectByLevel
            {
                public int Level;
                public GameObject Prefab;
            }
        }

        private void Update()
        {
            UpdateNextLevel();
        }

        private void UpdateNextLevel()
        {
            _timerSeconds += Time.deltaTime;
            if (_timerSeconds > _nextLevelSeconds)
            {
                _timerSeconds = 0f;

                if (_buildingType == BuildingType.Building && _level != 4)
                {
                    Build(_level + 1, BuildingType.Building);
                }
            }
        }

        public float HeatProvided
        {
            get
            {
                switch (_buildingType)
                {
                    case BuildingType.Forest: return -_forestHeatt ; break;
                    case BuildingType.Building: return _level*0.5f; break;
                    default:
                        return 0; 
                }
            }
        }

        public void OnHeatInterval()
        {
            var heatProvided = HeatProvided;

            if (heatProvided != 0)
            {
                //_landHeatAddedText.transform.rotation = Quaternion.FromToRotation(_landHeatAddedText.transform.up, _landHeatAddedText.transform.position) * transform.rotation;
                _landHeatAddedText.Play(heatProvided, HasDirectEyeContactToCamera()).Forget();
            }
        }

        private bool HasDirectEyeContactToCamera()
        {
            var dirToCamera = Camera.main.transform.position - Position;
            return Vector3.Dot(dirToCamera, Position)>0;
        }
        
        public void IncreaseBuildTime(float amount)
        {
            
            // play building animation;
            
            curBuildTime += amount;
            if (curBuildTime >= buildTime && BuildingType == BuildingType.None)
            {
                madeBuilding?.Invoke();
                curBuildTime = 0;
                Build(_level + 1, BuildingType.Building);
            }
        }
    }
}