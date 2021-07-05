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
    /// Класс решения задачи критического пути.
    /// </summary>
    class CriticalPath
    {
        string readPath;
        string savePath;

        struct Activity
        {
            public string eventStart, eventEnd;
            public int time;
        }
        struct Path
        {
            public string path, lastPoint;
            public int length;
        }

        /// <summary>
        /// Конструктор класса решения критического пути.
        /// </summary>
        public CriticalPath(string readPath, string savePath)
        {
            this.readPath = readPath;
            this.savePath = savePath;
        }

        List<Activity> activities = new List<Activity>(); //Список всех работ (в графике это дуги)
        List<Path> pathes = new List<Path>(); //Список всех путей

        /// <summary>
        /// Метод считывания данных из файла.
        /// </summary>
        void ReadData()
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
                    activities.Add(new Activity { eventStart = str[0], eventEnd = str[1], time = Convert.ToInt32(str[2]) });
                }
            }
            catch
            {
                MessageBox.Show("Неверный формат записи данных!");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Метод записи в файл.
        /// </summary>
        /// <param name="savingPath"></param>
        void WriteToFile(List<Path> savingPath)
        {
            if (!File.Exists(savePath)) File.Create(savePath).Close();
            try
            {
                using (StreamWriter sw = new StreamWriter(savePath, false, UnicodeEncoding.UTF8))
                {
                    if (savingPath.Count == 1)
                    {
                        sw.WriteLine("Найденный критический путь имеет вид:");
                        sw.WriteLine(savingPath[0].path);
                        sw.WriteLine("Его длина составляет: " + savingPath[0].length);
                    }
                    else
                    {
                        sw.WriteLine("Найденные критические пути имеют вид:");
                        foreach (Path savPath in savingPath)
                            sw.WriteLine(savPath.path);
                        sw.WriteLine("Длина каждого из них составляет: " + savingPath[0].length);
                    }
                }
                Console.WriteLine("Решение записано в файл {0}.", savePath);
            }
            catch
            {
                MessageBox.Show("Не удалось записать данные в файл!");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Метод поиска стартовой точки.
        /// </summary>
        /// <returns></returns>
        string FindStartingPos() //Метод для поиска начальной точки
        {
            string tempStartPos = " ", lastPoint = "";
            int countCheck = 0;
            foreach (Activity activity in activities)   //Если нет таких дуг, которые бы входили в данную точку, то она начальная.
            {
                if (activities.Where(x => x.eventEnd == activity.eventStart).Count() == 0)
                {
                    tempStartPos = activity.eventStart;
                    countCheck++;
                    if (countCheck > 1 && lastPoint != activity.eventStart)
                    {
                        MessageBox.Show("В введенных данных присутствует несколько начальных точек отсутствует. Решение невозможно.");
                        Environment.Exit(0);
                    }
                    lastPoint = activity.eventStart;
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
        string FindEndingPos() //Метод для поиска конечной точки
        {
            string tempEndPos = "", lastPoint = "";
            int countCheck = 0;
            foreach (Activity activity in activities)   //Если нет таких дуг, которые бы исходили из данной точки, то она конечная.
            {
                if (activities.Where(x => x.eventStart == activity.eventEnd).Count() == 0)
                {
                    tempEndPos = activity.eventEnd;
                    countCheck++;
                    if (countCheck > 1 && lastPoint != activity.eventEnd)
                    {
                        MessageBox.Show("В введенных данных присутствует несколько конечных точек отсутствует. Решение невозможно.");
                        Environment.Exit(0);
                    }
                    lastPoint = activity.eventEnd;
                }
            }
            if (countCheck == 0)
            {
                MessageBox.Show("Конечная точка отсутствует.");
                Environment.Exit(0);
            }
            return tempEndPos;
        }

        /// <summary>
        /// Метод подсчета путей.
        /// </summary>
        void CalculatePathes() //Метод подсчета путей
        {
            foreach (Activity activity in activities.Where(x => x.eventStart == FindStartingPos())) //Сначала в список путей заносятся все начальные дуги
            {
                pathes.Add(new Path { path = activity.eventStart + "--" + activity.eventEnd, lastPoint = activity.eventEnd, length = activity.time });
            }
            for (int i = 0; i < pathes.Count; i++) //Затем программа начинает обход по всем записанным путям (в ходе выполнения цикла их количество пополняется)
            {
                foreach (Activity activity in activities.Where(x => x.eventStart == pathes[i].lastPoint)) //В список путей заносятся новые пути, которые исходят из проверяемого в данных момент
                {
                    //Таким образом в список заносятся все промежуточные пути, зато работает
                    pathes.Add(new Path { path = pathes[i].path + "--" + activity.eventEnd, lastPoint = activity.eventEnd, length = pathes[i].length + activity.time });
                }
            }
        }

        /// <summary>
        /// Метод поиска критического пути.
        /// </summary>
        /// <returns></returns>
        List<Path> FindCriticalPath() //Метод поиска критического пути
        {
            int maxLength = 0;
            string endPos = FindEndingPos();
            foreach (Path path in pathes.Where(x => x.lastPoint == endPos)) //Проверяет все пути, конечная точка которых совпадает с концом сети
            {
                if (path.length > maxLength) maxLength = path.length; //Вычисляет самый длинный путь из представленных
            }
            List<Path> criticalPath = new List<Path>();
            foreach (Path path in pathes.Where(x => x.length == maxLength && x.lastPoint == endPos))
            {
                criticalPath.Add(path);
            }
            return criticalPath;
        }

        /// <summary>
        /// Вычислить критический путь.
        /// </summary>
        public void CalculateCriticalPath()
        {
            ReadData();
            CalculatePathes();
            var criticalPath = FindCriticalPath();
            WriteToFile(criticalPath);
        }
    }
}

