using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace Plugins.ScriptFinder.RunTime.Manager
{
    [DefaultExecutionOrder(-10)]
    public class FinderManager : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO[] finders;

        private void Awake()
        {
            if (finders == null || finders.Length == 0) return;
            Initialize();
        }

        private void Initialize()
        {
            var components = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            
            foreach (var finder in finders)
            {
                Type keyType = finder.KeyType?.Type;
                if (keyType == null) continue;
                if (finder.FindAll)
                {
                    var component = components.Where(c => c.GetType() == keyType).ToList();
                    if (component.Count > 0)
                    {
                        finder.SetTargets(component);
                    }
                }
                else
                {
                    var component = components.FirstOrDefault(c => c.GetType() == keyType);
                    if (component != null)
                    {
                        finder.SetTarget(component);
                    }
                }
            }
        }
    }
}