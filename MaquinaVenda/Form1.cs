using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MaquinaVenda
{
    public partial class Form1 : Form
    {
        // Declaração das variáveis globais
        private decimal saldo = 0.0m;
        private Dictionary<decimal, int> moedasDisponiveis = new Dictionary<decimal, int>
        {
            {0.05m, 15},
            {0.10m, 15},
            {0.20m, 15},
            {0.50m, 15},
            {1.0m, 15},
            {2.0m, 15}
        };

        private Dictionary<string, decimal> precosProdutos = new Dictionary<string, decimal>
        {
            {"Café", 0.25m},
            {"Cappuccino", 0.30m},
            {"Chocolate", 0.35m},
            {"Chá", 0.20m}
        };

        private string produtoSelecionado;

        // Método construtor da classe Form1
        public Form1()
        {
            InitializeComponent();
            // Exibe a mensagem de boas-vindas
            MostrarMensagem("Bem-vindo! Selecione um produto.");
        }

        // Eventos dos cliques nas PictureBoxes dos produtos
        private void pictureBoxCafe_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "Café";
            // Exibe informações do produto selecionado
            MostrarMensagem($"Produto selecionado: {produtoSelecionado}. Preço: {precosProdutos[produtoSelecionado]:C}. Saldo: {saldo:C}");
        }

        private void pictureBoxCappuccino_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "Cappuccino";
            MostrarMensagem($"Produto selecionado: {produtoSelecionado}. Preço: {precosProdutos[produtoSelecionado]:C}. Saldo: {saldo:C}");
        }

        private void pictureBoxChocolate_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "Chocolate";
            MostrarMensagem($"Produto selecionado: {produtoSelecionado}. Preço: {precosProdutos[produtoSelecionado]:C}. Saldo: {saldo:C}");
        }

        private void pictureBoxCha_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "Chá";
            MostrarMensagem($"Produto selecionado: {produtoSelecionado}. Preço: {precosProdutos[produtoSelecionado]:C}. Saldo: {saldo:C}");
        }

        // Eventos dos cliques nas PictureBoxes das moedas
        private void pictureBoxMoeda005_Click(object sender, EventArgs e)
        {
            AdicionarMoeda(0.05m);
        }

        private void pictureBoxMoeda010_Click(object sender, EventArgs e)
        {
            AdicionarMoeda(0.10m);
        }

        private void pictureBoxMoeda020_Click(object sender, EventArgs e)
        {
            AdicionarMoeda(0.20m);
        }

        private void pictureBoxMoeda050_Click(object sender, EventArgs e)
        {
            AdicionarMoeda(0.50m);
        }

        private void pictureBoxMoeda100_Click(object sender, EventArgs e)
        {
            AdicionarMoeda(1.00m);
        }

        private void pictureBoxMoeda200_Click(object sender, EventArgs e)
        {
            AdicionarMoeda(2.00m);
        }

        // Evento do clique no botão "Comprar"
        private void btnComprar_Click_1(object sender, EventArgs e)
        {
            if (produtoSelecionado != null)
            {
                ComprarProduto(produtoSelecionado);
            }
            else
            {
                // Exibe mensagem se nenhum produto foi selecionado
                MostrarMensagem("Selecione um produto primeiro.");
            }
        }

        // Método para adicionar moeda ao saldo
        private void AdicionarMoeda(decimal valor)
        {
            saldo += valor;
            // Atualiza a exibição do saldo
            AtualizarSaldo();
        }

        // Método para realizar a compra do produto
        private void ComprarProduto(string produto)
        {
            if (saldo >= precosProdutos[produto])
            {
                // Deduz o preço do produto do saldo
                saldo -= precosProdutos[produto];
                // Atualiza a exibição do saldo
                AtualizarSaldo();

                // Exibe mensagem de sucesso da compra
                MostrarMensagem($"Aqui está o seu {produto}. \nObrigado pela compra!");

                decimal troco = saldo >= 0 ? saldo : 0;
                if (troco > 0)
                {
                    // Devolve o troco e atualiza o saldo
                    DevolverTroco(troco);
                    saldo = 0; // Zera o saldo após a compra bem sucedida e o troco ser dado
                }
            }
            else
            {
                // Exibe mensagem se o saldo for insuficiente
                MostrarMensagem("Saldo insuficiente.");
            }
        }

        // Método para devolver o troco
        private void DevolverTroco(decimal troco)
        {
            // Converte as chaves do dicionário moedasDisponiveis numa lista e a ordena em ordem decrescente
            List<decimal> moedas = moedasDisponiveis.Keys.ToList();
            moedas.Sort((a, b) => b.CompareTo(a)); // Ordena as moedas em ordem decrescente

            // Dicionário para armazenar a quantidade de cada moeda no troco
            Dictionary<decimal, int> trocoEmMoedas = new Dictionary<decimal, int>();

            // Loop sobre cada moeda disponível para calcular a quantidade de moedas no troco
            foreach (decimal moeda in moedas)
            {
                // Calcula a quantidade de moedas de determinado valor necessário para o troco
                int quantidade = (int)(troco / moeda);
                // Verifica se há moedas disponíveis suficientes para atender à quantidade calculada
                quantidade = Math.Min(quantidade, moedasDisponiveis[moeda]); // Verifica se há moedas disponíveis suficientes
                                                                             // Deduz o valor das moedas utilizadas do valor total do troco
                troco -= quantidade * moeda;

                // Adiciona a quantidade de moedas de determinado valor ao dicionário trocoEmMoedas
                if (quantidade > 0)
                {
                    trocoEmMoedas.Add(moeda, quantidade);
                    // Atualiza a quantidade de moedas disponíveis no dicionário moedasDisponiveis
                    moedasDisponiveis[moeda] -= quantidade;
                }

                // Se o troco for totalmente coberto, encerra o loop
                if (troco == 0)
                    break;
            }


            // Exibe o troco ao utilizador
            string mensagemTroco = $"Aqui está o seu {produtoSelecionado}. \n\tTroco: ";
            foreach (var par in trocoEmMoedas)
            {
                // Adiciona a quantidade de moedas de determinado valor e o seu respectivo tipo ao troco
                mensagemTroco += $"\n{par.Value} moeda(s) de {par.Key:C}, ";
            }
            // Remove a vírgula e o espaço em branco extra do final da string
            mensagemTroco = mensagemTroco.TrimEnd(',', ' ');

            // Adiciona mensagem de agradecimento ao troco
            mensagemTroco += "\nObrigado pela compra!";

            // Exibe mensagem de troco com a mensagem de compra
            MostrarMensagem(mensagemTroco);

        }

        // Método para exibir mensagens na label lblEcran
        private void MostrarMensagem(string mensagem)
        {
            lblEcran.Text = mensagem;
        }

        // Método para atualizar a exibição do saldo
        private void AtualizarSaldo()
        {
            lblEcran.Text = $"Saldo: {saldo:C}";
        }

    }
}
