// Copyright (c) 2002,2003,2004, Rüdiger Klaehn
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of lambda computing nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#endregion

namespace System.Collections.Generic
{
    [Serializable]
    public class Set<T> : ICollection<T>,IEnumerable<T>,ICollection,IEnumerable
    {
        private struct Dummy {}
        private static Dummy dummy=new Dummy();
        private Dictionary<T, Dummy> data;
        public Set() 
        {
            data = new Dictionary<T, Dummy>();
        }
        public Set(int capacity)
        {
            data = new Dictionary<T, Dummy>(capacity);
        }
        public Set(Set<T> original)
        {
            data = new Dictionary<T, Dummy>(original.data);
        }
        public Set(IEnumerable<T> original)
        {
            data = new Dictionary<T, Dummy>();
            AddRange(original);
        }
        public int Count
        {
            get { return data.Count; }
        }
        public void Add(T a)
        {
            data[a] = dummy;
        }
        public void AddRange(IEnumerable<T> range)
        {
            foreach (T a in range)
                Add(a);
        }
        public Set<U> ConvertAll<U>(Converter<T, U> converter)
        {
            Set<U> result = new Set<U>(this.Count);
            foreach (T element in this)
                result.Add(converter(element));
            return result;
        }
        public bool TrueForAll(Predicate<T> predicate)
        {
            foreach (T element in this)
                if (!predicate(element))
                    return false;
            return true;
        }
        public Set<T> FindAll(Predicate<T> predicate)
        {
            Set<T> result = new Set<T>();
            foreach (T element in this)
                if (predicate(element))
                    result.Add(element);
            return result;
        }
        public T PickOne()
        {
            T res = default(T);
            foreach(T  t in this)
            {
                res = t;
                break;
            }
            return res;
        }
        public void ForEach(Action<T> action)
        {
            foreach (T element in this)
                action(element);
        }
        public void Clear()
        {
            data.Clear();
        }
        public bool Contains(T a)
        {
            return data.ContainsKey(a);
        }
        public bool Includes(Set<T> s)
        {
            bool res = true;
            if (this.Count < s.Count)
                return false;

            foreach(T e in s)
            {
                res = res && (this.Contains(e));
                if(!res)
                    break;
            }
            return res;
        }
        public bool IntersectionNotEmpty(Set<T> s)
        {
            bool res = false;
            Set<T> s1 = s;
            Set<T> s2 = this;

            if (this.Count < s.Count)
            {
                s1 = this; s2 = s;
            }
            foreach (T e in s1)
            {
                res = res || (s2.Contains(e));
                if (res)
                    break;
            }
            return res;
        }
        public bool IntersectionEmpty(Set<T> s)
        {
            return !IntersectionNotEmpty(s);
        }
        public void CopyTo(T[] array, int index)
        {
            data.Keys.CopyTo(array, index);
        }
        public bool Remove(T a)
        {
            return data.Remove(a);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return data.Keys.GetEnumerator();
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public static Set<T> operator |(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>(a);
            result.AddRange(b);
            return result;
        }
        public Set<T> Union(IEnumerable<T> b)
        {
            return this|new Set<T>(b);
        }
        public static Set<T> operator &(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
            foreach(T element in a)
                if(b.Contains(element))
                    result.Add(element);
            return result;
        }
        public Set<T> Intersection(IEnumerable<T> b)
        {
            return this & new Set<T>(b);
        }
        public static Set<T> operator -(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
            foreach (T element in a)
                if (!b.Contains(element))
                    result.Add(element);
            return result;
        }
        public Set<T> Difference(IEnumerable<T> b)
        {
            return this - new Set<T>(b);
        }
        public static Set<T> operator ^(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
            foreach (T element in a)
                if (!b.Contains(element))
                    result.Add(element);
            foreach (T element in b)
                if (!a.Contains(element))
                    result.Add(element);
            return result;
        }
        public Set<T> SymmetricDifference(IEnumerable<T> b)
        {
            return this ^ new Set<T>(b);
        }
        public static Set<T> Empty 
        { 
            get { return new Set<T>(0); }
        }

        public static Set<T> Singleton(T n)
        {
            Set<T> ns = new Set<T>();
            ns.Add(n);
            return ns;
        }


        public static bool operator <=(Set<T> a, Set<T> b)
        {
            foreach (T element in a)
                if (!b.Contains(element))
                    return false;
            return true;
        }
        public static bool operator < (Set<T> a,Set<T> b) 
        {
            return (a.Count < b.Count) && (a <= b);
        }
        //public static bool operator ==(Set<T> a, Set<T> b)
        //{
        //    return (a.Count == b.Count) && (a <= b);
        //}
        public static bool operator > (Set<T> a, Set<T> b)
        {
            return b < a;
        }
        public static bool operator >=(Set<T> a, Set<T> b)
        {
            return (b <= a);
        }
        //public static bool operator !=(Set<T> a, Set<T> b)
        //{
        //    return !(a == b);
        //}
        public override bool Equals(object obj)
        {
            Set<T> a = this;
            Set<T> b = obj as Set<T>;
            if (Object.Equals(b, null))
                return false;
            return (a.Count == b.Count) && (a <= b);
            // return a == b;
        }
        public override int GetHashCode()
        {
            int hashcode = 0;
            foreach (T element in this)
                hashcode ^= element.GetHashCode();
            return hashcode;
        }
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)data.Keys).CopyTo(array, index);
        }
        object ICollection.SyncRoot
        {
            get { return ((ICollection)data.Keys).SyncRoot; }
        }
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)data.Keys).IsSynchronized; }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)data.Keys).GetEnumerator(); 
        }
        public override string ToString()
        {
            string res = "{";
            foreach (T e in this)
                res += e.ToString() + " ,";
            if (res[res.Length - 1] == ',')
                res=res.Substring(0, res.Length - 2);
            res += "}";
            return res;
        }
    }
}
