using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System.Linq;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class DocumentDetail
    {
        public static SEDocumentDetailOut GetSEDocumentDetail(SEDocumentDetailIn seDocumentDetailIn)
        {
            SEDocumentDetailOut seDocumentDetailOut = new SEDocumentDetailOut();

            SEClient seClient = Connection.GetConnection();
            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.DocumentDetail.SearchAttributeOwnerRegistration"],
                VLATTRIBUTE = seDocumentDetailIn.registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter();
            searchDocumentFilter.IDCATEGORY = WebConfigurationManager.AppSettings["SoftExpert.DocumentDetail.SearchAttributeOwnerCategory"];
            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                documentDataReturn documentDataReturn = Document.GetDocumentData(searchDocumentReturn.RESULTS.Select(x => x.IDDOCUMENT).FirstOrDefault());

                seDocumentDetailOut.result = new SEDocumentDetailVM()
                {
                    centerId = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == "ATR06").FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),
                    center = "0125",
                    courseId = "0125",
                    course = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == "ATR03").FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),
                    initials = "0125",
                    registration = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == "ATR02").FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),                    
                    cpf = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == "ATR04").FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),                   
                    name = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == "ATR01").FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),                    
                };
            }
            else
            {
                seDocumentDetailOut.result = null;
            }

            return seDocumentDetailOut;
        }
    }
}
