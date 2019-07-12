using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEDocument
    {
        #region .: Attributes :.

        readonly static string searchAttributePendingValue = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingValue"];
        readonly static string searchAttributePendingName = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingName"];
        readonly static string searchAttributePendingCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingCategory"];
        readonly static string searchAttributeOwnerCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"];
        readonly static string messageDeleteDocument = WebConfigurationManager.AppSettings["SoftExpert.MessageDeleteDocument"];
        readonly static SEClient seClient = SEConnection.GetConnection();

        #endregion

        #region .: Public Methods :.

        public static string GetSEDocument(ECMDocumentIn ecmDocumentIn)
        {
            eletronicFile[] eletronicFiles = seClient.downloadEletronicFile(ecmDocumentIn.externalId, "", "", "", ecmDocumentIn.categoryId, "", "", "1");
            if (!eletronicFiles.Any(x => x.ERROR != null))
            {
                return Encoding.ASCII.GetString(eletronicFiles.FirstOrDefault().BINFILE);
            }
            else
            {
                throw new Exception(eletronicFiles.FirstOrDefault().ERROR);
            }
        }

        public static ECMDocumentsOut GetSEDocuments()
        {
            ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();

            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = searchAttributePendingName,
                VLATTRIBUTE = searchAttributePendingValue
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter { IDCATEGORY = searchAttributePendingCategory };
            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                documentDataReturn documentDataReturn = new documentDataReturn();

                foreach (var item in searchDocumentReturn.RESULTS)
                {
                    documentDataReturn = new documentDataReturn();

                    documentDataReturn = Common.GetDocumentProperties(item.IDDOCUMENT);

                    ecmDocumentsOut.result.Add(new ECMDocumentsVM()
                    {
                        documentStatusId = (int)EDocumentStatus.New,
                        externalId = item.IDDOCUMENT,
                        registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        name = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        unity = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    });
                }
            }
            else
            {
                ecmDocumentsOut.result = null;
            }

            return ecmDocumentsOut;
        }

        public static bool SEDocumentSave(ECMDocumentSaveIn eCMDocumentSaveIn)
        {
            try
            {
                //Check if there is a registered owner document
                documentReturn documentReturnOwner = Common.CheckRegisteredDocument(eCMDocumentSaveIn.registration, searchAttributeOwnerCategory);
                if (documentReturnOwner == null)
                {
                    throw new Exception(i18n.Resource.StudentNotFound);
                }

                //Checks whether the document exists
                documentDataReturn documentDataReturn = Common.GetDocumentProperties(eCMDocumentSaveIn.DocumentId);
                documentDataReturn documentDataReturnOwner = Common.GetDocumentProperties(eCMDocumentSaveIn.registration);

                if (documentDataReturnOwner.ATTRIBUTTES.Count() > 0)
                {
                    //If the document already exists in the specified category, it uploads the document and properties 
                    if (documentDataReturn.IDDOCUMENT == eCMDocumentSaveIn.DocumentId)
                    {
                        Common.SEDocumentDataSave(eCMDocumentSaveIn, documentDataReturnOwner, documentDataReturn);
                    }
                    //If you do not insert a new document
                    else
                    {
                        var document = seClient.newDocument(eCMDocumentSaveIn.categoryId, eCMDocumentSaveIn.DocumentId, eCMDocumentSaveIn.title, "", "", "", eCMDocumentSaveIn.user, null, 0, null);

                        var documentMatrix = document.Split(':');

                        if (documentMatrix.Count() > 0)
                        {
                            if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                            {
                                Common.SEDocumentDataSave(eCMDocumentSaveIn, documentDataReturnOwner, documentDataReturn);
                            }
                            else
                            {
                                throw new Exception(string.Format("Method: newDocument. Message: {0}", document));
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception(i18n.Resource.StudentNotFound);
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static bool SEDocumentDelete(string documentId)
        {
            try
            {
                //Checks whether the document exists
                documentDataReturn documentDataReturn = Common.GetDocumentProperties(documentId);

                //If the document already exists in the specified category, it deletes the document
                if (documentDataReturn.IDDOCUMENT == documentId)
                {
                    var deleteDocument = seClient.deleteDocument(documentDataReturn.IDCATEGORY, documentDataReturn.IDDOCUMENT, "", messageDeleteDocument);
                }
                else if (!string.IsNullOrEmpty(documentDataReturn.ERROR))
                {
                    throw new Exception(documentDataReturn.ERROR);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
