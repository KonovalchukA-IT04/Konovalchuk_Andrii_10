using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //Створення чистого поля
                Console.Clear();                
                Field.ar = new string[,] 
                {
                    { " "," "," "," " },
                    { " "," "," "," " },
                    { " "," "," "," " },
                    { " "," "," "," " }
                };

                //Генерація перших двох елементів
                Rand.Rnd2();
                Rand.Rnd2();

                //Визов методу розширення вікна
                Width();

                //Вивід початкової інформації
                ConsoleKeyInfo keys;
                Console.SetCursorPosition(10, 10);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Нажмiть будь-яку кнопку для початку");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ReadKey();
                Console.Clear();

                Paint.Draw();

                //Управління
                while ((keys = Console.ReadKey(true)).Key != ConsoleKey.Escape)
                {
                    switch (keys.Key)
                    {
                        case ConsoleKey.W:                                                  //W-вверх
                            Console.Clear();    //стираємо косоль(для виводу нової інфи)
                            Movement.Copyar();  //свторюємо копію карти до перетворень
                            Movement.LiftUp();  //зміщення всіх елементів в один бік
                            Movement.ConcUp();  //зклеювання чисел
                            Rand.Rnd2();        //рандомізація числа на полі(2 або 4)
                            Paint.Draw();       //вивід карти з числами та кольорами в консоль
                            Paint.Rec();        //вивід рекорду
                            break;                                  //і далі т.д.
                        case ConsoleKey.A:                                                  //A-вліво
                            Console.Clear();
                            Movement.Copyar();
                            Movement.LiftLeft();
                            Movement.ConcLeft();
                            Rand.Rnd2();
                            Paint.Draw();
                            Paint.Rec();
                            break;
                        case ConsoleKey.S:                                                  //S-вниз
                            Console.Clear();
                            Movement.Copyar();
                            Movement.LiftDown();
                            Movement.ConcDown();
                            Rand.Rnd2();
                            Paint.Draw();
                            Paint.Rec();
                            break;
                        case ConsoleKey.D:                                                  //D-вправо
                            Console.Clear();
                            Movement.Copyar();
                            Movement.LiftRight();
                            Movement.ConcRight();
                            Rand.Rnd2();
                            Paint.Draw();
                            Paint.Rec();
                            break;
                    }
                    WinLose.Lose();     //перевірка на програш
                    WinLose.Win();      //перевірка на виграш
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
        }

        //Метод розширення вікна
        public static void Width()
        {
            Console.SetWindowSize(50, 25);
            Console.SetBufferSize(50, 25);
        }        
    }

    class Rand
    {
        //Рандомайзер заповнення карти числами
        public static void Rnd2()
        {
            //перевірка можливості руху: якщо не можна склеїти, то рух безглуздий--пропуск
            int kckount=0;
            for (int ii = 0; ii < Field.ar.GetLength(0); ii++)
                for (int jj = 0; jj < Field.ar.GetLength(1); jj++)
                    if (Field.ar[ii, jj] == Field.clonAr[ii, jj])
                        kckount++;
            if (kckount == 16)
                return;
            //сам генератор чисел на полі
            int i, j;
            int counti = 0;
            int[] vali = new int[0];
            Random rnd = new Random();
            bool flag = true, key = true, keyj = true;
            //рандомим індекс рядка(перший раз)
            i = rnd.Next(0, 4);
            while (flag)
            {
                int kounti = 0, kountj = 0;
                int count = 0;
                int[] val = new int[0];
                //виключаємо(записуємо) індекси підмасиву(j) в яких стоїть елемент
                while (true)
                {
                    for (j = 0; j < Field.ar.GetLength(1); j++)
                    {
                        if (Field.ar[i, j] != " ")
                        {
                            Array.Resize(ref val, val.Length + 1);
                            val[count] = j;
                            count++;
                        }
                    }
                    //якщо заповнений рядок, то ліваєм
                    if (val.Length == 4)
                        break;
                    //рандомим в пусті місце 
                    while (keyj)
                    {
                        j = rnd.Next(0, 4);
                        foreach (int n in val)
                        {
                            if (j == n)
                            {
                                kountj++;
                            }
                        }
                        //перевіряємо чи збігається рандомний індекс з виключенимим індексами, і рандомим заново, якщо так
                        if (kountj > 0)
                            kountj = 0;
                        //якщо попали то ліваєм--діло зроблено
                        else
                        {
                            Field.ar[i, j] = RanNum.Num();
                            flag = false;
                            keyj = false;
                        }

                    }
                    keyj = true;
                    break;
                }
                //ліваєм якщо діло зроблене
                if (flag == false)
                    break;
                //записуємо у виняток індекс (і), якщо рядок заповнений
                Array.Resize(ref vali, vali.Length + 1);
                vali[counti] = i;
                counti++;
                if (vali.Length == 4)
                    break;
                //рандомим індекс (і), якщо невдача генерації у першозгенерованому(аналогічно для(j)--з перевіркою винятків)
                while (key)
                {
                    i = rnd.Next(0, 4);
                    foreach (int h in vali)
                    {
                        if (i == h)
                        {
                            kounti++;
                        }
                    }
                    if (kounti > 0)
                        kounti = 0;
                    else
                        key = false;
                }
                key = true;
            }
        }
    }
    //Рандом(90%-"2";10%-"4")
    class RanNum
    {
        public static string Num()
        {
            string s;
            int l;
            string[] sa = { "2", "2", "2", "2", "2", "2", "2", "2", "2", "4" };
            Random rs = new Random();
            l = rs.Next(0, 10);
            s = sa[l];
            return s;
        }
    }

    //Переміщення і злиття
    class Movement
    {
        //створення копії карти до перетворень(для перевірки можливості руху: якщо не можна склеїти, то рух безглуздий--заборона генерації)
        public static void Copyar()
        {
            Field.clonAr = (string[,])Field.ar.Clone();
        }
        //переміщення вліво
        public static void LiftLeft()
        {
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                int j = 0;
                int c = 0;
                while (true)
                {             
                    //перевірка на пустоту: якщо так, то зміщуємо масив на одиницю
                    if (Field.ar[i, j] == " ")
                    {
                        for (int k = j; k < Field.ar.GetLength(1) - 1; k++)
                        {
                            Field.ar[i, k] = Field.ar[i, k + 1];
                        }
                        Field.ar[i, Field.ar.GetLength(1) - 1] = " ";
                        c++;
                    }
                    //якщо символ, то переходин на наступний індекс масиву
                    else if (Field.ar[i, j] != " ")
                    {
                        j++;
                    }
                    if (j == Field.ar.GetLength(1))
                    {
                        break;
                    }
                    if (c == Field.ar.GetLength(1))
                    {
                        break;
                    }
                }
            }
        }
        //зклеювання--вліво
        public static void ConcLeft()
        {
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                for (int j = 0; j < Field.ar.GetLength(1) - 1; j++)
                {
                    if (Field.ar[i, j] != " " && Field.ar[i, j] == Field.ar[i, j + 1])
                    {
                        Field.ar[i, j] = Convert.ToString(Convert.ToInt32(Field.ar[i, j]) + Convert.ToInt32(Field.ar[i, j + 1]));
                        Field.ar[i, j + 1] = " ";
                    }
                }
            }
            LiftLeft();
        }
        //переміщення вправо
        public static void LiftRight()
        {
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                int j = Field.ar.GetLength(1)-1;
                int c = 0;
                while (true)
                {
                    //перевірка на пустоту: якщо так, то зміщуємо масив на одиницю
                    if (Field.ar[i, j] == " ")
                    {
                        for (int k = j; k >=1 ; k--)
                        {
                            Field.ar[i, k] = Field.ar[i, k - 1];
                        }
                        Field.ar[i, 0] = " ";
                        c++;
                    }
                    //якщо символ, то переходин на наступний індекс масиву
                    else if (Field.ar[i, j] != " ")
                    {
                        j--;
                    }
                    if (j == 0)
                    {
                        break;
                    }
                    if (c == Field.ar.GetLength(1))
                    {
                        break;
                    }
                }
            }
        }
        //зклеювання--рух вправо
        public static void ConcRight()
        {
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                for (int j = Field.ar.GetLength(1) - 1; j >= 1; j--)
                {
                    if (Field.ar[i, j] != " " && Field.ar[i, j] == Field.ar[i, j - 1])
                    {
                        Field.ar[i, j] = Convert.ToString(Convert.ToInt32(Field.ar[i, j]) + Convert.ToInt32(Field.ar[i, j - 1]));
                        Field.ar[i, j - 1] = " ";
                    }
                }
            }
            LiftRight();
        }
        //переміщення вверх
        public static void LiftUp()
        {
            for (int j = 0; j < Field.ar.GetLength(1); j++)
            {
                int i = 0;
                int c = 0;
                while (true)
                {
                    //перевірка на пустоту: якщо так, то зміщуємо масив на одиницю
                    if (Field.ar[i, j] == " ")
                    {
                        for (int k = i; k < Field.ar.GetLength(0) - 1; k++)
                        {
                            Field.ar[k, j] = Field.ar[k+1, j];
                        }
                        Field.ar[Field.ar.GetLength(0) - 1, j] = " ";
                        c++;
                    }
                    //якщо символ, то переходин на наступний індекс масиву
                    else if (Field.ar[i, j] != " ")
                    {
                        i++;
                    }
                    if (i == Field.ar.GetLength(0))
                    {
                        break;
                    }
                    if (c == Field.ar.GetLength(0))
                    {
                        break;
                    }
                }
            }
        }
        //зклеювання--рух вверх
        public static void ConcUp()
        {
            for (int j = 0; j < Field.ar.GetLength(1); j++)
            {
                for (int i = 0; i < Field.ar.GetLength(0) - 1; i++)
                {
                    if (Field.ar[i, j] != " " && Field.ar[i, j] == Field.ar[i + 1, j])
                    {
                        Field.ar[i, j] = Convert.ToString(Convert.ToInt32(Field.ar[i, j]) + Convert.ToInt32(Field.ar[i + 1, j]));
                        Field.ar[i + 1, j] = " ";
                    }
                }
            }
            LiftUp();
        }
        //переміщення вниз
        public static void LiftDown()
        {
            for (int j = 0; j < Field.ar.GetLength(1); j++)
            {
                int i = Field.ar.GetLength(0) - 1;
                int c = 0;
                while (true)
                {
                    //перевірка на пустоту: якщо так, то зміщуємо масив на одиницю
                    if (Field.ar[i, j] == " ")
                    {
                        for (int k = i; k >= 1; k--)
                        {
                            Field.ar[k, j] = Field.ar[k - 1, j];
                        }
                        Field.ar[0, j] = " ";
                        c++;
                    }
                    //якщо символ, то переходин на наступний індекс масиву
                    else if (Field.ar[i, j] != " ")
                    {
                        i--;
                    }
                    if (i == 0)
                    {
                        break;
                    }
                    if (c == Field.ar.GetLength(0))
                    {
                        break;
                    }
                }
            }
        }
        //зклеювання--рух вниз
        public static void ConcDown()
        {
            for (int j = 0; j < Field.ar.GetLength(1); j++)
            {
                for (int i = Field.ar.GetLength(0) - 1; i >= 1; i--)
                {
                    if (Field.ar[i, j] != " " && Field.ar[i, j] == Field.ar[i - 1, j])
                    {
                        Field.ar[i, j] = Convert.ToString(Convert.ToInt32(Field.ar[i, j]) + Convert.ToInt32(Field.ar[i - 1, j]));
                        Field.ar[i - 1, j] = " ";
                    }
                }
            }
            LiftDown();
        }

    }
    //Карта поля--публічні змінні
    class Field
    {
        public static string[,] ar = new string[4, 4];        

        public static string[,] clonAr = new string[4,4];

    }

    class Paint
    {
        //Вивід на консоль поля з числами
        public static void Draw()
        {
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                for (int j = 0; j < Field.ar.GetLength(1); j++)
                {
                    PaintDraw(Field.ar, i, j);
                    Console.Write(Field.ar[i, j] + "\t");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }

        //Замальовка
        public static void PaintDraw(string[,] ar, int i, int j)
        {
            if (ar[i, j] == " ")
                Console.BackgroundColor = ConsoleColor.Gray;
            if (ar[i, j] == "2")
                Console.BackgroundColor = ConsoleColor.Blue;
            if (ar[i, j] == "4")
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            if (ar[i, j] == "8")
                Console.BackgroundColor = ConsoleColor.Green;
            if (ar[i, j] == "16")
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            if (ar[i, j] == "32")
                Console.BackgroundColor = ConsoleColor.Magenta;
            if (ar[i, j] == "64")
                Console.BackgroundColor = ConsoleColor.Red;
            if (ar[i, j] == "128")
                Console.BackgroundColor = ConsoleColor.DarkRed;
            if (ar[i, j] == "256")
                Console.BackgroundColor = ConsoleColor.Cyan;
            if (ar[i, j] == "512")
                Console.BackgroundColor = ConsoleColor.Yellow;
            if (ar[i, j] == "1028")
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            if (ar[i, j] == "2048")
                Console.BackgroundColor = ConsoleColor.White;
        }
        //Вивід рекорду
        public static void Rec()
        {
            int rec = 0;
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                for (int j = 0; j < Field.ar.GetLength(1); j++)
                {
                    if (Field.ar[i, j] != " ")
                        rec += Convert.ToInt32(Field.ar[i, j]);
                }
            }
            Console.WriteLine("Score:3->_ " + rec);
        }
    }
    class WinLose
    {
        //Перевірка на програш
        public static void Lose()
        {
            int coufild = 0;
            int couwalk = 0;
            //перевірка можливості з'єднання(вбоки і вниз-верх)
            for (int i = 0; i < Field.ar.GetLength(0)-1; i++)
            {
                for (int j = 0; j < Field.ar.GetLength(1); j++)
                {
                    if (Field.ar[i, j] != " " && Field.ar[i, j] == Field.ar[i + 1, j])
                        couwalk++;
                }
            }
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                for (int j = 0; j < Field.ar.GetLength(1) - 1; j++)
                {
                    if (Field.ar[i, j] != " " && Field.ar[i, j] == Field.ar[i, j + 1])
                        couwalk++;
                }
            }
            //перевірка заповненості карти
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                for (int j = 0; j < Field.ar.GetLength(1); j++)
                {
                    if (Field.ar[i, j] != " ")
                        coufild++;
                }
            }
            if (coufild==16 && couwalk==0)
            {
                Console.SetCursorPosition(10, 10);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("На жаль ти програв(");
                Console.SetCursorPosition(10, 12);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Для рестарта -> \"Esc\" ");
            }
        }
        //перевірка на виграш
        public static void Win()
        {
            for (int i = 0; i < Field.ar.GetLength(0); i++)
            {
                for (int j = 0; j < Field.ar.GetLength(1); j++)
                {
                    if (Field.ar[i, j] == "2048")
                    {
                        Console.SetCursorPosition(10, 10);
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("!!!Перемога!!!");
                    }
                }
            }
        }
    }
}