using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.CRUD
{
    class MudarStatusPedido
    {
        public MudarStatusPedido() {}

        public void alterarStatusPedido (Pedido pd, string name)
        {

            PessoaViewModel pessoaVm = new PessoaViewModel();

            int contador = 0;
            int posicao = -1;

            if (pd.StatusPedido == "ENVIADO" && name == "PAGO")
            {
                MessageBox.Show("Pedido enviado não pode voltar a ser pago", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (pd.StatusPedido == "RECEBIDO" && name == "PAGO")
            {
                MessageBox.Show("Pedido recebido não pode voltar a ser pago", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (pd.StatusPedido == "RECEBIDO" && name == "ENVIADO")
            {
                MessageBox.Show("Pedido recebido não pode voltar a ser enviado", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (pd.StatusPedido == "RECEBIDO" && name == "PAGO")
            {
                MessageBox.Show("Pedido recebido não pode voltar a ser pago", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                foreach (Pedido p in pessoaVm.TodosPedidos)
                {
                    if (pd.Id == p.Id)
                    {
                        pd.StatusPedido = name;
                        posicao = contador;
                    }
                    contador++;
                }
                pessoaVm.TodosPedidos[posicao] = pd;

                pessoaVm.Edicao = true;

                string jsonString = JsonSerializer.Serialize(pessoaVm.TodosPedidos, new JsonSerializerOptions() { WriteIndented = true });
                using (StreamWriter outputFile = new StreamWriter("pedido.json"))
                {
                    outputFile.WriteLine(jsonString);
                }
                pessoaVm.Clear();
                pessoaVm.PreparaPedidoCollection();
            }
        }
    }
}