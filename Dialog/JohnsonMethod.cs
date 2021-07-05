using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog
{
    /// <summary>
    /// Класс решения задачи методом Джонсона.
    /// </summary>
    class JohnsonMethod
    {
        string readPath;
        string savePath;

        struct Item : IComparable<Item>
        {
            public int number, aTime, bTime;

            public override string ToString()
            {
                return aTime + " " + bTime;
            }

            public int CompareTo(Item item)
            {
                if (aTime <= bTime)
                {
                    if (aTime > item.aTime)
                        return 1;
                    if (aTime < item.aTime)
                        return -1;
                }
                else
                {
                    if (bTime > item.bTime)
                        return 1;
                    if (bTime < item.bTime)
                        return -1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Конструктор класса решения задачи методом Джонсона.
        /// </summary>
        /// <param name="readPath"></param>
        /// <param name="savePath"></param>
        public JohnsonMethod(string readPath, string savePath)
        {
            this.readPath = readPath;
            this.savePath = savePath;
        }

        /// <summary>
        /// Метод считывания данных из файла.
        /// </summary>
        void ReadData()
        {
            try
            {
                if (!File.Exists(readPath))
                {
                    MessageBox.Show("Файл не найден!");
                    Environment.Exit(0);
                }
                var lines = File.ReadAllLines(readPath);
                int i = 1;
                foreach (var line in lines)
                {
                    string[] str = line.Split(';');
                    items.Add(new Item { number = i, aTime = Convert.ToInt32(str[0]), bTime = Convert.ToInt32(str[1]) });
                    i++;
                }
            }
            catch
            {
                MessageBox.Show("Неверный формат записи данных!");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Метод записи данных в файл.
        /// </summary>
        void WriteToFile()
        {
            if (!File.Exists(savePath)) File.Create(savePath).Close();
            try
            {
                using (StreamWriter sw = new StreamWriter(savePath, false, UnicodeEncoding.UTF8))
                {
                    sw.WriteLine("Введенная матрица имеет вид:");
                    sw.WriteLine("Номер;\ta;\tb;");
                    foreach (Item item in items)
                    {
                        sw.WriteLine("{0};\t{1};\t{2};", item.number, item.aTime, item.bTime);
                    }
                    sw.WriteLine("Время простоя второй машины при первичном порядке равно:");
                    sw.WriteLine(prostoi.Max() + "\n");
                    sw.WriteLine("Оптимальная перестановка имеет следующий вид:");
                    sw.WriteLine("Номер;\ta;\tb;");
                    foreach (Item item in optimalItems)
                    {
                        sw.WriteLine("{0};\t{1};\t{2};", item.number, item.aTime, item.bTime);
                    }
                    sw.WriteLine("Время простоя при оптимальной перестановке равно:");
                    sw.WriteLine(optimalProstoi.Max());
                }
            }
            catch
            {
                Console.WriteLine("Не удалось записать данные в файл!");
                Environment.Exit(0);
            }
        }

        List<Item> items = new List<Item>(); //Список предметов, загруженный из файла
        List<Item> optimalItems = new List<Item>(); //Список предметов после оптимального распределения

        List<int> prostoi = new List<int>(); //Список простоев станков
        List<int> optimalProstoi = new List<int>(); //Список простоев станков после оптимального распределения

        /// <summary>
        /// Сортировка элементов
        /// </summary>
        void SortElements() //Метод сортировки элементов по массиву
        {
            List<Item> aList = new List<Item>(); //Первый временный список, для дальнейшего распределения
            List<Item> bList = new List<Item>(); //Второй временный список, для дальнейшего распределения
            foreach (Item item in items)
            {
                if (item.aTime <= item.bTime) //Если время на первом станке <=, чем на втором, то идет добавление в первый временный список
                {
                    aList.Add(item);
                }
                else bList.Add(item); //В ином случае записывается во второй временный список
            }
            aList.Sort(); //Данный список сортируется в порядке возрастания времени на первом станке (см. структуру)
            bList.Sort(); //Данный список сортируется в порядке возрастания времени на втором станке (см. структуру)
            bList.Reverse(); //Далее данный список переворачивается для дальнейшего объединения
            foreach (Item item in aList) optimalItems.Add(item); //Все данные из первого списка записываются в новый список
            foreach (Item item in bList) optimalItems.Add(item); //Все данные из второго списка записываются в новый список
        }

        /// <summary>
        /// Поиск оптимального решения.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="prostoi"></param>
        void FindOptimal(List<Item> items, List<int> prostoi)
        {
            int count = 0; //Временная переменная для подсчета простоев
            for (int i = 0; i < items.Count; i++)
            {
                if (i != 0) //Если это не первый элемент в первом списке
                    count += (items[i].aTime - items[i - 1].bTime); //То вычисляется по этой формуле
                else //Если первый, то просто записывается
                    count += items[i].aTime;
                prostoi.Add(count); //И это записывается в лист простоев
            }
        }

        /// <summary>
        /// Решить задачу методом Джонсона.
        /// </summary>
        public void Calculate()
        {
            ReadData();
            SortElements();
            FindOptimal(items, prostoi);
            FindOptimal(optimalItems, optimalProstoi);
            WriteToFile();
        }
    }
}
