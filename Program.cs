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
        //Для выбора типа создаваемой сортировки
        public enum ArrayType { FromSmallToBig, FromBigToSmall, FullRandom };
        static void Main(string[] args)
        {
            SortArray(ArrayType.FullRandom,100);
            SortArray(ArrayType.FullRandom,500);
            SortArray(ArrayType.FullRandom,1000);
            SortArray(ArrayType.FullRandom,5000);
            SortArray(ArrayType.FullRandom,10000);
            SortArray(ArrayType.FullRandom,50000);
            SortArray(ArrayType.FullRandom,100000);
            Console.WriteLine("Нажмите \"Ввод\" для запуска теста на неслучайных массивах");
            Console.ReadLine();
            Console.WriteLine("Сортировка массива из 10000 элементов упорядоченных от большего к меньшему");
            SortArray( ArrayType.FromBigToSmall, 10000);
            Console.WriteLine("Сортировка массива из 10000 элементов упорядоченных от меньшего к большему");
            SortArray( ArrayType.FromSmallToBig, 10000);

            Console.WriteLine("Нажмите \"Ввод\" для выхода из программы");
            Console.ReadLine();
        }

        //Метод создания массивов
        public static uint[] CreateArray(int len, ArrayType AT)
        {
            try
            {
                uint[] newUInt = new uint[len];
                if (AT== ArrayType.FromBigToSmall)
                {
                    for (int i = len - 1; i >= 0; i--)
                        newUInt[len - i - 1] = (uint)(i);
                }
                else if (AT== ArrayType.FromSmallToBig)
                {
                    for (int i = len - 1; i >= 0; i--)
                        newUInt[i] = (uint)i;
                }
                else if (AT == ArrayType.FullRandom)
                {
                    Random rnd = new Random(); ;
                    for (int i = 0; i < len; i++)
                        newUInt[i] = (uint)(rnd.NextDouble() * 3000000000);
                }
                else
                    Console.WriteLine("Массив был проинициализарован нулями, т.к. для переданного типа массива \"" + AT.ToString() + "\" не задана реализация в программе");
                return newUInt;
            }
            catch (OutOfMemoryException ex)
            {
                uint[] newUInt = new uint[1];
                Console.WriteLine("Задан массив слишком большой длинны: " + ex.Message);
                return newUInt;
            }
        }
        public static void SortArray( ArrayType AT, int arrayLength)
        {
            uint[] array=new uint[arrayLength];
            //Cоздаём тестовый массив заданной длинны и типа
            try
            {
               array = CreateArray(arrayLength, ArrayType.FullRandom); 
            }
            catch (OverflowException ex)
            { Console.WriteLine("Переполнение: " + ex.Message); }
            catch (OutOfMemoryException ex)
            { Console.WriteLine("Задан массив слишком большой длинны: " + ex.Message); }

            string[] sortNames = { "Selection", "Insertion", "Merge", "Quick", "Radix", "Shell"};//Названия массивов в порядке сортировки
            SortClass sc = new SortClass();
            for (int sortNum = 0; sortNum < 6; sortNum++)
            {
                int testCount = 10;//Количество тестов
                uint[] HelpTestMass = new uint[arrayLength];// Для хранения неотсортированной копии массива
                int[] timeSortMass = new int[testCount];//Для хранения результатов тестов
                int timeSortMassMid = 0;//для хранения среднего результата испытаний
                Stopwatch stopWatch;
                try
                {
                    for (int j = 0; j < testCount; j++)//10 испытаний одного массива
                    {
                        Array.Copy(array, HelpTestMass, arrayLength);
                        stopWatch = new Stopwatch();
                        stopWatch.Start();
                        if (sortNum == 0)
                            HelpTestMass = sc.Selection(HelpTestMass);
                        else if (sortNum == 1)
                            HelpTestMass = sc.Insertion(HelpTestMass);
                        else if (sortNum == 2)
                            HelpTestMass = sc.Merge(HelpTestMass);
                        else if (sortNum == 3)
                            HelpTestMass = sc.Quick(HelpTestMass, 0, arrayLength - 1);
                        else if (sortNum == 4)
                            HelpTestMass = sc.Radix(HelpTestMass, arrayLength);
                        else if (sortNum == 5)
                            HelpTestMass = sc.Shell(HelpTestMass, arrayLength);
                        else
                            Console.WriteLine("Задан неверный код типа сортировки");
                        stopWatch.Stop();
                        timeSortMass[j] = (int)stopWatch.ElapsedMilliseconds;
                        timeSortMassMid += (int)stopWatch.ElapsedMilliseconds;
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
                timeSortMassMid = timeSortMassMid / testCount;
                Array.Sort(timeSortMass);
                Console.WriteLine("RunTime " + arrayLength + " " + sortNames[sortNum] + " " + timeSortMassMid + " мс");
                //Механизм записи в файл с именем массива и рассматриваемым случаем
                FileStream fout;
                // Открываем файл
                try
                {
                    fout = new FileStream(sortNames[sortNum] + "_" + AT + "_" + arrayLength + ".txt", FileMode.Create);
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
                    byte[] info = new UTF8Encoding(true).GetBytes("Массив из " + arrayLength + " элементов типа " + AT.ToString() + ". Сортировка \"" + sortNames[sortNum].ToString() +
                        "\" произведена за время: худшее - " + timeSortMass[testCount - 1] + ", среднее - " + timeSortMassMid + ", лучшее - " + timeSortMass[0] + " мс \r\n");
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
                testCount = 0;//обнуляем счетчик тестов
            }
        }
    }
}
