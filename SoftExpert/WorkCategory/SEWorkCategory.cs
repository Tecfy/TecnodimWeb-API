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
    public static class SEWorkCategory
    {
        #region .: Public Methods :.

        public static bool SEWorkCategorySave(ECMWorkCategorySaveIn seWorkCategorySaveIn)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                string prefix = WebConfigurationManager.AppSettings["SoftExpert.Document.SearchAttributePrefixReplicate"];

                //Check if there is a registered owner document
                documentReturn documentReturnOwner = SEDocument.GetSEDocumentByRegistrationAndCategory(seWorkCategorySaveIn.registration, WebConfigurationManager.AppSettings["SoftExpert.Document.SearchAttributeOwnerCategory"]);

                if (documentReturnOwner == null)
                {
                    throw new Exception(i18n.Resource.StudentNotFound);
                }

                //Checks whether the document exists
                documentReturn documentReturn = SEDocument.GetSEDocumentByRegistrationAndCategory(seWorkCategorySaveIn.registration, seWorkCategorySaveIn.categoryId);

                //If the document already exists in the specified category, it uploads the document and properties
                if (documentReturn != null)
                {
                    documentDataReturn documentDataReturn = SEDocument.GetDocumentData(documentReturnOwner.IDDOCUMENT);
                    if (documentDataReturn.ATTRIBUTTES.Count() > 0)
                    {
                        foreach (var item in documentDataReturn.ATTRIBUTTES)
                        {
                            if (item.ATTRIBUTTENAME.Contains(prefix))
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
                        }

                        var ds = SEDocumentUpload(seWorkCategorySaveIn, documentReturn.IDDOCUMENT);
                    }
                }

                //If you do not insert a new document
                else
                {
                    documentDataReturn documentDataReturn = SEDocument.GetDocumentData(documentReturnOwner.IDDOCUMENT);
                    if (documentDataReturn.ATTRIBUTTES.Count() > 0)
                    {
                        string atributos = "";
                        foreach (var item in documentDataReturn.ATTRIBUTTES)
                        {
                            if (item.ATTRIBUTTENAME.Contains(prefix))
                            {
                                string valor = "";

                                if (item.ATTRIBUTTEVALUE.Count() > 0)
                                {
                                    valor = item.ATTRIBUTTEVALUE[0];
                                }

                                atributos += item.ATTRIBUTTENAME + "=" + valor + ";";
                            }
                        }

                        var document = seClient.newDocument(seWorkCategorySaveIn.categoryId, seWorkCategorySaveIn.registration.Trim() + "-" + seWorkCategorySaveIn.categoryId.Trim(), seWorkCategorySaveIn.title, "", "", atributos, "", null, 0);

                        var documentMatrix = document.Split(':');

                        if (documentMatrix.Count() > 0)
                        {
                            if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                            {
                                SEDocumentUpload(seWorkCategorySaveIn, seWorkCategorySaveIn.registration.Trim() + "-" + seWorkCategorySaveIn.categoryId.Trim());
                            }
                            else
                            {
                                throw new Exception(document);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region .: Private Methods :.

        private static bool SEDocumentUpload(ECMWorkCategorySaveIn ecmWorkCategorySaveIn, string documentid)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                eletronicFile[] eletronicFiles = new eletronicFile[2];
                eletronicFiles[0] = new eletronicFile
                {
                    BINFILE = Convert.FromBase64String(ecmWorkCategorySaveIn.archive),
                    ERROR = "",
                    NMFILE = ecmWorkCategorySaveIn.title
                };

                var response = seClient.uploadEletronicFile(documentid, "", "", eletronicFiles);

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
