using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Lab5 {
    public class RehashMethodBasedTable<TValue> where TValue : IEquatable<TValue> {
        public struct Entry {
            public string Key;
            public TValue Value;
            public Entry(string key, TValue value) {
                Key = key;
                Value = value;
            }
            public static bool operator ==(Entry current, Entry other) {
                return current.Key == other.Key && current.Value.Equals(other.Value);
            }
            public static bool operator !=(Entry current, Entry other) {
                return current.Key != other.Key || !current.Value.Equals(other.Value);
            }
        }
        private readonly static Random Rnd;
        static RehashMethodBasedTable() {
            Rnd = new Random();
        }
        public Entry[] _entries;
        private int[] _randomNumbersSequence { get; set; }
        public RehashMethodBasedTable(int capacity) {
            _entries = new Entry[capacity];
            _randomNumbersSequence = GetRandomNumbersSequence(capacity);
        }
        public long Add(string key, TValue value) {
            var sw = new Stopwatch();
            sw.Start();
            var hashCode = HashCalculator.Calculate(key);
            var index = hashCode % _entries.Length;
            foreach (var offset in _randomNumbersSequence) {
                var newIndex = (index + offset) % _entries.Length;
                if (_entries[newIndex] == default(Entry)) {
                    _entries[newIndex] = new Entry(key, value);
                    sw.Stop();
                    return sw.ElapsedNanoSeconds();
                }
            }
            throw new Exception("В таблице не осталось свободных ячеек");
        }

        public Tuple<bool, TValue, long> Search(string key) {
            var sw = new Stopwatch();
            sw.Start();
            var hashCode = HashCalculator.Calculate(key);
            var index = hashCode % _entries.Length;
            foreach (var ind in _randomNumbersSequence) {
                var newIndex = (index + ind) % _entries.Length;
                if (_entries[newIndex] == default(Entry)) {
                    sw.Stop();
                    return new Tuple<bool, TValue, long>(false, default(TValue), sw.ElapsedNanoSeconds());
                } else {
                    if (_entries[newIndex].Key == key) {
                        sw.Stop();
                        return new Tuple<bool, TValue, long>(true, _entries[newIndex].Value, sw.ElapsedNanoSeconds());
                    }
                }
            }
            sw.Stop();
            return new Tuple<bool, TValue, long>(false, default(TValue), sw.ElapsedNanoSeconds());
        }
        private int[] GetRandomNumbersSequence(int capacity) {
            var res = new HashSet<int>();
            var value = 0;
            res.Add(value);
            for (var i = 1; i < capacity; ++i) {
                do {
                    value = Rnd.Next(1, capacity);
                } while (res.Contains(value));
                res.Add(value);
            }
            return res.ToArray();
        }
    }
}