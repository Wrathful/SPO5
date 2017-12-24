using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Lab5 {
    public partial class Form1 : Form {
        private static Random Rnd;
        private RehashMethodBasedTable<int> _rehashBasedTable;
        private ChainMethod<int> _ChainMethod;
        private int _capacity;
        private List<DataForAdd> _rehashMethodAddingData;
        private List<DataForAdd> _chainMethodAddingData;
        private List<DataForSearch> _rehashMethodSearchingData;
        private List<DataForSearch> _chainMethodSearchingData;
        private char[] _alphabet = new []{'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                                         'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                                         's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
        private KeyValuePair<string,int>[] _testElements;
        private string[] _keysForSearch;
        static Form1() {
            Rnd = new Random();
        }

        public Form1() {
            InitializeComponent();
            _rehashMethodAddingData = new List<DataForAdd>();
            _chainMethodAddingData = new List<DataForAdd>();
            _rehashMethodSearchingData = new List<DataForSearch>();
            _chainMethodSearchingData = new List<DataForSearch>();
            dataGridView1.DataSource = _rehashMethodAddingData;
            dataGridView2.DataSource = _chainMethodAddingData;
            dataGridView3.DataSource = _rehashMethodSearchingData;
            dataGridView4.DataSource = _chainMethodSearchingData;
        }

        private KeyValuePair<string, int>[] GetRandomElements(int capacity) {
            var uniqueSet = new HashSet<string>();
            KeyValuePair<string, int>[] res = new KeyValuePair<string,int>[capacity];
            for(var i = 0; i < capacity; ++i) {
                var keyLength = Rnd.Next(1, 40);
                var key = string.Empty;
                do {
                    key = GenerateKey();
                } while (uniqueSet.Contains(key));
                uniqueSet.Add(key);
                var value = Rnd.Next(0, 10000000);
                res[i] = new KeyValuePair<string, int>(key, value);
            }
            return res;
        }
        
        private string GenerateKey() {
            var keyLength = Rnd.Next(1, 40);
            var key = string.Empty;
            for (var j = 0; j < keyLength; ++j) {
                key += _alphabet[Rnd.Next(_alphabet.Length)];
            }
            return key;
        }

        private string[] GetKeysForSearch(int capacity, KeyValuePair<string, int>[] elements) {
            var res = new string[capacity];
            var containedElements = (int)(elements.Length * 0.7);
            for(var i = 0; i < containedElements; ++i) {
                res[i] = elements[i].Key;
            }
            for(var i = containedElements; i < capacity; ++i) {
                res[i] = GenerateKey();
            }
            return res;
        }

        private async void TestAddBtn_Click(object sender, EventArgs e) {
            testSearchBtn.Enabled = false;
            _capacity = (int)numericUpDown1.Value;
            _rehashBasedTable = new RehashMethodBasedTable<int>(_capacity);
            _ChainMethod = new ChainMethod<int>(_capacity);
            _testElements = GetRandomElements(_capacity);
            _keysForSearch = GetKeysForSearch(_capacity, _testElements);
            label1.Text = "Рехеширование. Добавление";
            label2.Text = "Метод цепочек. Добавление";
            label3.Text = "Рехеширование. Поиск";
            label4.Text = "Метод цепочек. Поиск";
            await Task.Run(() => {
                ClearDataSources();
                Invoke((MethodInvoker)(() => UpdateDataGrids()));
                foreach (var elem in _testElements) {
                    var rehashAddTime = _rehashBasedTable.Add(elem.Key, elem.Value);
                    var chainMethodAddTime = _ChainMethod.Add(elem.Key, elem.Value);
                    var rehashAddData = new DataForAdd {
                        Key = elem.Key,
                        Value = elem.Value,
                        Time = rehashAddTime
                    };
                    var chainMethodAddData = new DataForAdd {
                        Key = elem.Key,
                        Value = elem.Value,
                        Time = chainMethodAddTime
                    };
                    _rehashMethodAddingData.Add(rehashAddData);
                    _chainMethodAddingData.Add(chainMethodAddData);
                }
                var rehashTotalTime = _rehashMethodAddingData.Select(x => x.Time).Sum() / 1e9;
                var chainMethodTotalTime = _chainMethodAddingData.Select(x => x.Time).Sum() / 1e9;
                Invoke((MethodInvoker)(() => label1.Text = $"Рехеширование. Добавление - {rehashTotalTime} c."));
                Invoke((MethodInvoker)(() => label2.Text = $"Метод цепочек. Добавление - {chainMethodTotalTime} c."));
            });
            UpdateDataGrids();
            testSearchBtn.Enabled = true;
        }

        private async void TestSearchBtn_Click(object sender, EventArgs e) {
            await Task.Run(() => {
                foreach(var key in _keysForSearch) {
                    var rehashSearchRes = _rehashBasedTable.Search(key);
                    var chainMethodSearchRes = _ChainMethod.Search(key);
                    var rehashSearchData = new DataForSearch {
                        Key = key,
                        Contais = rehashSearchRes.Item1,
                        Value = rehashSearchRes.Item1 ? (int?)rehashSearchRes.Item2 : null,
                        Time = rehashSearchRes.Item3
                    };
                    var chainMethodSearchData = new DataForSearch {
                        Key = key,
                        Contais = chainMethodSearchRes.Item1,
                        Value = chainMethodSearchRes.Item1 ? (int?)chainMethodSearchRes.Item2 : null,
                        Time = chainMethodSearchRes.Item3
                    };
                    _rehashMethodSearchingData.Add(rehashSearchData);
                    _chainMethodSearchingData.Add(chainMethodSearchData);
                }
                var rehashTotalTime = _rehashMethodSearchingData.Select(x => x.Time).Sum() / 1e9;
                var chainMethodTotalTime = _chainMethodSearchingData.Select(x => x.Time).Sum() / 1e9;
                Invoke((MethodInvoker)(() => label3.Text = $"Рехеширование. Поиск - {rehashTotalTime} c."));
                Invoke((MethodInvoker)(() => label4.Text = $"Метод цепочек. Поиск - {chainMethodTotalTime} c."));
            });
            UpdateDataGrids();
            testSearchBtn.Enabled = false;
        }

        private void ClearDataSources() {
            _rehashMethodAddingData.Clear();
            _rehashMethodSearchingData.Clear();
            _chainMethodAddingData.Clear();
            _chainMethodSearchingData.Clear();
        }

        private void RefrashGrid(DataGridView dgv) {
            (dgv.BindingContext[dgv.DataSource] as CurrencyManager).Refresh();
        }

        private void UpdateDataGrids() {
            RefrashGrid(dataGridView1);
            RefrashGrid(dataGridView2);
            RefrashGrid(dataGridView3);
            RefrashGrid(dataGridView4);
        }
    }
}