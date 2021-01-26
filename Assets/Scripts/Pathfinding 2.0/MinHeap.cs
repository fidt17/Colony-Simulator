using System;

namespace Pathfinding
{
	public interface IHeapItem<T> : IComparable<T>
	{
		int HeapIndex { get; set; }
	}

	public class MinHeap<T> where T : IHeapItem<T>
	{
		public int Count { get; private set; }
		
		private readonly T[] _items;

		public MinHeap(int maxSize)
		{
			_items = new T[maxSize];
		}

		public void Add(T item)
		{
			item.HeapIndex = Count;
			_items[Count] = item;
			BubbleUp(item);
			Count++;
		}

		public T RemoveFirst()
		{
			var first = _items[0];
			Count--;
			_items[0] = _items[Count];
			_items[0].HeapIndex = 0;
			BubbleDown(_items[0]);
			return first;
		}

		public void UpdateItem(T item)
		{
			BubbleUp(item);
		}

		public bool Contains(T item)
		{
			return Equals(_items[item.HeapIndex], item);
		}

		private void BubbleDown(T item)
		{
			while (true)
			{
				int leftChild = item.HeapIndex  * 2 + 1;
				int rightChild = item.HeapIndex * 2 + 2;
				int swapIndex = 0;

				if (leftChild < Count)
				{
					swapIndex = leftChild;

					if (rightChild                                         < Count
					    && _items[leftChild].CompareTo(_items[rightChild]) < 0)
						swapIndex = rightChild;

					if (item.CompareTo(_items[swapIndex]) < 0)
						Swap(item, _items[swapIndex]);
					else
						return;
				}
				else
				{
					return;
				}
			}
		}

		private void BubbleUp(T item)
		{
			int parentIndex = (item.HeapIndex - 1) / 2;

			while (true)
			{
				var parentItem = _items[parentIndex];
				if (item.CompareTo(parentItem) > 0)
					Swap(item, parentItem);
				else
					break;

				parentIndex = (item.HeapIndex - 1) / 2;
			}
		}

		private void Swap(T itemA, T itemB)
		{
			_items[itemA.HeapIndex] = itemB;
			_items[itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}
	}
}