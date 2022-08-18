using System;
using System.ComponentModel;

namespace WpfApp3.MVVM.Model
{
    class Pedido : INotifyPropertyChanged, ICloneable, BaseNotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }


        private long _id;
        private Pessoa _pessoa;
        private Double _valorTotal;
        private String _dataVenda;
        private String _statusPedido;
        private String _formaPagamento;
        public enum _status
        {
            PENDENTE,
            PAGO,
            ENVIADO,
            RECEBIDO
        }


        public enum FormaPagamento
        {
            Dinheiro,
            Cartao,
            Boleto
        }

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
        public String DataVenda
        {
            get { return _dataVenda; }
            set
            {
                _dataVenda = value;
                OnPropertyChanged("DataVenda");
            }
        }
        public String StatusPedido
        {
            get { return _statusPedido;}
            set
            {
                _statusPedido = value;
                OnPropertyChanged("StatusPedido");
            }
            
        }

        public String FormaPagamentoPedido
        {
            get { return _formaPagamento; }
            set
            {
                _formaPagamento = value;
                OnPropertyChanged("FormaPagamento");
            }
        }
    }
}