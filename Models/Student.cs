namespace GradebookApp.Models
{
    public class Student
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public double Mark { get; set; }
        
        public Student()
        {
        }
        
        public Student(string name, string subject, double mark)
        {
            Name = name;
            Subject = subject;
            Mark = mark;
        }
        
        public override string ToString()
        {
            return $"{Name} - {Subject}: {Mark:F2}";
        }
    }
}
