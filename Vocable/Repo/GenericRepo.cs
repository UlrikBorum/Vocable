using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Vocable
{
    public class GenericRepo<T> : IRepo<T>, IEnumerable<T> where T : IContainId
    {

        #region instance fields
        private List<T> _items;
        #endregion

        #region Constructor
        public GenericRepo()
        {
            _items = new List<T>();
        }

        public GenericRepo(List<T> items)
        {
            _items = items;
        }
        #endregion

        #region properties
        // Expose the internal list while keeping backing field consistent
        public List<T> Items { get => _items; set => _items = value ?? new List<T>(); }
        #endregion



        public T Create(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // If id is not set or already exists, assign a unique id
            if (item.Id <= 0 || IdExists(item.Id))
            {
                item.Id = GetNextId();
            }

            _items.Add(item);
            return item;
        }



        public List<T> ReadAll()
        {
            return new List<T>(_items);
        }

        public T ReadById(int id)
        {
            T? item = _items.Find(item => item.Id == id);

            if (item == null)
                throw new KeyNotFoundException();

            return item;
        }

        public T Update(int id, T updatedItem)
        {
            if (updatedItem == null)
            {
                throw new ArgumentNullException(nameof(updatedItem));
            }

            T itemOld = ReadById(id);

            int placement = _items.IndexOf(itemOld);

            // Ensure the id remains unique.If updatedItem.Id conflicts with another item, assign a new unique id.
            if (updatedItem.Id != itemOld.Id)
            {
                bool conflict = IdExistsExcluding(updatedItem.Id, itemOld.Id);
                if (conflict || updatedItem.Id <= 0)
                {
                    updatedItem.Id = GetNextId();
                }
                else
                {
                    updatedItem.Id = id;
                }
            }
            updatedItem.Id = id;

            _items[placement] = updatedItem;

            return _items[placement];
        }

        public T Delete(int id)
        {
            T itemToDelete = ReadById(id);

            _items.Remove(itemToDelete);

            return itemToDelete;
        }
        private bool IdExists(int id)
        {
            foreach (var item in _items)
            {
                if (item.Id == id)
                    return true;
            }

            return false;
        }

        private bool IdExistsExcluding(int id, int excludeId)
        {

            return _items.Any(c => c.Id == id && c.Id != excludeId);
        }

        private int GetNextId()
        {
            int max = _items.Max(car => car.Id);
            return max + 1;
        }

        // Implement IEnumerable<T> so callers can use LINQ against this repo
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



    }
}
