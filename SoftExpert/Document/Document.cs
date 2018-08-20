using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Linq;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class Document
    {
        public static documentDataReturn GetDocumentData(string documentid)
        {
            SEClient seClient = Connection.GetConnection();
            return seClient.viewDocumentData(documentid, "", "");
        }

        public static SEDocumentOut GetSEDocument(SEDocumentIn seDocumentIn)
        {
            SEDocumentOut seDocumentOut = new SEDocumentOut();

            SEClient seClient = Connection.GetConnection();

            eletronicFile[] eletronicFiles = seClient.downloadEletronicFile(seDocumentIn.externalId, "", "", "", "", "");
            if (eletronicFiles.Count() > 0)
            {
                seDocumentOut.result = new SEDocumentVM()
                {
                    archive = System.Convert.ToBase64String(eletronicFiles.FirstOrDefault().BINFILE),
                    hash = seDocumentIn.hash
                };
            }
            else
            {
                seDocumentOut.result = null;
            }

            return seDocumentOut;
        }

        public static SEDocumentsOut GetSEDocuments()
        {
            SEDocumentsOut seDocumentsOut = new SEDocumentsOut();

            SEClient seClient = Connection.GetConnection();
            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.Document.SearchAttributePendingName"],
                VLATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.Document.SearchAttributePendingValue"]
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter();
            searchDocumentFilter.IDCATEGORY = WebConfigurationManager.AppSettings["SoftExpert.Document.SearchAttributePendingCategory"];
            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                documentDataReturn documentDataReturn = new documentDataReturn();

                foreach (var item in searchDocumentReturn.RESULTS)
                {
                    documentDataReturn = new documentDataReturn();

                    documentDataReturn = Document.GetDocumentData(item.IDDOCUMENT);

                    seDocumentsOut.result.Add(new SEDocumentsVM()
                    {
                        documentStatusId = (int)EDocumentStatus.New,
                        externalId = item.IDDOCUMENT,
                        registration = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == "ATR02").FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),
                        name = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == "ATR01").FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),
                        hash = Guid.NewGuid(),
                    });
                }
            }
            else
            {
                seDocumentsOut.result = null;
            }

            return seDocumentsOut;
        }
    }
}
