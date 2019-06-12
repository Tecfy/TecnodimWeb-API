using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class StudentsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        StudentRepository studentRepository = new StudentRepository();

        [Authorize, HttpGet]
        public StudentDocumentsOut GetStudentDocuments(string username, string password, string registration = null, string name = null, string cpf = null)
        {
            StudentDocumentsOut studentDocumentsOut = new StudentDocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                StudentDocumentsIn studentDocumentsIn = new StudentDocumentsIn() { username = username, password = password, registration = registration, name = name, cpf = cpf, userId = User.Identity.Name, key = Key.ToString() };

                studentDocumentsOut = studentRepository.GetStudentDocuments(studentDocumentsIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.StudentsController.GetStudentDocuments", ex.Message);

                studentDocumentsOut.result = null;
                studentDocumentsOut.successMessage = null;
                studentDocumentsOut.messages.Add(ex.Message);
            }

            return studentDocumentsOut;
        }

        [Authorize, HttpGet]
        public StudentDocumentOut GetStudentDocument(string username, string password, string iddocument)
        {
            StudentDocumentOut studentDocumentOut = new StudentDocumentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                StudentDocumentIn studentDocumentIn = new StudentDocumentIn() { username = username, password = password, iddocument = iddocument, userId = User.Identity.Name, key = Key.ToString() };

                studentDocumentOut = studentRepository.GetStudentDocument(studentDocumentIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.StudentsController.GetStudentDocument", ex.Message);

                studentDocumentOut.result = null;
                studentDocumentOut.successMessage = null;
                studentDocumentOut.messages.Add(ex.Message);
            }

            return studentDocumentOut;
        }
    }
}