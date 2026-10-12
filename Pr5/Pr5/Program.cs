/*создать систему управления университетом. 
сис-ма должна позволять управлять информацией о студентах, преподавателях и курсах через консоль
в универе есть студенты, к-е могут записываться на различные курсы
у каждого курса есть препод, к-й его ведет
сис-ма должна хранить информацию обо всех участниках учебного процесса и позволять выполнять различные операции
юзер должен иметь возможность добав-ть в сис-му новых студентов и просматривать инфу о них
нужно реализовать функц-ть записи студентов на курсы и просмотра списка всех курсов, на к-е записан конкретный студент
сис-ма должна позволять добавлять преподов и просматривать инфу о каждом
преподы могут быть назначены на различные курсы, которые они будут вести
для управ-я курсами нужна возм-ть созд-я новых курсов, просмотра детальной инфы о каждом курсе и вывода списка всех студентов, записанных на конкретный курс
программа должна предост-ть возм-сть вывода полных списков: 
всех студентов в системе, всех преподавателей и всех доступных курсов.бб

ТРЕБОВАНИЯ:
1. выделить общие хар-ки и повед-е для похожих сущн-й
2. студ-ты и преподы имеют общие хар-ки. испол-ть наследов-е
3. д-е объектов должны быть защищены от прямого доступа
разнве типы людей в универе могу иметь разное представл-е своей инфы */

using System;
using System.Collections.Generic;
using System.Linq;

abstract class Person
{
    private static int nextId = 1;

    private string name;
    private int age;
    private string email;

    public int Id { get; private set; }

    public string Name
    {
        get { return name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty");
            name = value.Trim();
        }
    }

    public int Age
    {
        get { return age; }
        set
        {
            if (value < 16 || value > 100)
                throw new ArgumentOutOfRangeException("Age must be between 16 and 100");
            age = value;
        }
    }

    public string Email
    {
        get { return email; }
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@") || !value.Contains(".") || value.Contains(" "))
                throw new ArgumentException("Invalid email");
            email = value.Trim();
        }
    }

    protected Person(string name, int age, string email)
    {
        Id = nextId++;
        Name = name;
        Age = age;
        Email = email;
    }

    public abstract string GetRole();

    public virtual string GetInfo()
    {
        return $"[{GetRole()}] Id: {Id}, Name: {Name}, Age: {Age}, Email: {Email}";
    }
}

class Student : Person
{
    public Student(string name, int age, string email)
        : base(name, age, email)
    {
    }

    public override string GetRole()
    {
        return "student";
    }
}

class Teacher : Person
{
    private string departament;

    public string Departament
    {
        get { return departament; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Department cannot be empty");
            departament = value.Trim();
        }
    }

    public Teacher(string name, int age, string email, string departament)
        : base(name, age, email)
    {
        Departament = departament;
    }

    public override string GetRole()
    {
        return "teacher";
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", Department: {Departament}";
    }
}

enum CourseLevel
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3
}

class Course
{
    private static int nextId = 1;

    private const int MaxStudents = 30;

    private string title;
    private Teacher teacher;
    private List<Student> students = new List<Student>();

    public int Id { get; private set; }
    public CourseLevel Level { get; private set; }

    public string Title
    {
        get { return title; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Course title cannot be empty");
            title = value.Trim();
        }
    }

    public Teacher Teacher
    {
        get { return teacher; }
        set
        {
            if (value == null)
                throw new ArgumentException("The course must have a teacher");
            teacher = value;
        }
    }

    public IReadOnlyList<Student> Students
    {
        get { return students; }
    }

    public Course(string title, CourseLevel level, Teacher teacher)
    {
        if (!Enum.IsDefined(typeof(CourseLevel), level))
            throw new ArgumentException("Unknown course level");

        Title = title;
        Level = level;
        Teacher = teacher;

        Id = nextId++;
    }

    public void AddStudent(Student student)
    {
        if (student == null)
            throw new ArgumentException("Student cannot be null");

        if (students.Contains(student))
            throw new InvalidOperationException($"{student.Name} is already enrolled in {Title}");

        if (students.Count >= MaxStudents)
            throw new InvalidOperationException("There are no available places");

        students.Add(student);
    }

    public string GetLevelText()
    {
        switch (Level)
        {
            case CourseLevel.Beginner:
                return "beginner";
            case CourseLevel.Intermediate:
                return "intermediate";
            default:
                return "advanced";
        }
    }

    public string GetInfo()
    {
        return $"Id: {Id}, Course: {Title}, Level: {GetLevelText()}, " +
               $"Teacher: {Teacher.Name}, Students: {students.Count}/{MaxStudents}";
    }
}

class University
{
    private List<Student> students = new List<Student>();
    private List<Teacher> teachers = new List<Teacher>();
    private List<Course> courses = new List<Course>();

    public IReadOnlyList<Student> Students { get { return students; } }
    public IReadOnlyList<Teacher> Teachers { get { return teachers; } }
    public IReadOnlyList<Course> Courses { get { return courses; } }

