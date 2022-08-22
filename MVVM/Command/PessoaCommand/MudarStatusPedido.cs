using System.IO;
using System.Text.Json;
using System.Windows;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.CRUD
{
    public class MudarStatusPedido
    {
        public MudarStatusPedido() { }

        public void alterarStatusPedido(Pedido pd, string name)
        {

            PessoaViewModel pessoaVm = new PessoaViewModel();
            pessoaVm.CarregarTela();

            int contador = 0;
            int posicao = -1;

            if (pd.Status.Equals(Pedido.StatusEnum.Enviado.ToString()) && name == "PAGO")
            {
                MessageBox.Show("Pedido enviado não pode voltar a ser pago", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (pd.Status.Equals(Pedido.StatusEnum.Recebido.ToString()) && name == "PAGO")
            {
                MessageBox.Show("Pedido recebido não pode voltar a ser pago", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (pd.Status.Equals(Pedido.StatusEnum.Recebido.ToString()) && name == "ENVIADO")
            {
                MessageBox.Show("Pedido recebido não pode voltar a ser enviado", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (pd.Status.Equals(Pedido.StatusEnum.Recebido.ToString()) && name == "PAGO")
            {
                MessageBox.Show("Pedido recebido não pode voltar a ser pago", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                foreach (Pedido p in pessoaVm.TodosPedidos)
                {
                    if (pd.Id == p.Id)
                    {
                        switch (name)
                        {
                            case "PAGO":
                                pd.Status = Pedido.StatusEnum.Pago.ToString();
                                break;
                            case "ENVIADO":
                                pd.Status = Pedido.StatusEnum.Enviado.ToString();
                                break;
                            case "PENDENTE":
                                pd.Status = Pedido.StatusEnum.Pendente.ToString();
                                break;
                            case "RECEBIDO":
                                pd.Status = Pedido.StatusEnum.Recebido.ToString();
                                break;
                            default:
                                pd.Status = Pedido.StatusEnum.Pendente.ToString();
                                break;
                        }
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