using Model.In;
using Model.Out;
using SoftExpert;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public SEDocumentDetailOut GetDocumentDetail(SEDocumentDetailIn seDocumentDetailIn)
        {
            SEDocumentDetailOut seDocumentDetailOut = new SEDocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(seDocumentDetailIn.userId.Value, seDocumentDetailIn.key.Value, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            
            seDocumentDetailOut = DocumentDetail.GetDocumentDetail(seDocumentDetailIn);

            registerEventRepository.SaveRegisterEvent(seDocumentDetailIn.userId.Value, seDocumentDetailIn.key.Value, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            return seDocumentDetailOut;
        }
    }
}
