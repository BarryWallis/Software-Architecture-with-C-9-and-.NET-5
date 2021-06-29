using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SmartSearch
{
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public class SmartDictionary<T>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {
        private List<T> _allItems;
        private readonly Func<T, string> _keyAccessor;
        protected class Rater
        {
            public T Item { get; set; }
            public int FoundChars { get; set; } = 0;
            public double Penalty { get; set; } = 0;
        }
        public SmartDictionary(Func<T, string> keyAccessor, IEnumerable<T> allItems)
        {
            if (allItems == null)
            {
                throw new ArgumentNullException(nameof(allItems));
            }

            _keyAccessor = keyAccessor ?? throw new ArgumentNullException(nameof(keyAccessor));
            _allItems = allItems.ToList();
        }
        public void Replace(IEnumerable<T> newItems)
        {
            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            _allItems = newItems.ToList();
        }
        public void Add(T newItem)
        {
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem));
            }

            _allItems.Add(newItem);
        }
        protected Rater[] Rate(string s)
        {
            Rater[] result = new Rater[_allItems.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = RateItem(s, new Rater
                {
                    Item = _allItems[i]
                });
            }
            return result;
        }
        protected Rater RateItem(string search, Rater x)
        {
            string toSearch = search.ToLower(CultureInfo.CurrentCulture);
            string destination = _keyAccessor(x.Item).ToLower(CultureInfo.CurrentCulture);
            bool firstMatch = true;
            for (int j = 0; j < toSearch.Length; j++)
            {
                if (destination == string.Empty)
                {
                    return x;
                }

                char currChar = toSearch[j];
                int index = destination.IndexOf(currChar);
                if (index == -1)
                {
                    continue;
                }

                x.FoundChars++;
                if (firstMatch)
                {
                    x.Penalty += index;
                    firstMatch = false;

                }
                else
                {
                    x.Penalty += index * 1000;
                }

                destination = index + 1 < destination.Length ? destination[(index + 1)..] : string.Empty;
            }
            return x;
        }
        public IEnumerable<T> Search(string search, int maxItems)
        {
            if (search == null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            if (maxItems <= 0)
            {
                throw new ArgumentException($"{nameof(maxItems)} must be greater than zero", nameof(maxItems));
            }

            Rater[] pres = Rate(search);
            Array.Sort(pres, new Comparison<Rater>((x, y) => x.FoundChars > y.FoundChars ? -1 : x.FoundChars < y.FoundChars ? 1 : x.Penalty.CompareTo(y.Penalty)));
            return pres.Take(maxItems).Select(m => m.Item);
        }
    }
}
