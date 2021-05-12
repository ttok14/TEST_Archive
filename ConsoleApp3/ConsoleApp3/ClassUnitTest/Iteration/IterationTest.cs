using System;
using System.Collections;
using System.Collections.Generic;
// using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.ClassUnitTest.Iteration
{
    /// <summary>
    /// 다음과도 같은 순회 관련 인터페이스들 Test .
    /// <see cref="ICollection"/>
    /// <see cref="IList"/>
    /// <see cref="IEnumerable"/>
    /// <see cref="IEnumerator"/>
    /// </summary>
    class IterationTest
    {
        /// <summary>
        /// [물건] 을 의미함 . 
        /// </summary>
        public class Item
        {
            public ulong uid;
            public string name;
            public int cnt;
            public int grade;

            public Item(ulong uid, string name, int cnt, int grade)
            {
                this.uid = uid;
                this.name = name;
                this.cnt = cnt;
                this.grade = grade;
            }
        }

        #region ========= IList 로 Inventory 구현하기 ===============
        /// <summary>
        /// <see cref="=IList"/> 는 기본적으로 컬렉션 자료구조를 위한 <see cref="=ICollection"/> 을 내부적으로 
        /// 상속받고 있음 . 즉 기존 ICollection + List 를 위한 배열 접근자 및 순회를 위한 <see cref="IEnumerable"/> 도 
        /// 상속받고 있음.
        /// </summary>
        public class JayceInventory_IList : IList<Item>
        {
            #region =========:: 인터페이스 구현 ::==========
            Item IList<Item>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            int ICollection<Item>.Count => throw new NotImplementedException();

            bool ICollection<Item>.IsReadOnly => throw new NotImplementedException();

            void ICollection<Item>.Add(Item item)
            {
                throw new NotImplementedException();
            }

            void ICollection<Item>.Clear()
            {
                throw new NotImplementedException();
            }

            bool ICollection<Item>.Contains(Item item)
            {
                throw new NotImplementedException();
            }

            void ICollection<Item>.CopyTo(Item[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            int IList<Item>.IndexOf(Item item)
            {
                throw new NotImplementedException();
            }

            void IList<Item>.Insert(int index, Item item)
            {
                throw new NotImplementedException();
            }

            bool ICollection<Item>.Remove(Item item)
            {
                throw new NotImplementedException();
            }

            void IList<Item>.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }
            #endregion
        }
        #endregion

        #region ========= ICollection 로 Inventory 구현하기 ==========

        /// <typeparam name="Item"> 인벤토리에 담을수 있는 아이템 타입 </typeparam>
        /// ** ICollection 에는 IList 에는 있는 배열 접근자 등 이 없음 . ** 
        public class JayceInventory_ICollection : ICollection<Item>
        {
            /// <summary>
            /// 아이템들 
            /// </summary>
            protected Item[] items;

            /// <summary>
            /// 내부는 배열이기에 capacity 와 size 가 존재 
            /// </summary>
            protected int capacity;
            protected int size;

            public JayceInventory_ICollection()
            {
                capacity = 1;
                SyncCapacity();
            }
            public JayceInventory_ICollection(int capacity)
            {
                this.capacity = capacity;
                if (capacity == 0)
                    capacity = 1;
                SyncCapacity();
            }

            #region ====:: 추가 별도 구현 ::====
            public int Capacity
            {
                get => this.capacity;
                set
                {
                    this.capacity = value;
                    SyncCapacity();
                }
            }

            /// <summary>
            /// 현재 capacity 에 맞게 <see cref="items"/> 을 할당 또는 제거
            /// </summary>
            void SyncCapacity()
            {
                int length = items != null ? items.Length : 0;

                /// <see cref="capacity"/> 만큼 요소를 할당하되 , default 세팅 
                if (capacity > length)
                {
                    var ori = items;
                    items = new Item[capacity];

                    if (ori != null)
                        Array.Copy(ori, items, ori.Length);
                    /// 애는 byte 로 copy range 를 정하는데.. 어케가져오냐 사이즈 . marshal 써야함 ? 
                    /// Buffer.BlockCopy(ori, 0, items, 0, );
                }
                /// downsize 케이스 . 
                else if (capacity < length)
                {
                    var ori = items;
                    items = new Item[capacity];
                    Array.Copy(ori, items, capacity);
                }
            }

            /// <summary>
            /// uid 로 아이템 가져오기 
            /// </summary>
            public Item Take(ulong uid)
            {
                if (items == null)
                    return default;

                Item result = default;

                for (int i = 0; i < size; i++)
                {
                    var t = items[i];
                    if (t != null
                        && t.uid == uid)
                    {
                        result = t;
                        break;
                    }
                }

                return result;
            }

            /// <summary>
            /// 인덱스로 데이터를 가져옴 . 
            /// => ** 보통 배열 operator 로 overriding 해서 가져오는거는
            /// <see cref="IList"/> 에 존재 . 
            /// => ** <see cref="ICollection"/> 에는 해당 operator 가 없음 . 이런거때문에 
            /// <see cref="List{T}"/> 를 보면은 내부에 <see cref="ICollection"/> , <see cref="IList"/> 등 전부 가지고있지 . 
            /// </summary>
            public Item TakeByIndex(int idx)
            {
                if (items == null || idx >= items.Length)
                    return default;

                return items[idx];
            }

            public void SetByIndex(int index, Item target)
            {
                if (index >= size)
                {
                    return;
                }

                items[index] = target;
            }

            #endregion

            #region ======:: 인터페이스 함수 구현 ::======

            public int Count => size;

            public bool IsReadOnly => false;

            public void Add(Item item)
            {
                /// 현재 size 가 한계에 다다랐다면 
                /// 확장함. 
                if (size == capacity)
                {
                    capacity *= 2;
                    SyncCapacity();
                }

                size++;
                items[size - 1] = item;
            }

            public void Clear()
            {
                size = 0;
                items = null;
            }

            public bool Contains(Item item)
            {
                bool contain = false;

                for (int i = 0; i < size; i++)
                {
                    /// 고유 unique id 가 같으면 같은 아이템으로 판정한다 
                    if (items[i] != null && items[i].uid == item.uid)
                    {
                        contain = true;
                        break;
                    }
                }

                return contain;
            }

            public void CopyTo(Item[] array, int arrayIndex)
            {
                if (items == null)
                    return;

                Array.Copy(this.items, arrayIndex, array, 0, this.size);
            }

            public IEnumerator<Item> GetEnumerator()
            {
                return new ItemEnumerator(this);
            }

            public bool Remove(Item item)
            {
                if (items == null)
                    return false;

                int targetIndex = -1;

                for (int i = 0; i < size; i++)
                {
                    if (items[i].uid == item.uid)
                    {
                        targetIndex = i;
                        break;
                    }
                }

                /// removeTarget 을 찾았다면 요소들을 하나씩 앞으로 땡겨서
                /// target 을 덮어버림 . 
                /// => 삭제됨 . 
                /// => ++ down size 
                if (targetIndex != -1)
                {
                    for (int i = targetIndex; i < size; i++)
                    {
                        items[i] = i + 1 < size ? items[i + 1] : default;
                    }

                    size--;
                }

                return targetIndex != -1;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return items.GetEnumerator();
            }

            #endregion

            public class ItemEnumerator : IEnumerator<IterationTest.Item>
            {
                JayceInventory_ICollection col;
                int curIndex;
                IterationTest.Item curItem;

                IterationTest.Item IEnumerator<IterationTest.Item>.Current => curItem;

                object IEnumerator.Current => throw new NotImplementedException();

                public ItemEnumerator(JayceInventory_ICollection ori)
                {
                    curIndex = -1;
                    col = ori;
                    curItem = default;
                }

                void IDisposable.Dispose()
                {

                }

                bool IEnumerator.MoveNext()
                {
                    curIndex++;

                    if (curIndex >= col.size)
                    {
                        curItem = default;
                        return false;
                    }
                    else
                    {
                        curItem = col.TakeByIndex(curIndex);
                        return true;
                    }
                }

                void IEnumerator.Reset()
                {
                    curItem = default;
                    curIndex = -1;
                }
            }

        }

        #endregion

        #region ======== ICollection, IEnumerable 을 이용해서 IList 처럼 Custom 하게 자료구조 만들기 ==========
        public class JayceCustom : ICollection<Item>, IEnumerable<Item>, IEnumerable
        {
            #region ICollection Implementation
            int ICollection<Item>.Count => throw new NotImplementedException();

            bool ICollection<Item>.IsReadOnly => throw new NotImplementedException();

            void ICollection<Item>.Add(Item item)
            {
                throw new NotImplementedException();
            }

            void ICollection<Item>.Clear()
            {
                throw new NotImplementedException();
            }

            bool ICollection<Item>.Contains(Item item)
            {
                throw new NotImplementedException();
            }

            void ICollection<Item>.CopyTo(Item[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            bool ICollection<Item>.Remove(Item item)
            {
                throw new NotImplementedException();
            }
            #endregion

            #region IEnumerable Implementaion
            IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
            #endregion
        }
        #endregion

        public void RunTest()
        {
            #region IColletion

            var col = new IterationTest.JayceInventory_ICollection();

            col.Add(new Item(100, "Item01", 10, 2));
            col.Add(new Item(200, "Item02", 10, 2));
            col.Add(new Item(300, "Item03", 10, 2));
            col.Add(new Item(400, "Item04", 10, 2));

            col.SetByIndex(1, new Item(1500, "Inserted", 10010, 5));

            for (int i = 0; i < col.Count; i++)
            {
                //  Console.WriteLine(col.TakeByIndex(i).name);
            }

            var e = col.GetEnumerator();

            while (e.MoveNext())
            {
                Console.WriteLine(e.Current.name);
            }

            #endregion

            #region IList 


            #endregion
        }
    }
}
