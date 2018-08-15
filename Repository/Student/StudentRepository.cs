using Model.In;
using Model.Out;
using Model.VM;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class StudentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public StudentOut GetStudent(StudentIn studentIn)
        {
            StudentOut studentOut = new StudentOut();
            registerEventRepository.SaveRegisterEvent(studentIn.userId.Value, studentIn.key.Value, "Log - Start", "Repository.StudentRepository.GetStudent", "");

            List<Student> students = new List<Student>();
            students = CreateStudents();

            studentOut.result = students.Where(x => x.externalId == studentIn.externalId).Select(x => new StudentVM() { externalId = x.externalId, name = x.name, registration = x.registration, course = x.course, unity = x.unity }).FirstOrDefault();

            registerEventRepository.SaveRegisterEvent(studentIn.userId.Value, studentIn.key.Value, "Log - End", "Repository.StudentRepository.GetStudent", "");
            return studentOut;
        }

        private List<Student> CreateStudents()
        {
            List<Student> Students = new List<Student>
            {
                new Student {externalId = 1, name = "Flavia do Nascimento Alves", registration = "01201855", course = "Nutrição", unity = "Faculdade Uninassau Campina Grande" },
                new Student {externalId = 2, name = "Cicero Klebson dos Santos Silva", registration = "07020342", course = "Nutrição", unity = "Faculdade Uninassau Campina Grande"},
                new Student {externalId = 3, name = "Alessandra Viana de Lima", registration = "01203422", course = "Nutrição", unity = "Faculdade Uninassau Campina Grande"},
            };

            return Students;
        }
    }
}
