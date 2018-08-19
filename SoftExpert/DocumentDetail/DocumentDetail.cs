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
        public static SEDocumentDetailOut GetDocumentDetail(SEDocumentDetailIn seDocumentDetailIn)
        {
            SEDocumentDetailOut seDocumentDetailOut = new SEDocumentDetailOut();

            SEClient seClient = Connection.GetConnection();
            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.Document.DocumentDetail.SearchAttributeRegistration"],
                VLATTRIBUTE = seDocumentDetailIn.registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter();
            searchDocumentFilter.IDCATEGORY = WebConfigurationManager.AppSettings["SoftExpert.Document.DocumentDetail.SearchAttributeCategory"];
            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                documentDataReturn documentDataReturn = Document.GetDocumentData(searchDocumentReturn.RESULTS.Select(x => x.IDDOCUMENT).FirstOrDefault());

                seDocumentDetailOut.result = new SEDocumentDetailVM()
                {
                    centerId = documentDataReturn.ATTRIBUTTES[4].ATTRIBUTTEVALUE[0],
                    center = "0125",
                    courseId = "0125",
                    course = documentDataReturn.ATTRIBUTTES[1].ATTRIBUTTEVALUE[0],
                    initials = "0125",
                    registration = documentDataReturn.ATTRIBUTTES[0].ATTRIBUTTEVALUE[0],
                    cpf = documentDataReturn.ATTRIBUTTES[2].ATTRIBUTTEVALUE[0],
                    name = documentDataReturn.ATTRIBUTTES[5].ATTRIBUTTEVALUE[0]
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
