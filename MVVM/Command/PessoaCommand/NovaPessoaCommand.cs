using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using WpfApp3.core;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.CRUD
{
    class NovaPessoaCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return parameter is PessoaViewModel;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PessoaViewModel)parameter;
            var pessoa = new Model.Pessoa();
            long maxId = 0;
            if (viewModel.Pessoas.Any())
            {
                maxId = viewModel.Pessoas.Max(f => f.Id);
            }
            if (viewModel.Edicao == false)
            {
                pessoa.Id = maxId + 1;
                pessoa.Nome = viewModel.PessoaEdit.Nome;
                pessoa.Cpf = viewModel.PessoaEdit.Cpf != null ? System.Text.RegularExpressions.Regex.Replace(viewModel.PessoaEdit.Cpf, "[^0-9]", "") : viewModel.PessoaEdit.Cpf;
                pessoa.Endereco = viewModel.PessoaEdit.Endereco;


                if (pessoa.Nome == null || pessoa.Nome == "")
                {

                    MessageBox.Show("Campo Nome obrigatório!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                }
                else if (pessoa.Cpf == null || pessoa.Cpf == "")
                {

                    MessageBox.Show("Campo CPF obrigatório!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;


                }
                else if (!viewModel.PessoaEdit.Cpf.All(char.IsDigit) && !pessoa.Cpf.All(char.IsDigit))
                {
                    MessageBox.Show("CPF precisa ser numérico!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;

                }
                else if (!Util.Validacao.IsCpf(viewModel.PessoaEdit.Cpf.Replace(".", "").Replace("-", "")))
                {

                    MessageBox.Show("CPF invalido!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;

                }
                else if (pessoa.Cpf.Length < 11)
                    MessageBox.Show("Erro ao salvar o CPF, pois o campo CPF precisa conter 11 Digitos númericos.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    viewModel.Pessoas.Add(pessoa);
                    viewModel.PessoasSelecionado = pessoa;
                    viewModel.PessoaEdit.Id = 0;
                    viewModel.PessoaEdit.Nome = "";
                    viewModel.PessoaEdit.Endereco = "";
                    viewModel.PessoaEdit.Cpf = "";
                }

                string jsonString = JsonSerializer.Serialize(viewModel.Pessoas, new JsonSerializerOptions() { WriteIndented = true });
                using (StreamWriter outputFile = new StreamWriter("pessoa.json"))
                {
                    outputFile.WriteLine(jsonString);
                }
            }

            else
            {
                string jsonString = JsonSerializer.Serialize(viewModel.Pessoas, new JsonSerializerOptions() { WriteIndented = true });
                using (StreamWriter outputFile = new StreamWriter("pessoa.json"))
                {
                    outputFile.WriteLine(jsonString);
                }
                //Limpa os campos e possibilita uma nova inclusão
                viewModel.Edicao = false;
                viewModel.Pessoas.Clear();
                viewModel.PreparaPessoaCollection();
                viewModel.PessoasSelecionado = pessoa;
                viewModel.PessoaEdit.Id = 0;
                viewModel.PessoaEdit.Nome = "";
                viewModel.PessoaEdit.Endereco = "";
                viewModel.PessoaEdit.Cpf = "";
                viewModel.Pessoas.Clear();
                viewModel.PreparaPessoaCollection();
            }
        }

    }
}