    public List<Person> GetAllPeople()
    {
        List<Person> all = new List<Person>();
        all.AddRange(students);
        all.AddRange(teachers);
        return all;
    }

    private void CheckEmailIsFree(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        bool exists = GetAllPeople().Any(p => p.Email.ToLower() == email.Trim().ToLower());
        if (exists)
            throw new InvalidOperationException("A person with this email already exists");
    }

    public Student AddStudent(string name, int age, string email)
    {
        CheckEmailIsFree(email);
        Student student = new Student(name, age, email);
        students.Add(student);
        return student;
    }

    public Teacher AddTeacher(string name, int age, string email, string department)
    {
        CheckEmailIsFree(email);
        Teacher teacher = new Teacher(name, age, email, department);
        teachers.Add(teacher);
        return teacher;
    }

    public Course AddCourse(string title, CourseLevel level, int teacherId)
    {
        if (!string.IsNullOrWhiteSpace(title) &&
            courses.Any(c => c.Title.ToLower() == title.Trim().ToLower()))
            throw new InvalidOperationException("This course already exists");

        Teacher teacher = FindTeacher(teacherId);
        Course course = new Course(title, level, teacher);
        courses.Add(course);
        return course;
    }

    public Student FindStudent(int id)
    {
        Student student = students.FirstOrDefault(s => s.Id == id);
        if (student == null)
            throw new ArgumentException($"Student with ID {id} not found");
        return student;
    }

    public Teacher FindTeacher(int id)
    {
        Teacher teacher = teachers.FirstOrDefault(t => t.Id == id);
        if (teacher == null)
            throw new ArgumentException($"Teacher with ID {id} not found");
        return teacher;
    }

    public Course FindCourse(int id)
    {
        Course course = courses.FirstOrDefault(c => c.Id == id);
        if (course == null)
            throw new ArgumentException($"Course with ID {id} not found");
        return course;
    }

    public List<Course> GetStudentCourses(Student student)
    {
        return courses.Where(c => c.Students.Contains(student)).ToList();
    }

    public List<Course> GetTeacherCourses(Teacher teacher)
    {
        return courses.Where(c => c.Teacher == teacher).ToList();
    }
}


class Program
{
    static University university = new University();

