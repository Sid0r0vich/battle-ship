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
using System.Windows.Media.Media3D;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System.CodeDom;

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

        public Player()
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

        public List<Tuple<int, int>> GetComponents(Tuple<int, int> coords)
        {
            List<Tuple<int, int>> coordsList = new List<Tuple<int, int>>();
            List<Tuple<int, int>> CheckCoordsList = new List<Tuple<int, int>>();
            coordsList.Add(coords);
            CheckCoordsList.Add(coords);

            while (CheckCoordsList.Count > 0)
            {
                Tuple<int, int> CheckCoord = CheckCoordsList[0];
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                    {
                        if (CheckCoord.Item1 + i < 0 || CheckCoord.Item1 + i > 9 ||
                            CheckCoord.Item2 + j < 0 || CheckCoord.Item2 + j > 9)
                            continue;

                        if (i != 0 || j != 0)
                        {
                            if (Math.Abs(this.Pos[CheckCoord.Item1 + i, CheckCoord.Item2 + j]) == 1)
                            {
                                bool contains = false;
                                foreach (var coord in coordsList)
                                {
                                    if (coord.Item1 == CheckCoord.Item1 + i && coord.Item2 == CheckCoord.Item2 + j)
                                        contains = true;
                                }

                                if (!contains)
                                {
                                    var newCoords = new Tuple<int, int>(CheckCoord.Item1 + i, CheckCoord.Item2 + j);
                                    coordsList.Add(newCoords);
                                    CheckCoordsList.Add(newCoords);
                                }
                            }
                        }
                    }

                CheckCoordsList.RemoveAt(0);
            }

            return coordsList;
        }

        public bool IsAlive(List<Tuple<int, int>> coordsList)
        {
            foreach (Tuple<int ,int> coords in coordsList)
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                    {
                        if (coords.Item1 + i > 9 || coords.Item1 + i < 0 ||
                            coords.Item2 + j > 9 || coords.Item2 + j < 0)
                            continue;

                        if (i != 0 || j != 0)
                        {
                            if (this.Pos[coords.Item1 + i, coords.Item2 + j] == 1)
                            {
                                return true;
                            }
                        }
                    }

            return false;
        }

        public void SetPoints(List<Tuple<int, int>> coordsList)
        {
            foreach (Tuple<int, int> coords in coordsList)
            {
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i != 0 || j != 0)
                        {
                            if (coords.Item1 + i > 9 || coords.Item1 + i < 0 ||
                                coords.Item2 + j > 9 || coords.Item2 + j < 0)
                                continue;

                            ref int state1 = ref this.Pos[coords.Item1 + i, coords.Item2 + j];
                            if (state1 == 0)
                                state1 = 2;
                        }
                    }
            }
        }

        public void UpdateState(Tuple<int, int> coords, int state)
        {
            this.Pos[coords.Item1, coords.Item2] = state;
        }
    }

    public class Human : Player { }

    public class AI : Player
    {
        private static Random random = new Random();

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
        private List<Field[,]> Fields;

        private List<Player> Players;
        private Human human; public AI ai;

        private static MediaPlayer mediaPlayer = new MediaPlayer();
        private static string pathMessageVoices = "C:\\Users\\Overlord\\git\\battleShip\\battleShip\\music\\";
        private static Dictionary<Type, List<Uri>> tracks = new Dictionary<Type, List<Uri>>()
        {
            { Type.Human, new List<Uri>() {
                new Uri($"{pathMessageVoices}есть пробитие.mp3"),
                new Uri($"{pathMessageVoices}есть попадание.mp3"),
                new Uri($"{pathMessageVoices}пробитие.mp3"),
                new Uri($"{pathMessageVoices}готов.mp3"), }
            },

            { Type.Computer, new List<Uri>() {
                new Uri($"{pathMessageVoices}орудие повреждено.mp3"),
                new Uri($"{pathMessageVoices}повреждение боеукладки.mp3"),
                new Uri($"{pathMessageVoices}вращение башни невозможно.mp3"),
                new Uri($"{pathMessageVoices}танк уничтожен.mp3"), }
            },

        };

        private static Random random = new Random();

        private int MoveCount;

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
            RenderMap(this.Players[(int)Type.Human].Pos, Type.Human, showShips : true);
            RenderMap(this.Players[(int)Type.Computer].Pos, Type.Computer, showShips : false);
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

        private void RenderMap(int[,] Pos, Type type, bool showShips)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Pos[i, j] == 1 && showShips || Pos[i, j] == -1)
                    {
                        var margin = SetMargin(Pos, i, j, showShips : showShips);

                        Fields[(int)type][i, j].button.BorderThickness =
                            new Thickness(margin.left, margin.top, margin.right, margin.bottom);
                        Fields[(int)type][i, j].button.BorderBrush = Brushes.Blue;
                    }
                }
            }
        }

        public async void SelectField(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Tuple<int, int> coords = GetCoords(button);

            bool correct = MakeMove(coords, Type.Human);

            if (correct)
            {
                await Task.Delay(1200);
                this.GenerateMove(this.human.Pos, 100 - this.MoveCount);
                this.MoveCount++;
            }
        }

        private bool MakeMove(Tuple<int, int> coords, Type type)
        {
            int otherPlayer = (int)(type + 1) % 2;
            int currentPlayer = (int)type;
            int state = Players[otherPlayer].GetState(coords);

            if (state != 2 && state != -1)
                return false;

            this.Players[otherPlayer].UpdateState(coords, state);

            if (state == -1)
            {
                List<Tuple<int, int>> coordsList = this.Players[otherPlayer].GetComponents(coords);
                bool isAlive = this.Players[otherPlayer].IsAlive(coordsList);

                if (!isAlive)
                    this.Players[otherPlayer].SetPoints(coordsList);

                MainWindow.ChooseTrack(isAlive, type);
            }

            SetImages((Type)otherPlayer);
            bool showShips = type == Type.Human ? false : true;
            RenderMap(this.Players[otherPlayer].Pos, (Type)otherPlayer, showShips: showShips);

            return true;
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

        private static void ChooseTrack(bool isAlive, Type type)
        {
            int numTrack;
            if (isAlive)
                numTrack = MainWindow.random.Next(3);
            else
                numTrack = 3;

            PlayMusic(numTrack, type);
        }

        private static void PlayMusic(int numTreck, Type type)
        {
            MainWindow.mediaPlayer.Open(MainWindow.tracks[type][numTreck]);
            MainWindow.mediaPlayer.Play();
        }

        private static (int left, int top, int right, int bottom) SetMargin(int[,] Pos, int row, int column, bool showShips)
        {
            (int left, int top, int right, int bottom) margin = (2, 2, 2, 2);

            if (column > 0 && ShowShips(Pos[row, column - 1], showShips) == -1) margin.left = 0;
            if (row > 0 && ShowShips(Pos[row - 1, column], showShips) == -1) margin.top = 0;
            if (column < 9 && ShowShips(Pos[row, column + 1], showShips) == -1) margin.right = 0;
            if (row < 9 && ShowShips(Pos[row + 1, column], showShips) == -1) margin.bottom = 0;

            return margin;
        }

        private static int ShowShips(int state, bool showShips)
        {
            if (showShips) return -Math.Abs(state);
            else return state;
        }

        private void SetImages(Type type)
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    this.Fields[(int)type][i, j].SetImage(this.Players[(int)type].Pos[i,j]);
                }
        }
    }
}
