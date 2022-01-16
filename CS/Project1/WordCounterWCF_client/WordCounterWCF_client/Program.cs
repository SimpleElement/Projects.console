using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WordCounterWcf_client
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] linesOfFile;
            StreamWriter streamOfSave;
            Dictionary<string, int> dictionary = null;
            Stopwatch watchParallel = new Stopwatch();

            System.Console.WriteLine("Введите путь к файлу");
            while (true)
            {
                System.Console.WriteLine("Формат ввода: \"disk:\\directory\\fileName\"\n" +
                    "!При вводе \"fileName\" директория по умолчанию \"C:\\Users\\Саша\\source\\repos\\WordCounter\\WordCounter\\bin\\Debug\\netcoreapp3.1\"");
                try
                {
                    System.Console.Write("> ");
                    var lines = File.ReadAllLines(System.Console.ReadLine());
                    System.Console.WriteLine("");
                    linesOfFile = lines;
                    break;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Ошибка: " + ex.Message + "\n\n" +
                        "Попробуйте повторить ввод");
                }
            }

            watchParallel.Start();
            try
            {
                using (var service = new WordCounterWCF_client.ServiceWordCounter.Service1Client())
                {
                    dictionary = service.GetData(linesOfFile);
                }
            } catch(Exception e)
            {
                System.Console.WriteLine("Ошиюка: " + e.Message + "\n" +
                    "Выход из программы");
                Environment.Exit(0);
            }
            watchParallel.Stop();
            TimeSpan resultParallel = watchParallel.Elapsed;
            string resultTimeParallel = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            resultParallel.Hours, resultParallel.Minutes, resultParallel.Seconds, resultParallel.Milliseconds / 10);
            Console.WriteLine($"Анализ и сортировка параллельного процесса закончены\n" +
                $"Время исполнения: {resultTimeParallel}\n");

            System.Console.WriteLine("Введите путь, по которому нужно сохранить файл");
            while (true)
            {
                System.Console.WriteLine("Формат ввода: \"disk:\\directory\\file.txt\"\n" +
                    "!При вводе \"fileName\" директория по умолчанию \"C:\\Users\\Саша\\source\\repos\\WordCounter\\WordCounter\\bin\\Debug\\netcoreapp3.1\"");
                try
                {
                    System.Console.Write("> ");
                    StreamWriter stream = new StreamWriter(File.Create(Console.ReadLine()), Encoding.GetEncoding("UTF-8"));
                    System.Console.WriteLine("");
                    streamOfSave = stream;
                    break;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Ошибка: {ex.Message}\n\n" +
                        "Попробуйте повторить ввод");
                }
            }

            try
            {
                foreach (var element in dictionary)
                {
                    streamOfSave.WriteLine($"Слово: {element.Key}\t\t\tКоличество повторов: {element.Value}");
                }
                streamOfSave.Close();
                Console.WriteLine("Программа завершена, проверьте файл!");
            }
            catch (Exception e)
            {
                System.Console.WriteLine("\nОшиюка: " + e.Message + "\n" +
                    "Выход из программы");
                Environment.Exit(0);
            }
        }
    }
}
