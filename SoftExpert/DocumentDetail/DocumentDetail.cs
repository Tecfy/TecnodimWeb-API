using Helper.Enum;
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
                    unity = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_Unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),
                    course = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_CURSO.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),
                    registration = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),                    
                    cpf = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_CPF.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),                   
                    name = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),                    
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
