using Helper.Enum;
using Model.In;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Linq;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEJobCategory
    {
        #region .: Public Methods :.

        public static bool SEJobCategorySave(ECMJobCategorySaveIn ecmJobCategorySaveIn)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                string prefix = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePrefixReplicate"];

                //Check if there is a registered owner document
                documentReturn documentReturnOwner = SEDocument.GetSEDocumentByRegistrationAndCategory(ecmJobCategorySaveIn.registration, WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"]);

                if (documentReturnOwner == null)
                {
                    throw new Exception(i18n.Resource.StudentNotFound);
                }

                //Checks whether the document exists
                documentReturn documentReturn = SEDocument.GetSEDocumentByRegistrationAndCategory(ecmJobCategorySaveIn.registration, ecmJobCategorySaveIn.categoryId);

                //If the document already exists in the specified category, it uploads the document and properties
                if (documentReturn != null)
                {
                    var ds = seClient.deleteDocument(ecmJobCategorySaveIn.categoryId, documentReturn.IDDOCUMENT, "", WebConfigurationManager.AppSettings["SoftExpert.MessageDeleteDocument"]);
                }

                //If you do not insert a new document

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

                    try
                    {
                        atributos += EAttribute.MFP_Data_Job.ToString() + "=" + ecmJobCategorySaveIn.dataJob.ToString("yyyy-MM-dd") + ";";
                        atributos += EAttribute.MFP_Status.ToString() + "=" + WebConfigurationManager.AppSettings["SoftExpert.JobStatusInitial"] + ";";
                        atributos += EAttribute.MFP_Categoria.ToString() + "=" + ecmJobCategorySaveIn.categoryId + ";";
                        atributos += EAttribute.MFP_Usuario.ToString() + "=" + ecmJobCategorySaveIn.user + ";";
                    }
                    catch
                    {
                    }

                    var document = seClient.newDocument(WebConfigurationManager.AppSettings["SoftExpert.JobCategory"], ecmJobCategorySaveIn.code.Trim(), ecmJobCategorySaveIn.title, "", "", atributos, "", null, 0);

                    var documentMatrix = document.Split(':');

                    if (documentMatrix.Count() > 0)
                    {
                        if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                        {
                            SEDocumentUpload(ecmJobCategorySaveIn, ecmJobCategorySaveIn.code.Trim());
                        }
                        else
                        {
                            throw new Exception(document);
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

        private static bool SEDocumentUpload(ECMJobCategorySaveIn ecmJobCategorySaveIn, string documentid)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                eletronicFile[] eletronicFiles = new eletronicFile[2];
                eletronicFiles[0] = new eletronicFile
                {
                    BINFILE = Convert.FromBase64String(ecmJobCategorySaveIn.archive),
                    ERROR = "",
                    NMFILE = ecmJobCategorySaveIn.title
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
