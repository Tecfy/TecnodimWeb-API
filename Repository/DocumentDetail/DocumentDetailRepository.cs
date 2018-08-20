using DataEF.DataAccess;
using Model.In;
using Model.Out;
using SoftExpert;
using System.Linq;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public SEDocumentDetailOut GetSEDocumentDetail(SEDocumentDetailIn seDocumentDetailIn)
        {
            SEDocumentDetailOut seDocumentDetailOut = new SEDocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(seDocumentDetailIn.userId.Value, seDocumentDetailIn.key.Value, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetail", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.Active == true && x.DeletedDate == null && x.DocumentId == seDocumentDetailIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new System.Exception(i18n.Resource.RegisterNotFound);
                }

                seDocumentDetailIn.registration = document.Registration;
            }

            seDocumentDetailOut = DocumentDetail.GetSEDocumentDetail(seDocumentDetailIn);

            registerEventRepository.SaveRegisterEvent(seDocumentDetailIn.userId.Value, seDocumentDetailIn.key.Value, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            return seDocumentDetailOut;
        }
    }
}