    static string ReadString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Error: the string cannot be empty. Try again.");
            }
            else
            {
                return text.Trim();
            }
        }
    }

    static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();
            int number;

            if (!int.TryParse(text, out number))
            {
                Console.WriteLine("Enter an integer");
            }
            else if (number < min || number > max)
            {
                Console.WriteLine($"The number must be between {min} and {max}.");
            }
            else
            {
                return number;
            }
        }
    }

    static void AddStudent()
    {
        string name = ReadString("Name: ");
        int age = ReadInt("Age: ", 16, 100);
        string email = ReadString("Email: ");

        Student student = university.AddStudent(name, age, email);
        Console.WriteLine($"Success! Student's ID: {student.Id}");
    }

    static void ShowStudents()
    {
        Console.WriteLine("A list of students");

        if (university.Students.Count == 0)
        {
            Console.WriteLine("There are no students yet");
            return;
        }

        foreach (Student student in university.Students.OrderBy(s => s.Name))
        {
            Console.WriteLine(student.GetInfo());
        }
    }

    static void AddTeacher()
    {
        string name = ReadString("Name: ");
        int age = ReadInt("Age: ", 16, 100);
        string email = ReadString("Email: ");
        string department = ReadString("Department: ");

        Teacher teacher = university.AddTeacher(name, age, email, department);
        Console.WriteLine($"Success! Teacher's ID: {teacher.Id}");
    }

    static void ShowTeachers()
    {
        Console.WriteLine("A list of teachers");

        if (university.Teachers.Count == 0)
        {
            Console.WriteLine("There are no teachers yet");
            return;
        }

        foreach (Teacher teacher in university.Teachers.OrderBy(t => t.Name))
        {
            Console.WriteLine(teacher.GetInfo());
        }
    }

    static void AddCourse()
    {
        if (university.Teachers.Count == 0)
        {
            Console.WriteLine("There are no teachers yet. Add a teacher first.");
            return;
        }

        string title = ReadString("Course title: ");
        int level = ReadInt("Level (1 - Beginner, 2 - Intermediate, 3 - Advanced): ", 1, 3);

        Console.WriteLine("Available teachers:");
        foreach (Teacher t in university.Teachers)
        {
            Console.WriteLine(t.GetInfo());
        }

        int teacherId = ReadInt("Teacher ID: ", 1, int.MaxValue);

        Course course = university.AddCourse(title, (CourseLevel)level, teacherId);
        Console.WriteLine($"Success! Course's ID: {course.Id}");
    }

    static void ShowCourses()
    {
        Console.WriteLine("A list of courses");

        if (university.Courses.Count == 0)
        {
            Console.WriteLine("There are no courses yet");
            return;
        }

        foreach (Course course in university.Courses.OrderBy(c => c.Title))
        {
            Console.WriteLine(course.GetInfo());
        }
    }

    static void EnrollStudentInCourse()
    {
        if (university.Students.Count == 0 || university.Courses.Count == 0)
        {
            Console.WriteLine("You need at least one student and one course.");
            return;
        }

        Console.WriteLine("Students:");
        foreach (Student s in university.Students)
        {
            Console.WriteLine(s.GetInfo());
        }
        int studentId = ReadInt("Student ID: ", 1, int.MaxValue);

        Console.WriteLine("Courses:");
        foreach (Course c in university.Courses)
        {
            Console.WriteLine(c.GetInfo());
        }
        int courseId = ReadInt("Course ID: ", 1, int.MaxValue);

        Student student = university.FindStudent(studentId);
        Course course = university.FindCourse(courseId);
        course.AddStudent(student);

        Console.WriteLine($"Success! {student.Name} enrolled in {course.Title}");
    }

    static void ShowStudentCourses()
    {
        if (university.Students.Count == 0)
        {
            Console.WriteLine("There are no students yet");
            return;
        }

        Console.WriteLine("Students:");
        foreach (Student s in university.Students)
        {
            Console.WriteLine(s.GetInfo());
        }
        int studentId = ReadInt("Student ID: ", 1, int.MaxValue);

        Student student = university.FindStudent(studentId);
        List<Course> courses = university.GetStudentCourses(student);

        Console.WriteLine($"Courses of {student.Name}:");
        if (courses.Count == 0)
        {
            Console.WriteLine("No courses");
            return;
        }

        foreach (Course c in courses)
        {
            Console.WriteLine(c.GetInfo());
        }
    }

    static void ShowCourseStudents()
    {
        if (university.Courses.Count == 0)
        {
            Console.WriteLine("There are no courses yet");
            return;
        }

        Console.WriteLine("Courses:");
        foreach (Course c in university.Courses)
        {
            Console.WriteLine(c.GetInfo());
        }
        int courseId = ReadInt("Course ID: ", 1, int.MaxValue);

        Course course = university.FindCourse(courseId);

        Console.WriteLine($"Students of {course.Title}:");
        if (course.Students.Count == 0)
        {
            Console.WriteLine("No students");
            return;
        }

        foreach (Student s in course.Students)
        {
            Console.WriteLine(s.GetInfo());
        }
    }

    static void AssignTeacherToCourse()
    {
        if (university.Courses.Count == 0 || university.Teachers.Count == 0)
        {
            Console.WriteLine("You need at least one course and one teacher.");
            return;
        }

        Console.WriteLine("Courses:");
        foreach (Course c in university.Courses)
        {
            Console.WriteLine(c.GetInfo());
        }
        int courseId = ReadInt("Course ID: ", 1, int.MaxValue);

        Console.WriteLine("Teachers:");
        foreach (Teacher t in university.Teachers)
        {
            Console.WriteLine(t.GetInfo());
        }
        int teacherId = ReadInt("Teacher ID: ", 1, int.MaxValue);

        Course course = university.FindCourse(courseId);
        Teacher teacher = university.FindTeacher(teacherId);
        course.Teacher = teacher;

        Console.WriteLine($"Success! {teacher.Name} assigned to {course.Title}");
    }

    static void ShowAllPeople()
    {
        Console.WriteLine("All people of the university");

        List<Person> all = university.GetAllPeople();
        if (all.Count == 0)
        {
            Console.WriteLine("There are no people yet");
            return;
        }

        foreach (Person p in all.OrderBy(p => p.Name))
        {
            Console.WriteLine(p.GetInfo());
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("1. Add a student");
        Console.WriteLine("2. Show a list of all students");
        Console.WriteLine("3. Add a teacher");
        Console.WriteLine("4. Show a list of all teachers");
        Console.WriteLine("5. Create a course");
        Console.WriteLine("6. Show a list of all courses");
        Console.WriteLine("7. Enroll a student in a course");
        Console.WriteLine("8. Show the student's courses");
        Console.WriteLine("9. Show course students");
        Console.WriteLine("10. Assign a teacher to a course");
        Console.WriteLine("11. Show all university's people");
        Console.WriteLine("0. Exit");
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        bool running = true;

        while (running)
        {
            ShowMenu();
            int choice = ReadInt("Choose an option: ", 0, 11);
            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case 0:
                        running = false;
                        Console.WriteLine("Bye");
                        break;
                    case 1:
                        AddStudent();
                        break;
                    case 2:
                        ShowStudents();
                        break;
                    case 3:
                        AddTeacher();
                        break;
                    case 4:
                        ShowTeachers();
                        break;
                    case 5:
                        AddCourse();
                        break;
                    case 6:
                        ShowCourses();
                        break;
                    case 7:
                        EnrollStudentInCourse();
                        break;
                    case 8:
                        ShowStudentCourses();
                        break;
                    case 9:
                        ShowCourseStudents();
                        break;
                    case 10:
                        AssignTeacherToCourse();
                        break;
                    case 11:
                        ShowAllPeople();
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }

            Console.WriteLine();
        }
    }
}