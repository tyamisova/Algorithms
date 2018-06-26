using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort
{
    class SortClass
    {
        //Сортировка выбором
        public uint[] Selection(uint[] mass)
        {
            for (int i = 0; i < mass.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < mass.Length; j++)
                {
                    if (mass[j] < mass[min])
                    {
                        min = j;
                    }
                }
                uint temp = mass[i];
                mass[i] = mass[min];
                mass[min] = temp;
            }
            return mass;
        }
        //Сортировка вставками
        public uint[] Insertion(uint[] mass)

        {
            for (int i = 1; i < mass.Length; i++)
            {
                uint cur = mass[i];
                int j = i;
                while (j > 0 && cur < mass[j - 1])
                {
                    mass[j] = mass[j - 1];
                    j--;
                }
                mass[j] = cur;
            }
            return mass;
        }
        //Сортировка Шелла
        public uint[] Shell(uint[] mass, int length)
        {
            int h = 0;
            for (h = 1; h <= length / 9; h = 3 * h + 1)
                ;
            for (; h > 0; h /= 3)
            {
                for (int i = h; i < length; ++i)
                {
                    int j = i;
                    uint tmp = mass[i];
                    while (j >= h && tmp < mass[j - h])
                    {
                        mass[j] = mass[j - h];
                        j -= h;
                    }
                    mass[j] = tmp;
                }
            }
            return mass;
        }
        //Быстрая сортировка. Метод Хоара
        public uint[] Quick(uint[] mass, int start, int end)
        {
            if (start >= end)
            {
                return mass;
            }
            int pivot = Partition(mass, start, end);
            Quick(mass, start, pivot - 1);
            Quick(mass, pivot + 1, end);
            return mass;
        }
        //Быстрая сортировка
        public int Partition(uint[] list, int start, int end)
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
        //Сортировка слиянием
        public uint[] Merge(uint[] mass)
        {

            if (mass.Length <= 1)
                return mass;
            Int32 mid_point = mass.Length / 2;
            return Merge2(Merge(mass.Take(mid_point).ToArray()), Merge(mass.Skip(mid_point).ToArray()));
        }
        //Сортировка слиянием.
         uint[] Merge2(uint[] mass1, uint[] mass2)
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
        //Поразрядная сортировка
        public uint[] Radix(uint[] mass, int count)
        {
            uint[,] mIndex = new uint[4, 256];
            uint[] temp = new uint[count];
            uint i, m, n, u;
            int j;
            for (i = 0; i < count; i++)
            {
                u = mass[i];
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
                    u = mass[i];
                    m = (uint)(u >> (j << 3)) & 0xff;
                    temp[mIndex[j, m]++] = u;
                }
                Swap(ref mass, ref temp);
            }
            return mass;
        }
         void Swap<T>(ref T lhs, ref T rhs)// Универсальный метод Swap
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}
