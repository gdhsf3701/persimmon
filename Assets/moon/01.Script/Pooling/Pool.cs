using System.Collections.Generic;
using UnityEngine;

namespace moon._01.Script.Pooling
{
    public class Pool
    {
        private readonly Transform _prefab;
        private readonly Transform _parent;
        private readonly Stack<Transform> _pool;

        public Pool(Transform prefab, Transform parent , int initialSize = 1)
        {
            _prefab = prefab;
            _parent = parent;
            _pool = new Stack<Transform>();

            for (int i = 0; i < initialSize; i++)
            {
                Transform item = Object.Instantiate(_prefab, _parent);
                item.gameObject.SetActive(false);
                _pool.Push(item);
            }
        }

        public Transform Pop()
        {
            if (_pool.Count > 0)
            {
                GameObject item = _pool.Pop().gameObject;
                item.SetActive(true);
                return item.transform;
            }
            else
            {
                return Object.Instantiate(_prefab, _parent);
            }
        }

        public void Push(Transform item)
        {
            item.gameObject.SetActive(false);
            item.SetParent(_parent);
            _pool.Push(item);
        }
    }
}