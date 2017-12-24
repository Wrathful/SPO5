using System.ComponentModel;
namespace Lab5 {
    public class DataForAdd {
        [DisplayName("Ключ")]
        public string Key { get; set; }
        [DisplayName("Значение")]
        public int Value { get; set; }
        [DisplayName("Время добавления (нс)")]
        public long Time { get; set; }
    }
}
