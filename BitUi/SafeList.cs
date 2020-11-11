using System;
using System.Collections.Generic;

namespace BitUi
{

	public class SafeList<T> where T : class
	{
		public List<T> list = new List<T>(); // TODO should be a read only list, or provide a read-only iterator...
		private List<T> nextList = new List<T>();

		private bool dirty;
		private bool iterating;

		public int Count => list.Count;

		public void Add(T e)
		{
			if (iterating)
			{
				InitNextList();
				nextList.Add(e);
			}
			else
			{
				list.Add(e);
			}
		}

		public void Remove(T e)
		{
			if (iterating)
			{
				InitNextList();
				nextList.Remove(e);
			}
			else
			{
				list.Remove(e);
			}
		}

		public void Insert(int index, T e)
		{
			if (iterating)
			{
				InitNextList();
				nextList.Insert(index, e);
			}
			else
			{
				list.Insert(index, e);
			}
		}

		public void ForEach(Action<T> action)
		{
			if (iterating) throw new Exception("Cannot peform double iteration!");

			iterating = true;
			foreach (var e in list)
			{
				action(e);
			}
			iterating = false;
			FlushNextList();
		}

		private void InitNextList()
		{
			if (!dirty)
			{
				nextList.AddRange(list);
				dirty = true;
			}
		}

		private void FlushNextList()
		{
			if (dirty)
			{
				list.Clear();
				list.AddRange(nextList);
				nextList.Clear();
				dirty = false;
			}
		}
	}

}