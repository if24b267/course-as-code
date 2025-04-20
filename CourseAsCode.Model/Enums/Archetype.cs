namespace CourseAsCode.Model.Enums;

public enum Archetype
{
    Manager, // lesser administrator role
    CourseCreator, // can create courses
    EditingTeacher, // can manage and add content to courses
    NonEditingTeacher, // can grade in courses but not edit them
    Student, // can access and participate in courses
    Guest, // can view courses but not participate
    AuthenticatedUser, // the role all logged-in users have
    AuthenticatedUserOnFrontpage, // a logged-in user role for the front page only

                                          // Note: The administrator is not included here, as it is a special case
}
