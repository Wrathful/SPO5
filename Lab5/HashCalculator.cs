using System;

namespace Lab5 {
    public static class HashCalculator {
        public static int Calculate(string key) {
            long res = 1;
            var mod = 1000000007;
            foreach (var sym in key) {
                short code = Convert.ToInt16(sym);
                res = (res % mod * code) % mod;
            }
            return (int)(res % mod);
        }
    }
}
