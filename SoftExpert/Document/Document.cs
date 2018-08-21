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
        #region .: Public Methods :.

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
                        registration = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),
                        name = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault(),
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

        public static documentReturn GetSEDocumentByRegistrationAndCategory(string registration, string category)
        {
            SEClient seClient = Connection.GetConnection();
            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.Document.SearchAttributeOwnerRegistration"],
                VLATTRIBUTE = registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter();
            searchDocumentFilter.IDCATEGORY = category;
            var d = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (d.RESULTS.Count() > 0)
            {
                return d.RESULTS[0];
            }
            else
            {
                return null;
            }
        }

        public static bool SEDocumentSave(SEDocumentSaveIn seDocumentSaveIn)
        {
            bool retorno = false;
            try
            {
                SEClient seClient = Connection.GetConnection();

                //Check if there is a registered owner document
                documentReturn documentReturnOwner = GetSEDocumentByRegistrationAndCategory(seDocumentSaveIn.registration, WebConfigurationManager.AppSettings["SoftExpert.Document.SearchAttributeOwnerCategory"]);

                if (documentReturnOwner == null)
                {
                    throw new Exception(i18n.Resource.StudentNotFound);
                }

                //Checks whether the document exists
                documentReturn documentReturn = GetSEDocumentByRegistrationAndCategory(seDocumentSaveIn.registration, seDocumentSaveIn.categoryId);

                //If the document already exists in the specified category, it uploads the document and properties
                if (documentReturn != null)
                {
                    documentDataReturn documentDataReturn = GetDocumentData(documentReturnOwner.IDDOCUMENT);
                    if (documentDataReturn.ATTRIBUTTES.Count() > 0)
                    {
                        foreach (var item in documentDataReturn.ATTRIBUTTES)
                        {
                            string value = "";
                            if (item.ATTRIBUTTEVALUE.Count() > 0)
                            {
                                value = item.ATTRIBUTTEVALUE[0];
                            }
                            try
                            {
                                var s = seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", item.ATTRIBUTTENAME, value);
                            }
                            catch (Exception)
                            {
                                throw new Exception(string.Format(i18n.Resource.FieldWithError, item.ATTRIBUTTENAME));
                            }
                        }
                        var ds = SEDocumentUpload(seDocumentSaveIn, documentReturn.IDDOCUMENT);
                    }
                }

                //If you do not insert a new document
                else
                {
                    documentDataReturn documentDataReturn = GetDocumentData(documentReturnOwner.IDDOCUMENT);
                    if (documentDataReturn.ATTRIBUTTES.Count() > 0)
                    {
                        string atributos = "";
                        foreach (var item in documentDataReturn.ATTRIBUTTES)
                        {
                            string valor = "";
                            if (item.ATTRIBUTTEVALUE.Count() > 0)
                            {
                                valor = item.ATTRIBUTTEVALUE[0];
                            }

                            atributos += item.ATTRIBUTTENAME + "=" + valor + ";";
                        }

                        var document = seClient.newDocument(seDocumentSaveIn.category, "", seDocumentSaveIn.category, "", "", atributos, "", null, 0);

                        var documentMatrix = document.Split(':');

                        if (documentMatrix.Count() > 0)
                        {
                            if (documentMatrix[1].ToUpper().Contains("SUCESSO"))
                            {
                                SEDocumentUpload(seDocumentSaveIn, documentMatrix[0]);
                            }
                            else
                            {
                                throw new Exception(document);
                            }
                        }
                    }
                }

                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region .: Private Methods :.

        private static bool SEDocumentUpload(SEDocumentSaveIn seDocumentSaveIn, string documentid)
        {
            try
            { 
                SEClient seClient = Connection.GetConnection();

                eletronicFile[] eletronicFiles = new eletronicFile[2];
                eletronicFiles[0] = new eletronicFile
                {
                    BINFILE = seDocumentSaveIn.archive,
                    ERROR = "",
                    NMFILE = seDocumentSaveIn.archiveName
                };

                seClient.uploadEletronicFileAsync(documentid, "", "", eletronicFiles);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
