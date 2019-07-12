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
    public static class SEJobCategory
    {
        #region .: Attributes :.

        private static readonly string searchAttributeOwnerCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"];
        private static readonly string jobCategory = WebConfigurationManager.AppSettings["SoftExpert.Category.JobCategory"];
        private static readonly string messageDeleteDocument = WebConfigurationManager.AppSettings["SoftExpert.MessageDeleteDocument"];
        private static readonly SEClient seClient = SEConnection.GetConnection();

        #endregion

        #region .: Public Methods :.

        public static ECMJobCategoryOut GetSEJobCategory(ECMJobCategoryIn eCMJobCategoryIn)
        {
            ECMJobCategoryOut eCMJobCategoryOut = new ECMJobCategoryOut();

            eletronicFile[] eletronicFiles = seClient.downloadEletronicFile(eCMJobCategoryIn.externalId, "", "", "", eCMJobCategoryIn.categoryId, "", "", "1");
            if (eletronicFiles.Any(x => string.IsNullOrEmpty(x.ERROR)))
            {
                eCMJobCategoryOut.result = new ECMJobCategoryVM()
                {
                    archive = Encoding.ASCII.GetString(eletronicFiles.FirstOrDefault().BINFILE),
                };
            }
            else
            {
                throw new Exception(eletronicFiles.Where(x => !string.IsNullOrEmpty(x.ERROR)).FirstOrDefault().ERROR);
            }

            return eCMJobCategoryOut;
        }

        public static bool SEDocumentDeleteOldSaveNew(ECMJobCategorySaveIn ecmJobCategorySaveIn)
        {
            try
            {
                //Check if there is a registered owner document
                documentReturn documentReturnOwner = Common.CheckRegisteredDocument(ecmJobCategorySaveIn.registration, searchAttributeOwnerCategory);
                if (documentReturnOwner == null)
                {
                    throw new Exception(i18n.Resource.StudentNotFound);
                }

                //Checks whether the document exists
                documentDataReturn documentDataReturn = Common.GetDocumentProperties(ecmJobCategorySaveIn.DocumentId);

                //If the document already exists in the specified category, it deletes the document to re-create it
                if (documentDataReturn.IDDOCUMENT == ecmJobCategorySaveIn.DocumentId)
                {
                    seClient.deleteDocument(jobCategory, ecmJobCategorySaveIn.DocumentId, "", messageDeleteDocument);
                }

                //Insert a new document
                documentDataReturn documentDataReturnOwner = Common.GetDocumentProperties(ecmJobCategorySaveIn.registration);
                if (documentDataReturnOwner.ATTRIBUTTES.Count() > 0)
                {
                    string message = seClient.newDocument(jobCategory, ecmJobCategorySaveIn.DocumentId, ecmJobCategorySaveIn.title, "", "", "", "", null, 0, null);

                    string[] documentMatrix = message.Split(':');

                    if (documentMatrix.Count() > 0)
                    {
                        if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                        {
                            Common.SEDocumentDataSave(ecmJobCategorySaveIn, documentDataReturnOwner);
                        }
                        else
                        {
                            throw new Exception(message);
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

        public static bool SEDocumentSave(ECMJobSaveIn eCMJobSaveIn)
        {
            try
            {
                //Checks whether the document exists
                documentDataReturn documentDataReturnOwner = Common.GetDocumentProperties(eCMJobSaveIn.registration);

                if (!string.IsNullOrEmpty(documentDataReturnOwner.ERROR))
                {
                    throw new Exception(documentDataReturnOwner.ERROR);
                }

                documentDataReturn documentDataReturn = Common.GetDocumentProperties(eCMJobSaveIn.DocumentId);

                //If the document already exists in the specified category, it uploads the document and properties
                if (documentDataReturn.IDDOCUMENT == eCMJobSaveIn.DocumentId)
                {
                    Common.SEDocumentDataSave(eCMJobSaveIn, documentDataReturnOwner);
                }
                //If you do not insert a new document
                else
                {
                    string message = seClient.newDocument(eCMJobSaveIn.categoryId, eCMJobSaveIn.DocumentId, eCMJobSaveIn.title, "", "", "", eCMJobSaveIn.user, null, 0, null);

                    string[] documentMatrix = message.Split(':');

                    if (documentMatrix.Count() > 0)
                    {
                        if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                        {
                            Common.SEDocumentDataSave(eCMJobSaveIn, documentDataReturnOwner);
                        }
                        else
                        {
                            throw new Exception(message);
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
    }
}
