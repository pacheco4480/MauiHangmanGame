using System.ComponentModel;
using System.Diagnostics;

namespace MauiHangmanGame
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        #region UI Properties
        public string Destaque
        {
            get => destaque;
            set
            {
                destaque = value;
                OnPropertyChanged();
            }
        }
        public List<char> Letras
        {
            get => letras;
            set
            {
                letras = value;
                OnPropertyChanged();
            }
        }

        public string Mensagem
        {
            get => mensagem;
            set
            {
                mensagem = value;
                OnPropertyChanged();
            }
        }
        public string StatusJogo
        {
            get => statusJogo;
            set
            {
                statusJogo = value;
                OnPropertyChanged();
            }
        }
        public string ImagemAtual
        {
            get => imagemAtual;
            set
            {
                imagemAtual = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Fields
        List<string> palavras = new List<string>()
     {
        "python",
        "javascript",
        "maui",
        "csharp",
        "mongodb",
        "sql",
        "xaml",
        "word",
        "excel",
        "powerpoint",
        "abacaxi",
        "windows",
        "ubatuba"
     };

        string resposta = "";
        private string destaque;
        List<char> palpite = new List<char>();
        private List<char> letras = new List<char>();
        private string mensagem;
        int erros = 0;
        int maximoErros = 6;
        private string statusJogo;
        private string imagemAtual = "img0.jpg";

        #endregion

        public MainPage()
        {
            InitializeComponent();
            Letras.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext = this;
            EscolherPalavra();
            CalcularPalavra(resposta, palpite);
        }

        #region Jogo
        private void EscolherPalavra()
        {
            resposta =
                 palavras[new Random().Next(0, palavras.Count)];
            Debug.WriteLine(resposta);
        }

        private void CalcularPalavra(string resposta, List<char> palpite)
        {
            var temp =
                 resposta.Select(x => (palpite.IndexOf(x) >= 0 ? x : '_'))
                 .ToArray();

            Destaque = string.Join(' ', temp);
        }

        private void TratarPalpite(char letra)
        {
            if (palpite.IndexOf(letra) == -1)
            {
                palpite.Add(letra);
            }
            if (resposta.IndexOf(letra) >= 0)
            {
                CalcularPalavra(resposta, palpite);
                VerificaSeGanhou();
            }
            else if (resposta.IndexOf(letra) == -1)//erro
            {
                erros++;
                AtualizaStatus();
                VerificaSePerdeu();
                ImagemAtual = $"img{erros}.jpg";
            }
        }

        private void VerificaSePerdeu()
        {
            if (erros == maximoErros)
            {
                Mensagem = "Você Perdeu!!";
                DesabilitarLetras();
            }
        }

        private void DesabilitarLetras()
        {
            foreach (var children in LetrasContainer.Children)
            {
                var btn = children as Button;
                if (btn != null)
                {
                    btn.IsEnabled = false;
                }
            }
        }
        private void HabilitarLetras()
        {
            foreach (var children in LetrasContainer.Children)
            {
                var btn = children as Button;
                if (btn != null)
                {
                    btn.IsEnabled = true;
                }
            }
        }

        private void VerificaSeGanhou()
        {
            if (Destaque.Replace(" ", "") == resposta)
            {
                Mensagem = "Você Ganhou!";
                DesabilitarLetras();
            }
        }

        private void AtualizaStatus()
        {
            StatusJogo = $"Erros: {erros} de {maximoErros}";
        }


        #endregion

        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var letra = btn.Text;
                btn.IsEnabled = false;
                TratarPalpite(letra[0]);
            }
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            erros = 0;
            palpite = new List<char>();
            ImagemAtual = "img0.jpg";
            EscolherPalavra();
            CalcularPalavra(resposta, palpite);
            Mensagem = "";
            AtualizaStatus();
            HabilitarLetras();
        }
    }

}