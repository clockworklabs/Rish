using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace RishUI.Deprecated.Input
{
    internal class EventsList
    {
        private HashSet<int> Set { get; } = new HashSet<int>();
        private List<PointerEventData> List { get; } = new List<PointerEventData>();
        
        public int Count => List.Count;

        public PointerEventData this[int index] => List[index];

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public bool Add(PointerEventData eventData)
        {
            var id = eventData.pointerId;
            if (Set.Contains(id))
            {
                return false;
            }

            Set.Add(id);
            List.Add(eventData);

            return true;
        }

        public PointerEventData GetById(int id)
        {
            var index = FindIndex(id);
            return index < 0 ? null : List[index];
        }

        public bool Remove(PointerEventData eventData) => Remove(eventData.pointerId);

        public bool Remove(int id)
        {
            var index = FindIndex(id);
            return RemoveAt(index);
        }
        
        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= List.Count)
            {
                return false;
            }

            var id = List[index].pointerId;
            
            Set.Remove(id);
            List.RemoveAt(index);

            return true;
        }
        
        public int FindIndex(int id) => !Contains(id) ? -1 : List.FindIndex(data => data.pointerId == id);

        public void Clear()
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                RemoveAt(i);
            }
        }

        public bool Contains(PointerEventData eventData) => Contains(eventData.pointerId);
        public bool Contains(int id) => Set.Contains(id);

        public struct Enumerator
        {
            private readonly EventsList _list;

            private int _index;
            private PointerEventData _current;

            public Enumerator(EventsList list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }

            public PointerEventData Current => _current;

            public bool MoveNext()
            {
                if (_index >= _list.Count)
                {
                    return false;
                }
                
                _current = _list[_index++];
                
                return true;
            }
        }
    }
}
