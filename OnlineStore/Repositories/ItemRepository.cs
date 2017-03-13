using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Repositories
{
    public class ItemRepository
    {
        public void Add(Item item)
        {
            if (item == null)
                throw new ApplicationException("Invalid item.");
            lock (mItemsLock)
            {
                if (FindByName(item.Name) != null)
                    throw new ApplicationException("Duplicate item.");
                mItems.Add(item);
            }
        }

        public IEnumerable<Item> GetAll()
        {
            lock (mItemsLock)
            {
                return mItems.Select(item => item.Clone());
            }
        }

        public Item FindByName(string name)
        {
            lock (mItemsLock)
            {
                return mItems.Find((x) => x.Name == name);
            }
        }

        public void DeleteAll()
        {
            lock (mItemsLock)
            {
                mItems.Clear();
            }
        }

        private static List<Item> mItems = new List<Item>();
        private static readonly object mItemsLock = new object();
    }
}