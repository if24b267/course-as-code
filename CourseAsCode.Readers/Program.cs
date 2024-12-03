package reader;

import model.Course;

public class Main
{
    public static void main(String[] args)
    {
        Reader reader = new Reader();
        Course course = reader.Read("path/to/course/files", FileFormat.XML);

        System.out.println("Course Title: " + course.getTitle());
        System.out.println("Assignments: " + course.getAssignments());
        System.out.println("Forums: " + course.getForums());
        System.out.println("Quizzes: " + course.getQuizzes());
    }
}
