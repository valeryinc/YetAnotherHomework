using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Program
{
    public static void Main()
    {
        Student negr = new("Пипи", "Пупу", "Вертолет");
        Student nig = new("Бара", "Бере", "Танк");

        negr.AddCompletedCourse("Геншиноведение");
        negr.AddCourse("Хонкаеведение");
        nig.AddCompletedCourse("Танковедение");
        nig.AddCourse("Дотаведение");

        Lecturer dotas = new("Дота", "Танков", "Супра", new() { "Танковедение", "Дотаведение" });
        Lecturer hoyo = new("Сунь", "Вчай", "Гачи", new() { "Геншиноведение", "Хонкаеведение" });

        Reviewer kok = new("Папа", "Пепе", "Гене", new() { "Танковедение", "Дотаведение", "Геншиноведение", "Хонкаеведение" });

        negr.SetGrade(hoyo, "Геншиноведение", 9);
        negr.SetGrade(hoyo, "Хонкаеведение", 5);
        nig.SetGrade(dotas, "Танковедение", 8);
        nig.SetGrade(dotas, "Дотаведение", 6);

        kok.SetGrade(negr, "Геншиноведение", 10);
        kok.SetGrade(negr, "Хонкаеведение", 6);
        kok.SetGrade(nig, "Танковедение", 7);
        kok.SetGrade(nig, "Дотаведение", 7);

        negr.PrintInfo();
        nig.PrintInfo();
        dotas.PrintInfo();
        hoyo.PrintInfo();
        kok.PrintInfo();

        double GetAverageGradeByStudentCourse(Student[] students, string course)
        {
            double sum = 0;
            int count = 0;
            foreach(var s in students)
            {
                if (!s.grades.ContainsKey(course)) continue;
                count += s.grades[course].Count;
                foreach (var g in s.grades[course]) sum += g;
            }

            return sum / count;
        }

        double GetAverageGradeByLecturerCourse(Lecturer[] lecturer, string course)
        {
            double sum = 0;
            int count = 0;
            foreach (var s in lecturer)
            {
                if (!s.grades.ContainsKey(course)) continue;
                count += s.grades[course].Count;
                foreach (var g in s.grades[course]) sum += g;
            }

            return sum / count;
        }
    }
}

public class Student
{
    public string firstName;
    public string lastName;
    public string gender;

    public HashSet<string> courses = new(); 
    public Dictionary<string, List<int>> grades = new();
    public HashSet<string> currentCourses = new();

    public Student(string firstName, string lastName, string gender)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.gender = gender;
    }

    public void AddCompletedCourse(string course)
    {
        courses.Add(course);
        if(currentCourses.Contains(course)) currentCourses.Remove(course);
    }

    public void AddCourse(string course)
    {
        currentCourses.Add(course);
    }

    public void SetGrade(Lecturer lecturer, string course, int grade)
    {
        if (currentCourses.Contains(course) && lecturer.courses.Contains(course))
        {
            if (lecturer.grades.ContainsKey(course))
                lecturer.grades[course].Add(grade);
            else
                lecturer.grades.Add(course, new List<int> { grade });
        }
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Имя: {firstName}\nФамилия: {lastName}\n" +
                          $"Средняя оценка за домашние задания: {GetAverageGrade()}");
        Console.Write("Курсы в процессе изучения: ");
        foreach(var n in currentCourses) Console.Write(n);
        Console.WriteLine();
        Console.Write("Завершенные курсы: ");
        foreach (var n in courses) Console.Write(n);
        Console.WriteLine();
    }

    private double GetAverageGrade()
    {
        double sum = 0;
        int count = 0;

        foreach(var n in grades)
        {
            count += n.Value.Count;

            foreach (var g in n.Value) sum += g;
        }

        return sum / count;
    }
}

public class Mentor
{
    public string firstName;
    public string lastName;
    public string gender;
    public HashSet<string> courses = new();

    public Mentor(string firstName, string lastName, string gender, HashSet<string> courses)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.gender = gender;
        this.courses = courses;
    }
}

public class Lecturer : Mentor
{
    public Dictionary<string, List<int>> grades;
    public Lecturer(string firstName, string lastName, string gender, HashSet<string> courses) 
        : base(firstName, lastName, gender, courses)
    {
        grades = new();
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Имя: {firstName}\nФамилия: {lastName}\n" +
                          $"Средняя оценка за лекции: {GetAverageGrade()}");
    }

    private double GetAverageGrade()
    {
        double sum = 0;
        int count = 0;

        foreach (var n in grades)
        {
            count += n.Value.Count;

            foreach (var g in n.Value) sum += g;
        }

        return sum / count;
    }
}

public class Reviewer : Mentor
{
    public Reviewer(string firstName, string lastName, string gender, HashSet<string> courses)
        : base(firstName, lastName, gender, courses) { }

    public void SetGrade(Student student, string course, int grade)
    {
        if (!student.currentCourses.Contains(course)) return;
        if (student.grades.ContainsKey(course)) student.grades[course].Add(grade);
        else student.grades.Add(course, new List<int> { grade });
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Имя: {firstName}\nФамилия: {lastName}");
    }
}
