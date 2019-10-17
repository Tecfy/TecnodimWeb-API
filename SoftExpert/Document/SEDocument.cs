using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using RegisterEvent.Events;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEDocument
    {
        #region .: Attributes :.        

        private static readonly string searchAttributePendingValue = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingValue"];
        private static readonly string searchAttributePendingName = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingName"];
        private static readonly string searchAttributePendingCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingCategory"];
        private static readonly string messageDeleteDocument = WebConfigurationManager.AppSettings["SoftExpert.MessageDeleteDocument"];
        private static readonly SEClient seClient = SEConnection.GetConnection();

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

        public static ECMDocumentsOut GetSEDocuments(ECMDocumentsIn ecmDocumentsIn)
        {
            Events events = new Events();
            try
            {
                events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - Start", "SoftExpert.SEDocument.GetSEDocuments", "");

                ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();

                attributeData[] attributeDatas = new attributeData[1];
                attributeDatas[0] = new attributeData
                {
                    //search enrollment
                    IDATTRIBUTE = searchAttributePendingName,
                    VLATTRIBUTE = searchAttributePendingValue
                };

                searchDocumentFilter searchDocumentFilter = new searchDocumentFilter { IDCATEGORY = searchAttributePendingCategory };

                events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - Start", "SoftExpert.SEDocument.GetSEDocuments", "seClient.searchDocument");
                searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
                events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - End", "SoftExpert.SEDocument.GetSEDocuments", "seClient.searchDocument");

                if (searchDocumentReturn.RESULTS.Count() > 0)
                {
                    events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - Start", "SoftExpert.SEDocument.GetSEDocuments", string.Format("{0}. Qtd:{1}", "seClient.searchDocument - Reading", searchDocumentReturn.RESULTS.Count()));

                    documentDataReturn documentDataReturn = new documentDataReturn();

                    int import = 100;
                    int.TryParse(ConfigurationManager.AppSettings["Document.Import"], out import);

                    foreach (documentReturn item in searchDocumentReturn.RESULTS.Take(import))
                    {
                        documentDataReturn = new documentDataReturn();

                        documentDataReturn = Common.GetDocumentProperties(item.IDDOCUMENT);

                        try
                        {
                            ECMDocumentsVM ecmDocumentsVM = new ECMDocumentsVM()
                            {
                                documentStatusId = (int)EDocumentStatus.New,
                                externalId = item.IDDOCUMENT,
                                registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                                name = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                                unityCode = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                                unity = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                            };

                            ecmDocumentsOut.result.Add(ecmDocumentsVM);
                        }
                        catch (Exception ex)
                        {
                            events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Erro", "SoftExpert.SEDocument.GetSEDocuments", string.Format("ExternalId: {0} Message: {1} InnerException: {2} Source: {3} StackTrace: {4}", item.IDDOCUMENT, ex.Message, ex.InnerException, ex.Source, ex.StackTrace));
                        }
                    }
                }
                else
                {
                    events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - Start", "SoftExpert.SEDocument.GetSEDocuments", "seClient.searchDocument - Not Reading");

                    ecmDocumentsOut.result = null;
                }

                events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - End", "SoftExpert.SEDocument.GetSEDocuments", "");
                return ecmDocumentsOut;
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Erro", "SoftExpert.SEDocument.GetSEDocuments", ex.Message);
                throw ex;
            }
        }

        public static bool SEDocumentSave(ECMDocumentSaveIn eCMDocumentSaveIn)
        {
            try
            {
                #region .: Connection :.

                string connectionString = WebConfigurationManager.ConnectionStrings["DefaultSesuite"].ConnectionString;

                #endregion

                #region .: Synchronization :.

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("p_Document", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@Registration", SqlDbType.VarChar).Value = eCMDocumentSaveIn.registration;
                        command.Parameters.Add("@Dossier", SqlDbType.VarChar).Value = eCMDocumentSaveIn.externalId;
                        command.Parameters.Add("@Document", SqlDbType.VarChar).Value = eCMDocumentSaveIn.DocumentId;
                        command.Parameters.Add("@Category", SqlDbType.VarChar).Value = eCMDocumentSaveIn.categoryId;
                        command.Parameters.Add("@User", SqlDbType.VarChar).Value = eCMDocumentSaveIn.user;
                        command.Parameters.Add("@Title", SqlDbType.VarChar).Value = eCMDocumentSaveIn.title;
                        command.Parameters.Add("@Identifier", SqlDbType.VarChar).Value = eCMDocumentSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.Identifier) ? eCMDocumentSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.Identifier).FirstOrDefault().value : null;
                        DateTime? Competence;
                        if (eCMDocumentSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.Competence))
                        {
                            Competence = Convert.ToDateTime(eCMDocumentSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.Competence).FirstOrDefault().value).Date;
                        }
                        else
                        {
                            Competence = null;
                        }
                        command.Parameters.Add("@Competence", SqlDbType.DateTime).Value = Competence;
                        DateTime? Validity;
                        if (eCMDocumentSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.Validity))
                        {
                            Validity = Convert.ToDateTime(eCMDocumentSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.Validity).FirstOrDefault().value).Date;
                        }
                        else
                        {
                            Validity = null;
                        }
                        command.Parameters.Add("@Validity", SqlDbType.DateTime).Value = Validity;
                        command.Parameters.Add("@DocumentView", SqlDbType.VarChar).Value = eCMDocumentSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.DocumentView) ? eCMDocumentSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.DocumentView).FirstOrDefault().value : null;
                        command.Parameters.Add("@Note", SqlDbType.VarChar).Value = eCMDocumentSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.Note) ? eCMDocumentSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.Note).FirstOrDefault().value : null;
                        command.Parameters.Add("@SliceUser", SqlDbType.VarChar).Value = eCMDocumentSaveIn.sliceUser;
                        command.Parameters.Add("@SliceUserRegistration", SqlDbType.VarChar).Value = eCMDocumentSaveIn.sliceUserRegistration;
                        command.Parameters.Add("@ClassificationUser", SqlDbType.VarChar).Value = eCMDocumentSaveIn.classificationUser;
                        command.Parameters.Add("@ClassificationUserRegistration", SqlDbType.VarChar).Value = eCMDocumentSaveIn.classificationUserRegistration;
                        command.Parameters.Add("@SliceDate", SqlDbType.DateTime).Value = Convert.ToDateTime(eCMDocumentSaveIn.sliceDate).Date;
                        command.Parameters.Add("@SliceTime", SqlDbType.Decimal).Value = (Convert.ToDateTime(eCMDocumentSaveIn.sliceDate).TimeOfDay.TotalHours * 60);
                        command.Parameters.Add("@ClassificationDate", SqlDbType.DateTime).Value = Convert.ToDateTime(eCMDocumentSaveIn.classificationDate).Date;
                        command.Parameters.Add("@ClassificationTime", SqlDbType.Decimal).Value = (Convert.ToDateTime(eCMDocumentSaveIn.classificationDate).TimeOfDay.TotalHours * 60);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        Common.SEDocumentPhysicalFile(eCMDocumentSaveIn.FileBinary, eCMDocumentSaveIn.FileName, eCMDocumentSaveIn.DocumentId, eCMDocumentSaveIn.user, eCMDocumentSaveIn.categoryId);
                    }
                }

                #endregion

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
                    string deleteDocument = seClient.deleteDocument(documentDataReturn.IDCATEGORY, documentDataReturn.IDDOCUMENT, "", messageDeleteDocument);
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
