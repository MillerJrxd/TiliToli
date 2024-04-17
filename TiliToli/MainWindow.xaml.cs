using Microsoft.VisualBasic;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TiliToli
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd;
        Grid grid;
        Button[,] tomb;
        public MainWindow()
        {
            InitializeComponent();
            Button keveres = new Button();
            Kirajzol(3);
            Keveres();
            keveres.Width = 90;
            keveres.Height = 40;
            keveres.Content = "Keverés";
            keveres.Name = "btnKeveres";
            keveres.FontSize = 18;
            keveres.Click += KeveresClick;
            keveres.Margin = new Thickness(0, 0, 0, 5);
            container.Children.Add(keveres);
            ;

        }

        private void KeveresClick(object sender, RoutedEventArgs e)
        {
            Keveres();
        }

        private void Kirajzol(int param)
        {
            grid = new Grid();
            double cellCount = Math.Pow(param, 2);

            container.Children.Add(grid);

            for (int i = 0; i <= param; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int x = 0; x <= param; x++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            tomb = new Button[param, param];
            int seged = 1;
            for (int i = 0; i < cellCount; i++)
            {
                int id = grid.Children.Add(new Button());
                Button btn = grid.Children[id] as Button;
                
                btn.Content = $"{seged}";
                btn.SetValue(Grid.RowProperty, i / param);
                btn.SetValue(Grid.ColumnProperty, i % param);
                btn.SetValue(Button.FontSizeProperty, btn.FontSize = 22);
                btn.SetValue(Button.HeightProperty, btn.Height = 102);
                btn.SetValue(Button.WidthProperty, btn.Width = 102);
                btn.SetValue(Button.MarginProperty, btn.Margin = new Thickness(1,1,1,1));
                btn.Click += Mozog;
                tomb[i / param, i % param] = btn;
                seged++;
            }
            //grid.Children.RemoveAt(grid.Children.Count - 1);
            tomb[param - 1, param - 1].Visibility = Visibility.Hidden;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void Mozog(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var senderSora = Convert.ToInt32(btn.GetValue(Grid.RowProperty));
            var senderOszlopa = Convert.ToInt32(btn.GetValue(Grid.ColumnProperty));
            //var oszlopKord9 = -1;
            //var sorKord9 = -1;

            if(senderSora + 1 < tomb.GetLength(0) && tomb[senderSora + 1, senderOszlopa].Content.ToString() == "9")
            {
                Csere(senderSora, senderOszlopa, senderSora + 1, senderOszlopa);
            }
            if (senderSora - 1 >= 0 && tomb[senderSora -1, senderOszlopa].Content.ToString() == "9")
            {
                Csere(senderSora, senderOszlopa, senderSora -1, senderOszlopa);
            }
            if (senderOszlopa + 1 < tomb.GetLength(1) && tomb[senderSora, senderOszlopa + 1].Content.ToString() == "9")
            {
                Csere(senderSora, senderOszlopa, senderSora, senderOszlopa + 1);
            }
            if (senderOszlopa - 1 >= 0 && tomb[senderSora, senderOszlopa - 1].Content.ToString() == "9")
            {
                Csere(senderSora, senderOszlopa, senderSora, senderOszlopa - 1);
            }
            Ellenörzés();
        }

        private void Ellenörzés()
        {
            bool kesz = true;

            for (int i = 0; i < tomb.GetLength(0); i++)
            {
                for (int j = 0; j < tomb.GetLength(1); j++)
                {
                    if (tomb[i,j].Content.ToString() != $"{i * tomb.GetLength(0) + j +1}")
                    {
                        kesz = false;
                        break;
                    }
                }
            }
            if (kesz)
            {
                MessageBox.Show("Nyertél!");
            }
        }

        private void Keveres()
        {
            rnd = new Random();
            int cserekSzama;
            if (int.TryParse(Interaction.InputBox("Hányszor szeretné megkeverni?", "Keverés", "5"), out cserekSzama) && cserekSzama > 0)
            {
                for (int i = 0; i < cserekSzama; i++)
                {
                    int eredetiX = rnd.Next(0, tomb.GetLength(0));
                    int eredetiY = rnd.Next(0, tomb.GetLength(1));
                    int ujX = rnd.Next(0, tomb.GetLength(0));
                    int ujY = rnd.Next(0, tomb.GetLength(1));
                    while(ujX == eredetiX && ujY == eredetiY)
                    {
                        ujX = rnd.Next(0, tomb.GetLength(0));
                        ujY = rnd.Next(0, tomb.GetLength(1));
                    }
                    Csere(eredetiX, eredetiY, ujX, ujY);
                }
            }
            else
            {
                MessageBox.Show("Nem szám vagy nem megfelelő!");
            }
        }
        
        void Csere(int eredetiX, int eredetiY, int ujX, int ujY)
        {
            unsafe
            {
                Button csere = tomb[eredetiX, eredetiY];
                tomb[eredetiX, eredetiY] = tomb[ujX, ujY];
                tomb[ujX, ujY] = (Button)csere;
                Button btn = tomb[eredetiX, eredetiY];
                btn.SetValue(Grid.RowProperty, eredetiX);
                btn.SetValue(Grid.ColumnProperty, eredetiY);

                Button btn2 = tomb[ujX, ujY];
                btn2.SetValue(Grid.RowProperty, ujX);
                btn2.SetValue(Grid.ColumnProperty, ujY);
            }
        }
    }
}