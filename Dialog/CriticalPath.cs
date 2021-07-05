using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Dialog
{
    /// <summary>
    /// Класс нахождения критического пути.
    /// </summary>
    public class CriticalPath
    {
        string readPath;
        string savePath;

        List<Work> work = new List<Work>(); //Список всех работ
        List<Path> pathes = new List<Path>(); //Список путей


        public struct Path
        {
            public string path, lastPoint;
            public int length;
        }
        public struct Work
        {
            public string start, workEnd;
            public int time;
        }
        /// <summary>
        /// Конструктор класса критического пути.
        /// </summary>
        public CriticalPath(string readPath, string savePath)
        {
            this.readPath = readPath;
            this.savePath = savePath;
        }

        /// <summary>
        /// Метод поиска начальной точки.
        /// </summary>
        /// <returns></returns>
        string StartingPoint() 
        {
            string tempStartPos = " ", lastPoint = "";
            int countCheck = 0;
            foreach (Work activity in work)   //Проверка, есть ли путь к точке из других точек. Если нет, то это начало.
            {
                if (work.Where(x => x.workEnd == activity.start).Count() == 0)
                {
                    tempStartPos = activity.start;
                    countCheck++;
                    if (countCheck > 1 && lastPoint != activity.start)
                    {
                        MessageBox.Show("В введенных данных присутствует несколько начальных точек отсутствует. Решение невозможно.");
                        Environment.Exit(0);
                    }
                    lastPoint = activity.start;
                }
            }
            if (countCheck == 0)
            {
                MessageBox.Show("Начальная точка отсутствует.");
                Environment.Exit(0);
            }
            return tempStartPos;
        }

        /// <summary>
        /// Метод поиска конечной точки.
        /// </summary>
        /// <returns></returns>
        string EndPoint() 
        {
            string tempEndPos = "", lastPoint = "";
            int count = 0;
            foreach (Work activity in work)   //Проверка, исходят ли из этой точки другие дуги
            {
                if (work.Where(x => x.start == activity.workEnd).Count() == 0)
                {
                    tempEndPos = activity.workEnd;
                    count++;
                    if (count > 1 && lastPoint != activity.workEnd)
                    {
                        MessageBox.Show("Найдено несколько конечных точек, решить задачу невозможно.");
                        Environment.Exit(0);
                    }
                    lastPoint = activity.workEnd;
                }
            }
            if (count == 0)
            {
                MessageBox.Show("Конечная точка отсутствует.");
                Environment.Exit(0);
            }
            return tempEndPos;
        }

        /// <summary>
        /// Метод подсчета путей.
        /// </summary>
        public void CalculatingPaths()
        {
            Debug.WriteLine("Пути: ");
            foreach (Work activity in work.Where(x => x.start == StartingPoint())) //Сначала в список путей заносятся все начальные точки
            {
                pathes.Add(new Path { path = activity.start + "-" + activity.workEnd, lastPoint = activity.workEnd, length = activity.time });
            }
            for (int i = 0; i < pathes.Count; i++) //Проверка всех записанных путей
            {
                foreach (Work activity in work.Where(x => x.start == pathes[i].lastPoint)) //Добавление путей, которые начинаются в проверяемом 
                {
                    pathes.Add(new Path { path = pathes[i].path + "-" + activity.workEnd, lastPoint = activity.workEnd, length = pathes[i].length + activity.time });                   
                }
            }

            foreach(var p in pathes) Debug.WriteLine(p.path);
        }

        /// <summary>
        /// Метод поиска критического пути.
        /// </summary>
        /// <returns></returns>
        public List<Path> FindCriticalPath()
        {
            int maxLength = 0;
            string endPos = EndPoint();
            foreach (Path path in pathes.Where(x => x.lastPoint == endPos)) //Проверяет все пути, конечная точка которых совпадает с концом сети
            {
                if (path.length > maxLength) maxLength = path.length; //Вычисляет самый длинный путь
            }
            List<Path> criticalPath = new List<Path>();
            foreach (Path path in pathes.Where(x => x.length == maxLength && x.lastPoint == endPos))
            {
                criticalPath.Add(path);
            }
            

            return criticalPath;          
        }
        /// <summary>
        /// Метод считывания данных из файла.
        /// </summary>
        public void ReadData()
        {
            if (!File.Exists(readPath))
            {
                MessageBox.Show("Файл не найден!");
                Environment.Exit(0);
            }
            var lines = File.ReadAllLines(readPath);
            try
            {
                foreach (var line in lines)
                {
                    string[] str = line.Split(';');
                    work.Add(new Work { start = str[0], workEnd = str[1], time = Convert.ToInt32(str[2]) });
                }
            }
            catch
            {
                MessageBox.Show("Неверный формат записи, выберите другой файл");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Метод записи в файл.
        /// </summary>
        /// <param name="savingPath"></param>
        public void WriteToFile(List<Path> savingPath)
        {
            if (!File.Exists(savePath)) File.Create(savePath).Close();
            try
            {
                using (StreamWriter sw = new StreamWriter(savePath, false, UnicodeEncoding.UTF8))
                {
                    if (savingPath.Count == 1)
                    {
                        Debug.WriteLine("Крит. путь - "+ savingPath[0].path);
                        sw.WriteLine("Критический путь:");
                        sw.WriteLine(savingPath[0].path);
                        sw.WriteLine("Длина пути: " + savingPath[0].length);
                    }
                    else
                    {
                        sw.WriteLine("Найденные критические пути:");
                        foreach (Path savePaths in savingPath)
                        {
                            Debug.WriteLine("Крит. путь - " + savePaths.path);
                            sw.WriteLine(savePaths.path);
                        }
                        Debug.WriteLine("Длина путей: " + savingPath[0].length);
                        sw.WriteLine("Длина путей: " + savingPath[0].length);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось записать данные в файл.");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Основной метод, вызывающий остальные важные методы.
        /// </summary>
        public void CalculateCriticalPath()
        {
            //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Listeners.Add(new TextWriterTraceListener(File.CreateText("log.txt")));
            Debug.AutoFlush = true;
            ReadData();
            CalculatingPaths();
            var criticalPath = FindCriticalPath();
            WriteToFile(criticalPath);
        }
    }
}

