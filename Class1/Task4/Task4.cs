namespace Task4
{
    public class Task4
    {
/*
 * В решениях следующих заданий предполагается использование циклов.
 */

/*
 * Задание 4.1. Пользуясь циклом for, посимвольно напечатайте рамку размера width x height,
 * состоящую из символов frameChar. Можно считать, что width>=2, height>=2.
 * Например, вызов printFrame(5,3,'+') должен напечатать следующее:

+++++
+   +
+++++
 */
        internal static void PrintFrame(int width, int height, char frameChar = '*')
        {
            for (int i = 0; i < width; i++) Console.Write(frameChar);

            for (int i = 2; i < height; i++)
            {
                Console.Write($"\n{frameChar}");
                for (int j = 2; j < width; j++) Console.Write(' ');
                Console.Write(frameChar);
                Console.Write('\n');
            }

            for (int i = 0; i < width; i++) Console.Write(frameChar);

            Console.Write('\n');
        }

/*
 * Задание 4.2. Выполните предыдущее задание, пользуясь циклом while.
 */
        internal static void PrintFrame2(int width, int height, char frameChar = '*')
        {
            int i = 0;
            while (i < width)
            {
                Console.Write(frameChar);
                i++;
            }

            i = 2;
            while (i < height)
            {
                Console.Write($"\n{frameChar}");
                for (int j = 2; j < width; j++) Console.Write(' ');
                Console.Write(frameChar);
                Console.Write('\n');
                i++;
            }

            i = 0;
            while (i < width)
            {
                Console.Write(frameChar);
                i++;
            }

            Console.Write('\n');
        }


/*
 * Задание 4.3. Даны целые положительные числа A и B. Найти их наибольший общий делитель (НОД),
 * используя алгоритм Евклида:
 * НОД(A, B) = НОД(B, A mod B),    если B ≠ 0;        НОД(A, 0) = A,
 * где «mod» обозначает операцию взятия остатка от деления.
 */
        internal static long Gcd(long a, long b)
        {
            if (a == 0 || b == 0) return a + b;

            while (a != b)
            {
                if (a > b) a -= b;
                else b -= a;
            }

            return a;
        }

/*
 * Задание 4.4. Дано вещественное число X и целое число N (> 0). Найти значение выражения
 * 1 + X + X^2/(2!) + … + X^N/(N!), где N! = 1·2·…·N.
 * Полученное число является приближенным значением функции exp в точке X.
 */
        internal static double ExpTaylor(double x, int n)
        {
            double res = 1;

            for (int i = 1; i <= n; i++)
            {
                double p = Math.Pow(x, i);

                for (int j = 1; j <= i; j++) p /= j;

                res += p;
            }

            return res;
        }

        public static void Main(string[] args)
        {
            PrintFrame(5, 3, '+');
            PrintFrame2(5, 3, '+');
            Console.WriteLine(Gcd(12,0));
            Console.WriteLine(ExpTaylor(1.0, 10));

        }
    }
}