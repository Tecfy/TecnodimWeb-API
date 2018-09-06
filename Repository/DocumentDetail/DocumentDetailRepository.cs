using Model.In;
using Model.Out;
using SoftExpert;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMDocumentDetailOut GetECMDocumentDetail(ECMDocumentDetailIn ecmDocumentDetailIn)
        {
            ECMDocumentDetailOut ecmDocumentDetailOut = new ECMDocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(ecmDocumentDetailIn.userId, ecmDocumentDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetECMDocumentDetail", "");

            ecmDocumentDetailOut = SEDocumentDetail.GetSEDocumentDetail(ecmDocumentDetailIn);

            registerEventRepository.SaveRegisterEvent(ecmDocumentDetailIn.userId, ecmDocumentDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetECMDocumentDetail", "");
            return ecmDocumentDetailOut;
        }
    }
}
