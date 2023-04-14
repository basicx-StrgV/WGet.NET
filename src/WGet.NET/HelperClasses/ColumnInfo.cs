using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGetNET.HelperClasses
{
    internal class ColumnInfo
    {
        public string Name { get; set; } = string.Empty;
        public int StartIndex { get; set; } = 0;
        public int End { get; set; } = 0;

        ///<inheritdoc/>
        public override string ToString()
        {
            return Name;
        }
    }

    internal class ColumnInfoList : IList<ColumnInfo>, IEnumerable<ColumnInfo>, ICollection<ColumnInfo>
    {
        private List<ColumnInfo> _columns = new();

        ///<inheritdoc/>
        public ColumnInfo this[int index]
        {
            get => _columns[index];
            set => _columns[index] = value;
        }

        ///<inheritdoc/>
        public int Count => _columns.Count;

        ///<inheritdoc/>
        public bool IsReadOnly => false;

        ///<inheritdoc/>
        public void Add(ColumnInfo item)
        {
            _columns.Add(item);
        }

        ///<inheritdoc/>
        public void Clear()
        {
            _columns.Clear();
        }

        ///<inheritdoc/>
        public bool Contains(ColumnInfo item)
        {
            return _columns.Contains(item);
        }

        ///<inheritdoc/>
        public void CopyTo(ColumnInfo[] array, int arrayIndex)
        {
            _columns.CopyTo(array, arrayIndex);
        }

        ///<inheritdoc/>
        public IEnumerator<ColumnInfo> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        ///<inheritdoc/>
        public int IndexOf(ColumnInfo item)
        {
            return _columns.IndexOf(item);
        }

        ///<inheritdoc/>
        public void Insert(int index, ColumnInfo item)
        {
            _columns.Insert(index, item);
        }

        ///<inheritdoc/>
        public bool Remove(ColumnInfo item)
        {
            return _columns.Remove(item);
        }

        ///<inheritdoc/>
        public void RemoveAt(int index)
        {
            _columns.RemoveAt(index);
        }

        ///<inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _columns.GetEnumerator();
        }
    }
}
