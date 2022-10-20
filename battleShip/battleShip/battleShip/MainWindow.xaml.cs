using System;
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

using System.Collections.Generic;
using System.Threading;
using System.Media;
using System.Windows.Controls.Primitives;


namespace battleShip
{
    public enum Turn
    {
        First,
        Second,
    }

    public enum Type
    {
        Human,
        Computer,
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

        public void AddClick()
        {
            this.button.Click += this.owner.SelectField;
        }
    }

    public class Player
    {
        public int[,] Pos;
        public int[,] OpponentPos;

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
        public Human()
        {
            this.OpponentPos = new int[10, 10];

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
        private static Random random = new Random();

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

        public static Tuple<int, int>? GenerateMove(int[,] Pos, int count)
        {
            int number = AI.random.Next(count);
            count = 0;

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    if (Pos[i, j] == 0 || Pos[i, j] == 1)
                    {
                        if (count == number)
                            return new Tuple<int, int>(i, j);

                        count++;
                    }
                }

            return null;
        }
    }

    public partial class MainWindow : Window
    {
        private List<Player> Players;
        private List<Field[,]> Fields;

        private Human human; public AI ai;

        private static MediaPlayer mediaPlayer = new MediaPlayer();

        private int MoveCount;

        private static string path = "C:\\Users\\Overlord\\git\\battleShip\\battleShip\\music\\";

        private static List<Uri> trecks = new List<Uri>()
        {
            new Uri($"{path}есть пробитие.mp3"),
            new Uri($"{path}есть попадание.mp3"),
            new Uri($"{path}готов.mp3"),
            new Uri($"{path}орудие повреждено.mp3"),
            new Uri($"{path}повреждение боеукладки.mp3"),
            new Uri($"{path}вращение башни невозможно.mp3"),
        };


        public MainWindow()
        {
            InitializeComponent();

            human = new Human();
            ai = new AI();

            Players = new List<Player>() { human, ai };

            Fields = new List<Field[,]>();
            Fields.Add(new Field[10, 10]);
            Fields.Add(new Field[10, 10]);

            CreateAllMap();
            RenderAllMap();

            this.MoveCount = 0;
        }

        private void CreateAllMap()
        {
            CreateMap(this.board_grid_1, addClick : false, Type.Human);
            CreateMap(this.board_grid_2, addClick : true, Type.Computer);
        }

        private void RenderAllMap()
        {
            RenderMap(this.Players[(int)Type.Human].Pos, Type.Human);
            RenderMap(this.Players[(int)Type.Human].OpponentPos, Type.Computer);
        }

        private void CreateMap(Grid grid, bool addClick, Type type)
        {

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Field field = new Field(this, i, j, grid);
                    if (addClick) field.AddClick();
                    this.Fields[(int)type][i, j] = field;
                }
            }
        }

        private void RenderMap(int[,] Pos, Type type)
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

                        Fields[(int)type][i, j].button.BorderThickness = new Thickness(left, top, right, bottom);
                        Fields[(int)type][i, j].button.BorderBrush = Brushes.Blue;
                    }
                }
            }
        }

        public async void SelectField(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Tuple<int, int> coords = GetCoords(button);

            MakeMove(coords, Type.Human);

            await Task.Delay(800);
            this.GenerateMove(this.human.Pos, 100 - this.MoveCount);
            this.MoveCount++;
        }

        private void MakeMove(Tuple<int, int> coords, Type type)
        {
            int state = Players[(int)(type + 1) % 2].GetState(coords);

            if (state == 2 || state == -1)
            {
                this.Fields[(int)(type + 1) % 2][coords.Item1, coords.Item2].SetImage(state);

                if (type == Type.Human)
                {
                    this.human.UpdateState(coords, state);
                    RenderMap(Players[(int)type].OpponentPos, Type.Computer);
                }
                else
                    RenderMap(Players[(int)type].Pos, Type.Human);
            }

            if (state == -1)
                MainWindow.PlayMusic(new Random().Next(3) + (int)type * 3);
        }

        private void GenerateMove(int[,] Pos, int count)
        {
            Tuple<int, int>? coords = AI.GenerateMove(Pos, count);

            if (coords != null)
            {
                MakeMove(coords, Type.Computer);
            }
        }

        private Tuple<int, int> GetCoords(Button field)
        {
            int row = Grid.GetRow(field);
            int column = Grid.GetColumn(field);

            return new Tuple<int, int>(row, column);
        }

        private static void PlayMusic(int treck)
        {
            MainWindow.mediaPlayer.Open(trecks[treck]);
            MainWindow.mediaPlayer.Play();
        }
    }
}
