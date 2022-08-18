using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;
using System.Windows.Controls;
using WpfApp3.MVVM.View;

namespace WpfApp3.MVVM.CRUD
{
    class IncluirPedidoCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as CadastroPessoaViewModel;
            return viewModel != null;
        }

        public override void Execute(object parameter)
        {
            try
            {
                new MainWindow().ShowDialog(); //faz a abertura da janela de pedido
                System.Windows.Application.Current.Shutdown(); //encerra a aplicação
            }
            catch (Exception e)
            {
                Console.WriteLine("Finalizando");
            }
            

        }
    }
}
