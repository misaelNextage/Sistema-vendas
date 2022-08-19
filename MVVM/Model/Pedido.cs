using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WpfApp3.MVVM.Model
{
    public class Pedido : INotifyPropertyChanged, ICloneable, BaseNotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }


        private long _id;
        private Pessoa _pessoa;
        private Double _valorTotal;
        private DateTime _dataVenda;
        
        public List<ItemPedido> ItemsPedido { get; set; }
        public Pedido() { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public Pessoa Pessoa
        {
            get { return _pessoa; }
            set
            {
                _pessoa = value;
                OnPropertyChanged("Pessoa");
            }
        }

        public Double ValorTotal
        {
            get { return _valorTotal; }
            set
            {
                _valorTotal = value;
                OnPropertyChanged("ValorTotal");
            }
        }
        public DateTime DataVenda
        {
            get { return _dataVenda; }
            set
            {
                _dataVenda = value;
                OnPropertyChanged("DataVenda");
            }
        }
        public FormaPagamentoEnum FormaPagamento { get; set; }
        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Pendente,
            Pago,
            Enviado,
            Recebido

        }
        public enum FormaPagamentoEnum
        {
            Dinheiro,
            Cartao,
            Boleto
        }
    }
}