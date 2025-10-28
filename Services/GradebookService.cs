using System.Text.Json;
using GradebookApp.Models;

namespace GradebookApp.Services
{
    public class GradebookService
    {
        private readonly List<Student> students;
        private readonly string filePath= "gradebook.json";
        
        public GradebookService()
        {
            students = new List<Student>();
        }
        public bool AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
                
            if (student.Mark < 0 || student.Mark > 5)
                return false;
                
            if (string.IsNullOrWhiteSpace(student.Name) || string.IsNullOrWhiteSpace(student.Subject))
                return false;
                
            students.Add(student);
            SaveToFile();
            return true;
        }
        
        public bool DeleteStudent(string name, string subject)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(subject))
                return false;
                
            var studentToRemove = students.FirstOrDefault(s => 
                s.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                s.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase));
                
            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                SaveToFile();
                return true;
            }
            
            return false;
        }
        public IEnumerable<Student> FindStudentsBySubject(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                return Enumerable.Empty<Student>();
                
            return students
                .Where(s => s.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.Name)
                .ToList();
        }
        
        public IEnumerable<Student> GetAllStudents()
        {
            return students
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Subject)
                .ToList();
        }
        public void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                
                var json = JsonSerializer.Serialize(students, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
        }
        public void LoadFromFile()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var loadedStudents = JsonSerializer.Deserialize<List<Student>>(json);
                    
                    if (loadedStudents != null)
                    {
                        students.Clear();
                        students.AddRange(loadedStudents);
                        Console.WriteLine($"Загружено {loadedStudents.Count} записей из файла");
                    }
                }
                else
                {
                    Console.WriteLine("Файл данных не найден. Создан новый журнал.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
                Console.WriteLine("Создан новый журнал.");
            }
        }
        
    }
}
