using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WpfApp3.MVVM.Model
{
    public class ItemPedido : INotifyPropertyChanged, ICloneable, BaseNotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        // Esta rotina é chamada cada vez que o valor da propridade 
        // for definida. Isso vai disparar um evento para notificar 
        // a WPF via data binding que algo mudou
        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public Produto Produto { get; set; }

        public ItemPedido() { }
    }
}
