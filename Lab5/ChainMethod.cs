using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Lab5 {
    class ChainMethod<TValue> where TValue : IEquatable<TValue> {
        private struct Entry {
            public string Key;
            public TValue Value;
            public int Next;
            public Entry(string key, TValue value) {
                Key = key;
                Value = value;
                Next = -1;
            }
            public static bool operator == (Entry current, Entry other) {
                return current.Key == other.Key && current.Value.Equals(other.Value);
            }
            public static bool operator !=(Entry current, Entry other) {
                return current.Key != other.Key || !current.Value.Equals(other.Value);
            }
        }
        private int[] _buckets;
        private List<Entry> _entries;
        public ChainMethod(int capacity) {
            _buckets = new int[capacity];
            for(int i = 0; i < capacity; ++i) {
                _buckets[i] = -1;
            }
            _entries = new List<Entry>();
        }
        public long Add(string key, TValue value) {
            var sw = new Stopwatch();
            sw.Start();
            var hashCode = HashCalculator.Calculate(key);
            var index = hashCode % _buckets.Length;
            if(_buckets[index] == -1) {
                _entries.Add(new Entry(key, value));
                _buckets[index] = _entries.Count - 1;
                sw.Stop();
                return sw.ElapsedNanoSeconds();
            } else {
                _entries.Add(new Entry(key, value));
                var entry = _entries[_buckets[index]];
                index = _buckets[index];
                while(true) {
                    if(entry.Next == -1) {
                        break;
                    }
                    index = entry.Next;
                    entry = _entries[index];
                }
                entry.Next = _entries.Count - 1;
                _entries[index] = entry;
                sw.Stop();
                return sw.ElapsedNanoSeconds();
            }
        }
        public Tuple<bool,TValue,long> Search(string key) {
            var sw = new Stopwatch();
            sw.Start();
            var hashCode = HashCalculator.Calculate(key);
            var index = hashCode % _buckets.Length;
            if(_buckets[index] == -1) {
                sw.Stop();
                return new Tuple<bool, TValue, long>(false, default(TValue), sw.ElapsedNanoSeconds());
            }else if (_entries[_buckets[index]].Key == key) {
                sw.Stop();
                return new Tuple<bool, TValue, long>(true, _entries[_buckets[index]].Value, sw.ElapsedNanoSeconds());
            } else {
                index = _buckets[index];
                while((index = _entries[index].Next) != -1) {
                    if (_entries[index].Key == key) {
                        sw.Stop();
                        return new Tuple<bool, TValue, long>(true, _entries[index].Value, sw.ElapsedNanoSeconds());
                    }
                }
            }
            return new Tuple<bool, TValue, long>(false, default(TValue), sw.ElapsedNanoSeconds());
        }
    }
}