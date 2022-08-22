using WpfApp3.core;

namespace WpfApp3.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand CadastroViewCommand { get; set; }
        public RelayCommand VenderViewCommand { get; set; }

        public RelayCommand CadastroPessoaViewCommand { get; set; }

        public RelayCommand NovoPedidoCommand { get; set; }

        public PedidoViewModel PedidoVm { get; set; }
        public ProdutoViewModel CadastroProdutoVm { get; set; }

        public PessoaViewModel CadastroPessoaVm { get; set; }

        public PedidoViewModel NovoPedido { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            CadastroProdutoVm = new ProdutoViewModel();
            PedidoVm = new PedidoViewModel();
            PedidoVm.PedidoSelecionado = null;
            CurrentView = PedidoVm;
            CadastroPessoaVm = new PessoaViewModel();
            NovoPedido = new PedidoViewModel();

            CadastroViewCommand = new RelayCommand(a =>
            {
                CurrentView = CadastroProdutoVm;
            });

            VenderViewCommand = new RelayCommand(o =>
            {
                CurrentView = PedidoVm;
            });

            CadastroPessoaViewCommand = new RelayCommand(a =>
            {
                CurrentView = CadastroPessoaVm;

                ((PessoaViewModel)CurrentView).CarregarTela();
            });

            NovoPedidoCommand = new RelayCommand(a =>
            {
                CurrentView = NovoPedido;
            });
        }
    }
}
