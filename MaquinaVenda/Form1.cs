using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MaquinaVenda
{
    public partial class Form1 : Form
    {
        // Declara��o das vari�veis globais
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
            {"Caf�", 0.25m},
            {"Cappuccino", 0.30m},
            {"Chocolate", 0.35m},
            {"Ch�", 0.20m}
        };

        private string produtoSelecionado;

        // M�todo construtor da classe Form1
        public Form1()
        {
            InitializeComponent();
            // Exibe a mensagem de boas-vindas
            MostrarMensagem("Bem-vindo! Selecione um produto.");
        }

        // Eventos dos cliques nas PictureBoxes dos produtos
        private void pictureBoxCafe_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "Caf�";
            // Exibe informa��es do produto selecionado
            MostrarMensagem($"Produto selecionado: {produtoSelecionado}. Pre�o: {precosProdutos[produtoSelecionado]:C}. Saldo: {saldo:C}");
        }

        private void pictureBoxCappuccino_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "Cappuccino";
            MostrarMensagem($"Produto selecionado: {produtoSelecionado}. Pre�o: {precosProdutos[produtoSelecionado]:C}. Saldo: {saldo:C}");
        }

        private void pictureBoxChocolate_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "Chocolate";
            MostrarMensagem($"Produto selecionado: {produtoSelecionado}. Pre�o: {precosProdutos[produtoSelecionado]:C}. Saldo: {saldo:C}");
        }

        private void pictureBoxCha_Click(object sender, EventArgs e)
        {
            produtoSelecionado = "Ch�";
            MostrarMensagem($"Produto selecionado: {produtoSelecionado}. Pre�o: {precosProdutos[produtoSelecionado]:C}. Saldo: {saldo:C}");
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

        // Evento do clique no bot�o "Comprar"
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

        // M�todo para adicionar moeda ao saldo
        private void AdicionarMoeda(decimal valor)
        {
            saldo += valor;
            // Atualiza a exibi��o do saldo
            AtualizarSaldo();
        }

        // M�todo para realizar a compra do produto
        private void ComprarProduto(string produto)
        {
            if (saldo >= precosProdutos[produto])
            {
                // Deduz o pre�o do produto do saldo
                saldo -= precosProdutos[produto];
                // Atualiza a exibi��o do saldo
                AtualizarSaldo();

                // Exibe mensagem de sucesso da compra
                MostrarMensagem($"Aqui est� o seu {produto}. \nObrigado pela compra!");

                decimal troco = saldo >= 0 ? saldo : 0;
                if (troco > 0)
                {
                    // Devolve o troco e atualiza o saldo
                    DevolverTroco(troco);
                    saldo = 0; // Zera o saldo ap�s a compra bem sucedida e o troco ser dado
                }
            }
            else
            {
                // Exibe mensagem se o saldo for insuficiente
                MostrarMensagem("Saldo insuficiente.");
            }
        }

        // M�todo para devolver o troco
        private void DevolverTroco(decimal troco)
        {
            // Converte as chaves do dicion�rio moedasDisponiveis numa lista e a ordena em ordem decrescente
            List<decimal> moedas = moedasDisponiveis.Keys.ToList();
            moedas.Sort((a, b) => b.CompareTo(a)); // Ordena as moedas em ordem decrescente

            // Dicion�rio para armazenar a quantidade de cada moeda no troco
            Dictionary<decimal, int> trocoEmMoedas = new Dictionary<decimal, int>();

            // Loop sobre cada moeda dispon�vel para calcular a quantidade de moedas no troco
            foreach (decimal moeda in moedas)
            {
                // Calcula a quantidade de moedas de determinado valor necess�rio para o troco
                int quantidade = (int)(troco / moeda);
                // Verifica se h� moedas dispon�veis suficientes para atender � quantidade calculada
                quantidade = Math.Min(quantidade, moedasDisponiveis[moeda]); // Verifica se h� moedas dispon�veis suficientes
                                                                             // Deduz o valor das moedas utilizadas do valor total do troco
                troco -= quantidade * moeda;

                // Adiciona a quantidade de moedas de determinado valor ao dicion�rio trocoEmMoedas
                if (quantidade > 0)
                {
                    trocoEmMoedas.Add(moeda, quantidade);
                    // Atualiza a quantidade de moedas dispon�veis no dicion�rio moedasDisponiveis
                    moedasDisponiveis[moeda] -= quantidade;
                }

                // Se o troco for totalmente coberto, encerra o loop
                if (troco == 0)
                    break;
            }


            // Exibe o troco ao utilizador
            string mensagemTroco = $"Aqui est� o seu {produtoSelecionado}. \n\tTroco: ";
            foreach (var par in trocoEmMoedas)
            {
                // Adiciona a quantidade de moedas de determinado valor e o seu respectivo tipo ao troco
                mensagemTroco += $"\n{par.Value} moeda(s) de {par.Key:C}, ";
            }
            // Remove a v�rgula e o espa�o em branco extra do final da string
            mensagemTroco = mensagemTroco.TrimEnd(',', ' ');

            // Adiciona mensagem de agradecimento ao troco
            mensagemTroco += "\nObrigado pela compra!";

            // Exibe mensagem de troco com a mensagem de compra
            MostrarMensagem(mensagemTroco);

        }

        // M�todo para exibir mensagens na label lblEcran
        private void MostrarMensagem(string mensagem)
        {
            lblEcran.Text = mensagem;
        }

        // M�todo para atualizar a exibi��o do saldo
        private void AtualizarSaldo()
        {
            lblEcran.Text = $"Saldo: {saldo:C}";
        }

    }
}
