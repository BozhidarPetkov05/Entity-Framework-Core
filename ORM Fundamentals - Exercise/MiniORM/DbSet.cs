using System.Collections;

namespace MiniORM
{
    public class DbSet<T> : ICollection<T>
        where T : class, new()
    {
        internal ChangeTracker<T> ChangeTracker { get; }
        internal IList<T> Entities { get; }

        public int Count => Entities.Count;

        public bool IsReadOnly => Entities.IsReadOnly;

        public DbSet(IEnumerable<T> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            Entities = entities.ToList();
            ChangeTracker = new ChangeTracker<T>(Entities);
        }

        public void Add(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            Entities.Add(item);
            ChangeTracker.Add(item);
        }

        public void Clear()
        {
            while (Entities.Count > 0)
            {
                RemoveLast();
            }
        }

        public bool Contains(T item)
        {
            return Entities.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Entities.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var canBeRemoved = Entities.Remove(item);
            if (canBeRemoved)
            {
                ChangeTracker.Remove(item);
            }
            return canBeRemoved;
        }

        private void RemoveLast()
        {
            var entityToRemove = Entities[Entities.Count - 1];
            Entities.RemoveAt(Entities.Count - 1);
            ChangeTracker.Remove(entityToRemove);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
