using System;
using System.ComponentModel;

namespace WpfApp3.MVVM.Model
{
    public class ItemPedido : INotifyPropertyChanged, ICloneable, BaseNotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int Quantidade { get; set; }

        public double Valor { get; set; }

        public Produto Produto { get; set; }

        public ItemPedido() { }
    }
}
