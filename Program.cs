using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Sort
{
    class Program
    {
        //Для выбора типа создаваемого массива и типа сортировки
        public enum ArrayType { FromSmallToBig, FromBigToSmall, FullRandom };
        public enum ArraySortType { SelectionSort, InsertionSort, ShellSort, QuickSort, MergeSort, RadixSort };
        static void Main(string[] args)
        {
            AutoArrayCreateAndSort(100);
            AutoArrayCreateAndSort(500);
            AutoArrayCreateAndSort(1000);
            AutoArrayCreateAndSort(5000);
            AutoArrayCreateAndSort(10000);
            AutoArrayCreateAndSort(50000);
            AutoArrayCreateAndSort(100000);
            Console.WriteLine("Нажмите \"Ввод\" для запуска теста на ошибки");
            Console.ReadLine();
            AutoArrayCreateAndSort(-1);
            uint[] newUint = CreateArray(10,ArrayType.FullRandom);
            SortArray(newUint, ArraySortType.ShellSort, ArrayType.FullRandom, 12);
            AutoArrayCreateAndSort(999999999);
            uint[] newUint2 = CreateArray(999999999, ArrayType.FullRandom);
            Console.WriteLine("Нажмите \"Ввод\" для запуска теста на неслучайных массивах");
            Console.ReadLine();
            uint[] testMass = CreateArray(10000, ArrayType.FromBigToSmall);
            Console.WriteLine("Сортировка массива из "+ testMass.Length+" элементов упорядоченных от большего к меньшему");
            SortArray(testMass, ArraySortType.SelectionSort, ArrayType.FromBigToSmall, testMass.Length);
            SortArray(testMass, ArraySortType.InsertionSort, ArrayType.FromBigToSmall, testMass.Length);
            SortArray(testMass, ArraySortType.ShellSort, ArrayType.FromBigToSmall, testMass.Length);
            SortArray(testMass, ArraySortType.QuickSort, ArrayType.FromBigToSmall, testMass.Length);
            SortArray(testMass, ArraySortType.MergeSort, ArrayType.FromBigToSmall, testMass.Length);
            SortArray(testMass, ArraySortType.RadixSort, ArrayType.FromBigToSmall, testMass.Length);
            testMass = CreateArray(10000, ArrayType.FromSmallToBig);
            Console.WriteLine("Сортировка массива из " + testMass.Length + " элементов упорядоченных от меньшего к большему");
            SortArray(testMass, ArraySortType.SelectionSort, ArrayType.FromSmallToBig, testMass.Length);
            SortArray(testMass, ArraySortType.InsertionSort, ArrayType.FromSmallToBig, testMass.Length);
            SortArray(testMass, ArraySortType.ShellSort, ArrayType.FromSmallToBig, testMass.Length);
            SortArray(testMass, ArraySortType.QuickSort, ArrayType.FromSmallToBig, testMass.Length);
            SortArray(testMass, ArraySortType.MergeSort, ArrayType.FromSmallToBig, testMass.Length);
            SortArray(testMass, ArraySortType.RadixSort, ArrayType.FromSmallToBig, testMass.Length);
            Console.WriteLine("Нажмите \"Ввод\" для выхода из программы");
            Console.ReadLine();
        }
        //Метод автоматизации создания и сортировки массивов. Принимает количество элементов массива. 
        //Один массив сортируется всеми шестью методами. Тип данных в массиве - FullRandom
        public static void AutoArrayCreateAndSort(int arrayLength)
        {
            try
            {
                uint[] testMass = CreateArray(arrayLength, ArrayType.FullRandom); //Cоздаём тестовый массив заданной длинны
                SortArray(testMass, ArraySortType.SelectionSort, ArrayType.FullRandom, arrayLength);
                SortArray(testMass, ArraySortType.InsertionSort, ArrayType.FullRandom, arrayLength);
                SortArray(testMass, ArraySortType.ShellSort, ArrayType.FullRandom, arrayLength);
                SortArray(testMass, ArraySortType.QuickSort, ArrayType.FullRandom, arrayLength);
                SortArray(testMass, ArraySortType.MergeSort, ArrayType.FullRandom, arrayLength);
                SortArray(testMass, ArraySortType.RadixSort, ArrayType.FullRandom, arrayLength);
            }
            catch (OverflowException ex)
            { Console.WriteLine("Переполнение: " + ex.Message); }
            catch (OutOfMemoryException ex)
            { Console.WriteLine("Задан массив слишком большой длинны: " + ex.Message); }
        }
        //Метод создания массивов
        public static uint[] CreateArray(int len, ArrayType AT)
        {
            try
            {
                uint[] newUInt = new uint[len];
                switch (AT)
                {
                    case ArrayType.FromBigToSmall:
                        for (int i = len - 1; i >= 0; i--)
                            newUInt[len - i - 1] = (uint)(i);
                        break;
                    case ArrayType.FromSmallToBig:
                        for (int i = len - 1; i >= 0; i--)
                            newUInt[i] = (uint)i;
                        break;
                    case ArrayType.FullRandom:
                        //Для создания случайных чисел
                        Random rnd = new Random(); ;
                        for (int i = 0; i < len; i++)
                            newUInt[i] = (uint)(rnd.NextDouble() * 3000000000);
                        break;
                    default:
                        Console.WriteLine("Массив был проинициализарован нулями, т.к. для переданного типа массива \"" + AT.ToString() + "\" не задана реализация в программе");
                        break;
                }
                return newUInt;
            }
            catch (OutOfMemoryException ex)
            {
                uint[] newUInt = new uint[1];
                Console.WriteLine("Задан массив слишком большой длинны: " + ex.Message);
                return newUInt;
            }
        }
        //Метод запуска сортировки массива, подсчета времи и записи в файл результатов
        public static void SortArray(uint[] array, ArraySortType AST, ArrayType AT, int arrayLength)
        {
            int testCount = 10;//Количество тестов
            uint[] HelpTestMass = new uint[arrayLength];// Для хранения неотсортированной копии массива
            int[] timeSortMass = new int[testCount];//Для хранения результатов тестов
            int timeSortMassSumm = 0;
            Stopwatch stopWatch;
            TimeSpan ts = new TimeSpan();
            try
            {
                for (int i = 0; i < testCount; i++)//10 испытаний одного массива
                {
                    Array.Copy(array, HelpTestMass, arrayLength);
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                    switch (AST)
                    {
                        case ArraySortType.SelectionSort:
                            HelpTestMass = Selection(HelpTestMass);
                            break;
                        case ArraySortType.InsertionSort:
                            HelpTestMass = InsertionSort(HelpTestMass);
                            break;
                        case ArraySortType.ShellSort:
                            HelpTestMass = ShellSort(HelpTestMass, arrayLength);
                            break;
                        case ArraySortType.QuickSort:
                            HelpTestMass = Quicksort(HelpTestMass, 0, arrayLength - 1);
                            break;
                        case ArraySortType.MergeSort:
                            HelpTestMass = Merge_Sort(HelpTestMass);
                            break;
                        case ArraySortType.RadixSort:
                            HelpTestMass = RadixSort(HelpTestMass, arrayLength);
                            break;
                        default:
                            Console.WriteLine("Для сортировки указанного типа \"" + AST.ToString() + "\" не задана реализация в программе");
                            break;
                    }
                    stopWatch.Stop();
                    ts = stopWatch.Elapsed;
                    timeSortMass[i] = ts.Milliseconds;
                    timeSortMassSumm += ts.Milliseconds;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Array.Sort(timeSortMass);
            Console.WriteLine("RunTime " + arrayLength + " " + AST + " " + timeSortMassSumm / testCount + " мс");
            //Механизм записи в файл с именем массива (AST) и рассматриваемым случаем (AT)
            FileStream fout;
            // Открываем файл
            try
            {
                fout = new FileStream(AST + "_" + AT + "_" + arrayLength + ".txt", FileMode.Create);
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message + "Ошибка при открытии выходного файла.");
                return;
            }
            catch (UnauthorizedAccessException exc)
            {
                Console.WriteLine(exc.Message + "Ошибка доступа. Нет прав на создание или запись");
                return;
            }
            try
            {
                byte[] info = new UTF8Encoding(true).GetBytes("Массив из " + arrayLength + " элементов типа " + AT.ToString() + ". Сортировка \"" + AST.ToString() +
                    "\" произведена за время: худшее - " + timeSortMass[testCount - 1] + ", среднее - " + timeSortMassSumm / testCount + ", лучшее - " + timeSortMass[0] + " мс \r\n");
                fout.Write(info, 0, info.Length);
                for (int i = 0; i < HelpTestMass.Length; i++)
                {
                    info = new UTF8Encoding(true).GetBytes(HelpTestMass[i] + "\r\n");
                    fout.Write(info, 0, info.Length);
                }
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message + "Ошибка при записи в файл.");
            }
            finally
            {
                fout.Close();
            }
        }
        //Сортировка выбором. Не устойчив. Во всех случаях n2
        public static uint[] Selection(uint[] list)
        {
            for (int i = 0; i < list.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < list.Length; j++)
                {
                    if (list[j] < list[min])
                    {
                        min = j;
                    }
                }
                uint temp = list[i];// swap
                list[i] = list[min];
                list[min] = temp;
            }
            return list;
        }
        //Сортировка вставками без использования дополнительной пямяти. Устойчив. В худшем - n2, в лучшем - n.
        public static uint[] InsertionSort(uint[] list)

        {
            for (int i = 1; i < list.Length; i++)
            {
                uint cur = list[i];
                int j = i;
                while (j > 0 && cur < list[j - 1])
                {
                    list[j] = list[j - 1];
                    j--;
                }
                list[j] = cur;
            }
            return list;
        }
        //Сортировка Шелла, Не устойчив. В худшем - n4/3, в лучшем - n7/6
        public static uint[] ShellSort(uint[] list, int length)
        {
            int h = 0;
            for (h = 1; h <= length / 9; h = 3 * h + 1)
                ;
            for (; h > 0; h /= 3)
            {
                for (int i = h; i < length; ++i)
                {
                    int j = i;
                    uint tmp = list[i];
                    while (j >= h && tmp < list[j - h])
                    {
                        list[j] = list[j - h];
                        j -= h;
                    }
                    list[j] = tmp;
                }
            }
            return list;
        }
        //Быстрая сортировка. Метод Хоара. Худший случай - ведущий элемент минимальный или максимальный. Неустойчивый. Сложность в среднем n loq n
        public static uint[] Quicksort(uint[] list, int start, int end)
        {
            if (start >= end)
            {
                return list;
            }
            int pivot = Partition(list, start, end);
            Quicksort(list, start, pivot - 1);
            Quicksort(list, pivot + 1, end);
            return list;
        }
        //Быстрая сортировка
        public static int Partition(uint[] list, int start, int end)
        {
            int marker = start;
            for (int i = start; i <= end; i++)
            {
                if (list[i] <= list[end])
                {
                    uint temp = list[marker];
                    list[marker] = list[i];
                    list[i] = temp;
                    marker += 1;
                }
            }
            return marker - 1;
        }
        //Сортировка слиянием. Устойчив. Сложность n loq n
        public static uint[] Merge_Sort(uint[] massive)
        {

            if (massive.Length <= 1)
                return massive;
            Int32 mid_point = massive.Length / 2;
            return Merge(Merge_Sort(massive.Take(mid_point).ToArray()), Merge_Sort(massive.Skip(mid_point).ToArray()));
        }
        //Сортировка слиянием.
        public static uint[] Merge(uint[] mass1, uint[] mass2)
        {
            uint a = 0, b = 0;
            uint[] merged = new uint[mass1.Length + mass2.Length];
            for (Int32 i = 0; i < mass1.Length + mass2.Length; i++)
            {
                if (b < mass2.Length && a < mass1.Length)
                    if (mass1[a] > mass2[b])
                        merged[i] = mass2[b++];
                    else //if int go for
                        merged[i] = mass1[a++];
                else
                    if (b < mass2.Length)
                    merged[i] = mass2[b++];
                else
                    merged[i] = mass1[a++];
            }
            return merged;
        }
        //Поразрядная сортировка. Устойчив. Сложность n
        public static uint[] RadixSort(uint[] list, int count)
        {
            uint[,] mIndex = new uint[4, 256];
            uint[] temp = new uint[count];
            uint i, m, n, u;
            int j;
            for (i = 0; i < count; i++)
            {
                u = list[i];
                for (j = 0; j < 4; j++)
                {
                    mIndex[j, (uint)(u & 0xff)]++;
                    u >>= 8;
                }
            }
            for (j = 0; j < 4; j++)
            {
                m = 0;
                for (i = 0; i < 256; i++)
                {
                    n = mIndex[j, i];
                    mIndex[j, i] = m;
                    m += n;
                }
            }
            for (j = 0; j < 4; j++)
            {             // radix sort
                for (i = 0; i < count; i++)
                {
                    u = list[i];
                    m = (uint)(u >> (j << 3)) & 0xff;
                    temp[mIndex[j, m]++] = u;
                }
                Swap(ref list, ref temp);
            }
            return list;
        }
        static void Swap<T>(ref T lhs, ref T rhs)// Универсальный метод Swap
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}
