using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace battleShip
{
    public enum Turn
    {
        First,
        Second,
    }

    public class Field
    {
        private Image image = new Image();
        public Button button = new Button();
        private MainWindow owner;

        public static string path = "C:\\Users\\Overlord\\git\\battleShip\\battleShip\\images\\";
        public static BitmapImage cross = new BitmapImage(new Uri($"{path}cross.png"));
        public static BitmapImage point = new BitmapImage(new Uri($"{path}point.png"));

        public Field(MainWindow owner, int row, int column, Grid grid)
        {
            this.owner = owner;
            this.button.Click += this.owner.Select_Field;
            this.button.Content = this.image;
            this.button.Background = new SolidColorBrush(Color.FromArgb(150, 169, 213, 255));

            grid.Children.Add(this.button);
            Grid.SetRow(this.button, row);
            Grid.SetColumn(this.button, column);
        }

        public void SetImage(int state)
        {
            if (state == -1)
                this.image.Source = Field.cross;
            else if (state == 2)
                this.image.Source = Field.point;
        }
    }

    public class Player
    {
        public int[,] Pos;

        public int GetState(Tuple<int, int> coords)
        {
            ref int square = ref this.Pos[coords.Item1, coords.Item2];
            int state;
            if (square == 1)
                state = -1;
            else if (square == 0)
                state = 2;
            else state = 3;

            return state;
        }
    }

    public class Human : Player
    {
        public int[,] OpponentPos = new int[10, 10];

        public Human()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    OpponentPos[i, j] = 0;

            this.Pos = new int[10, 10]
            {
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
                { 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            };
        }

        public void UpdateState(Tuple<int, int> coords, int state)
        {
            this.OpponentPos[coords.Item1, coords.Item2] = state;
        }
    }

    public class AI : Player
    {
        public AI()
        {
            this.Pos = new int[10, 10]
            {
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
                { 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            };
        }
    }

    public partial class MainWindow : Window
    {
        private Field[,] Fields = new Field[10, 10];
        private Field[,] OpponentFields = new Field[10, 10];
        public Human player;
        public AI ai;

        public MainWindow()
        {
            InitializeComponent();

            this.player = new Human();
            this.ai = new AI();

            CreateAllMap();
            RenderAllMap();
        }

        private void CreateAllMap()
        {
            CreateMap(Fields, this.board_grid_1);
            CreateMap(OpponentFields, this.board_grid_2);
        }

        private void RenderAllMap()
        {
            RenderMap(Fields, this.player.Pos);
            RenderMap(OpponentFields, this.player.OpponentPos);
        }

        private void CreateMap(Field[,] fields, Grid grid)
        {

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Field field = new Field(this, i, j, grid);
                    fields[i, j] = field;
                }
            }
        }

        private void RenderMap(Field[,] fields, int[,] Pos)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Pos[i, j] == 1 || Pos[i, j] == -1)
                    {
                        int left, right, top, bottom;
                        left = right = top = bottom = 2;

                        if (j > 0 && Math.Abs(Pos[i, j - 1]) == 1) left = 0;
                        if (i > 0 && Math.Abs(Pos[i - 1, j]) == 1) top = 0;
                        if (j < 9 && Math.Abs(Pos[i, j + 1]) == 1) right = 0;
                        if (i < 9 && Math.Abs(Pos[i + 1, j]) == 1) bottom = 0;

                        fields[i, j].button.BorderThickness = new Thickness(left, top, right, bottom);
                        fields[i, j].button.BorderBrush = Brushes.Blue;
                    }
                }
            }
        }

        public void Select_Field(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Tuple<int, int> coords = GetCoords(button);
            int state = ai.GetState(coords);

            if (state == 2 || state == -1)
            {
                OpponentFields[coords.Item1, coords.Item2].SetImage(state);
                player.UpdateState(coords, state);
                RenderMap(OpponentFields, this.player.OpponentPos);
            }
        }

        private Tuple<int, int> GetCoords(Button field)
        {
            int row = Grid.GetRow(field);
            int column = Grid.GetColumn(field);

            return new Tuple<int, int>(row, column);
        }
    }
}
