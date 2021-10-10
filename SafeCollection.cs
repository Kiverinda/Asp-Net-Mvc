using System.Collections.Generic;

namespace Mvc
{
    class SafeCollection<T>
    {
        private List<T> _currentList = new List<T>();
        private object lockObj = new object();

        public SafeCollection(){
            _currentList = new List<T>();
        }

        public void SafeAdd(T value)
        {
            lock (lockObj)
            {
                _currentList.Add(value);
            }
        }

        public void SafeRemove(int value)
        {
            lock (lockObj)
            {
                _currentList.RemoveAt(value);
            }
        }

        public List<T> GetList()
        {
            return _currentList;
        }
    }
}
