using System.Collections.Generic;
using Plugins.ScriptFinder.RunTime.Serializable;
using UnityEngine;

namespace Plugins.ScriptFinder.RunTime.Finder
{
    [CreateAssetMenu(fileName = "ScriptFinder",menuName = "SO/ScriptFinder", order = 0)]
    public class ScriptFinderSO : ScriptableObject
    {
        [Header("MonoBehaviour type to find in the scene")]
        [SerializeField]
        private SerializableType _keyType;
        [SerializeField]
        private bool _findAll = false;
        private Transform _targetTransform;

        private MonoBehaviour _target;
        private List<MonoBehaviour> _targetsComponent;
        private List<Transform> _targetsTransform;
        public SerializableType KeyType => _keyType;
        public bool FindAll => _findAll;
        public void SetTarget(MonoBehaviour target)
        {
            _target = target;
            _targetTransform = target?.transform;
        }
        
        public void SetTargets(List<MonoBehaviour> targets)
        {
            _targetsComponent = targets;
            _targetsTransform = targets.ConvertAll(t => t.transform);
        }
        
        public T GetTarget<T>() where T : MonoBehaviour
        {
            if (_target is T t) return t;
            Debug.LogError($"ScriptFinderSO: Target is not of type {typeof(T).Name}. Current type: {_target?.GetType().Name ?? "null"}");
            return null;
        }
        
        public List<T> GetTargets<T>() where T : MonoBehaviour
        {
            if(_targetsComponent.Count > 0)
            {
                List<T> result = new List<T>();
                foreach (var component in _targetsComponent)
                {
                    if (component is T t) result.Add(t);
                    else
                    {
                        Debug.LogError($"ScriptFinderSO: Target is not of type {typeof(T).Name}. Current type: {component?.GetType().Name ?? "null"}");
                    }
                }
                return result;
            }
            Debug.LogError($"ScriptFinderSO: No targets found for type {typeof(T).Name}.");
            return null;
        }

        public Transform GetTargetTransform()
        {
            return _targetTransform;
        }
        
        public List<Transform> GetTargetsTransform()
        {
            return _targetsTransform;
        }
    }
}