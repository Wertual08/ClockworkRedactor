using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Resources.Interface
{

    public class InterfaceElementsList : IList<InterfaceElementBase>
    {
        private InterfaceElementBase OwnerStorage = null;
        private List<InterfaceElementBase> Elements = new List<InterfaceElementBase>();

        public InterfaceElementsList() { }
        public InterfaceElementsList(InterfaceElementBase elem) { OwnerStorage = elem; }
        public InterfaceElementsList(IList<InterfaceElementBase> list)
        {
            if (list != null) foreach (var element in list) Add(element);
        }

        public InterfaceElementBase Owner
        {
            get => OwnerStorage;
            set
            {
                OwnerStorage = value;
                foreach (var element in Elements) element.Owner = OwnerStorage;
            }
        }

        public InterfaceElementBase this[int index]
        {
            get => Elements[index];
            set
            {
                value.Owner = OwnerStorage;
                Elements[index] = value;
            }

        }

        public int Count => Elements.Count;

        public bool IsReadOnly => false;

        public void Add(InterfaceElementBase item)
        {
            item.Owner = Owner;
            Elements.Add(item);
        }

        public void Clear()
        {
            Elements.Clear();
        }

        public bool Contains(InterfaceElementBase item)
        {
            return Elements.Contains(item);
        }

        public void CopyTo(InterfaceElementBase[] array, int arrayIndex)
        {
            Elements.CopyTo(array, arrayIndex);
        }

        public IEnumerator<InterfaceElementBase> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        public int IndexOf(InterfaceElementBase item)
        {
            return Elements.IndexOf(item);
        }

        public void Insert(int index, InterfaceElementBase item)
        {
            item.Owner = Owner;
            Elements.Insert(index, item);
        }

        public bool Remove(InterfaceElementBase item)
        {
            if (item.Owner == Owner) item.Owner = null;
            return Elements.Remove(item);
        }

        public void RemoveAt(int index)
        {
            var item = Elements[index];
            if (item.Owner == Owner) item.Owner = null;
            Elements.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
