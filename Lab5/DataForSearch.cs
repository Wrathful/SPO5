using System.ComponentModel;
namespace Lab5 {
    class DataForSearch {
        [DisplayName("Ключ")]
        public string Key { get; set; }
        [DisplayName("Найден?")]
        public bool Contais { get; set; }
        [DisplayName("Найденное значение")]
        public int? Value { get; set; }
        [DisplayName("Время поиска (нс)")]
        public long Time { get; set; }
    }
}
