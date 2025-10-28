using System.Text;
using GradebookApp.Models;
using GradebookApp.Services;

namespace GradebookApp
{
    class Program
    {
        private static GradebookService gradebook = new GradebookService();

        static void Main(string[] args)
        {
            Console.WriteLine("Система учета оценок");

            gradebook.LoadFromFile();

            string choice = "0";

            do
            {
                string stringToPrint = choice switch
                {
                    "0" => ShowMenu(),
                    "1" => AddStudent(),
                    "2" => DeleteStudent(),
                    "3" => SearchBySubject(),
                    "4" => ShowAllStudents(),
                    _ => "Неверный выбор! Введите число от 0 до 5"
                };
                Console.WriteLine(stringToPrint);
                Console.WriteLine("Выберите действие: ");
                choice = Console.ReadLine();
            } while (choice != "5");
        }

        static string ShowMenu()
        {
            return "\n0. Показать доступные действия" +
                   "\n1. Добавить студента" +
                   "\n2. Удалить студента по имени и предмету" +
                   "\n3. Поиск по предмету" +
                   "\n4. Показать всех студентов" +
                   "\n5. Выход";
        }

        static string AddStudent()
        {

            Console.Write("Введите имя студента: ");
            string name = Console.ReadLine();

            Console.Write("Введите предмет: ");
            string subject = Console.ReadLine();

            Console.Write("Введите оценку от 0 до 5: ");
            if (double.TryParse(Console.ReadLine(), out double mark))
            {
                var student = new Student
                {
                    Name = name,
                    Subject = subject,
                    Mark = mark
                };

                if (gradebook.AddStudent(student))
                {
                    return $"Студент {name} добавлен в журнал";
                }

                return "Ошибка: оценка должна быть от 0 до 5";
            }

            return "Ошибка: введите корректное число для оценки";

        }

        static string DeleteStudent()
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите предмет: ");
            string subject = Console.ReadLine();

            if (gradebook.DeleteStudent(name, subject))
            {
                return $"Студент {name} с предметом {subject} успешно удален";
            }

            return $"Студента с именем {name} и предметом {subject} не найдено";
        }

        static string SearchBySubject()
        {
            Console.Write("Введите предмет: ");
            string subject = Console.ReadLine();

            var students = gradebook.FindStudentsBySubject(subject);

            if (students.Any())
            {
                StringBuilder result = new StringBuilder();
                result.AppendLine($"\nРезультаты поиска по предмету: {subject}");
                result.AppendLine(" № |        Имя |   Оценка");
                result.AppendLine("---|------------|---------");
                int index = 1;
                foreach (var student in students)
                {
                    result.AppendLine($"{index,2} | {student.Name,-10} | {student.Mark,6:F2}");
                    index++;
                }

                result.AppendLine($"\nНайдено студентов: {students.Count()}");
                return result.ToString();
            }

            return $"Студентов по предмету '{subject}' не найдено";
        }

        static string ShowAllStudents()
        {
            var students = gradebook.GetAllStudents();

            if (students.Any())
            {
                StringBuilder result = new StringBuilder();
                result.AppendLine("\n № |        Имя |    Предмет |   Оценка");
                result.AppendLine("---|------------|------------|---------");

                int index = 1;
                foreach (var student in students)
                {
                    result.AppendLine($"{index,2} | {student.Name,-10} | {student.Subject,-10} | {student.Mark,6:F2}");
                    index++;
                }

                return result.ToString();
            }
            return "Журнал пуст";

        }
    }
}
