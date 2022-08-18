using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WpfApp3.MVVM.Model
{
    class Pessoa : INotifyPropertyChanged, ICloneable, BaseNotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }


        private long _id;
        private string _nome;
        private string _cpf;

        private string _endereco;

        public Pessoa() { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }


        [Key]
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [Required]
        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }

        [Required]
        public string Cpf
        {
            get { return _cpf; }
            set
            {
                _cpf = value;
                OnPropertyChanged("Cpf");
            }
        }

        [Required]
        public string Endereco
        {
            get { return _endereco; }
            set
            {
                _endereco = value;
                OnPropertyChanged("Endereco");
            }
        }
    }
}