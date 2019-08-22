using Model.In;
using Model.Out;
using RegisterEvent.Events;
using SoftExpert;

namespace Repository
{
    public partial class StudentRepository
    {
        Events events = new Events();

        public StudentDocumentsOut GetStudentDocuments(StudentDocumentsIn studentDocumentsIn)
        {
            StudentDocumentsOut studentDocumentsOut = new StudentDocumentsOut();

            events.SaveRegisterEvent(studentDocumentsIn.userId, studentDocumentsIn.key, "Log - Start", "Repository.StudentRepository.GetStudentDocuments", "");

            studentDocumentsOut = SEStudent.GetSEStudentDocuments(studentDocumentsIn);

            if (studentDocumentsOut.result == null)
            {
                throw new System.Exception(i18n.Resource.StudentNotFound);
            }

            events.SaveRegisterEvent(studentDocumentsIn.userId, studentDocumentsIn.key, "Log - End", "Repository.StudentRepository.GetStudentDocuments", "");

            return studentDocumentsOut;
        }

        public StudentDocumentOut GetStudentDocument(StudentDocumentIn studentDocumentIn)
        {
            StudentDocumentOut studentDocumentOut = new StudentDocumentOut();

            events.SaveRegisterEvent(studentDocumentIn.userId, studentDocumentIn.key, "Log - Start", "Repository.StudentRepository.GetStudentDocument", "");

            studentDocumentOut = SEStudent.GetSEStudentDocument(studentDocumentIn);

            if (studentDocumentOut.result == null)
            {
                throw new System.Exception(i18n.Resource.NoDataFound);
            }

            events.SaveRegisterEvent(studentDocumentIn.userId, studentDocumentIn.key, "Log - End", "Repository.StudentRepository.GetStudentDocument", "");

            return studentDocumentOut;
        }
    }
}
