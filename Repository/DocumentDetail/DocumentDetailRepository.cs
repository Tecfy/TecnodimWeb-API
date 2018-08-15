using Model.In;
using Model.Out;
using Model.VM;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public DocumentDetailOut GetDocumentDetail(DocumentDetailIn studentIn)
        {
            DocumentDetailOut studentOut = new DocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(studentIn.userId.Value, studentIn.key.Value, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetail", "");

            List<DocumentDetail> students = new List<DocumentDetail>();
            students = CreateDocumentDetails();

            studentOut.result = students.Where(x => x.documentid == studentIn.documentId).Select(x => new DocumentDetailVM() { documentId = x.documentid, name = x.name, registration = x.registration, course = x.course, unity = x.unity }).FirstOrDefault();

            registerEventRepository.SaveRegisterEvent(studentIn.userId.Value, studentIn.key.Value, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            return studentOut;
        }

        private List<DocumentDetail> CreateDocumentDetails()
        {
            List<DocumentDetail> DocumentDetails = new List<DocumentDetail>
            {
                new DocumentDetail {documentid = 1, name = "Flavia do Nascimento Alves", registration = "01201855", course = "Nutrição", unity = "Faculdade Uninassau Campina Grande" },
                new DocumentDetail {documentid = 2, name = "Cicero Klebson dos Santos Silva", registration = "07020342", course = "Nutrição", unity = "Faculdade Uninassau Campina Grande"},
                new DocumentDetail {documentid = 3, name = "Alessandra Viana de Lima", registration = "01203422", course = "Nutrição", unity = "Faculdade Uninassau Campina Grande"},
            };

            return DocumentDetails;
        }
    }
}
