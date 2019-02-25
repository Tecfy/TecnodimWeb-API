using Helper.Enum;
using Model.In;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class Common
    {
        #region .: Attributes :.

        readonly static bool physicalFile = Convert.ToBoolean(WebConfigurationManager.AppSettings["Sesuite.Folder.Physical"]);
        readonly static string physicalPath = WebConfigurationManager.AppSettings["Sesuite.Physical.Path"];
        readonly static string physicalPathSE = WebConfigurationManager.AppSettings["Sesuite.Physical.Path.SE"];
        readonly static string jobStatusInitial = WebConfigurationManager.AppSettings["SoftExpert.JobStatusInitial"];
        readonly static string jobCategory = WebConfigurationManager.AppSettings["SoftExpert.JobCategory"];
        readonly static string prefix = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePrefixReplicate"];
        readonly static string searchAttributeOwnerCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"];
        readonly static string searchAttributeOwnerRegistration = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerRegistration"];
        readonly static int newAccessPermissionUserType = int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.UserType"].ToString());
        readonly static string newAccessPermissionPermission = WebConfigurationManager.AppSettings["NewAccessPermission.Permission"].ToString();
        readonly static int newAccessPermissionPermissionType = int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.PermissionType"].ToString());
        readonly static string newAccessPermissionFgaddLowerLevel = WebConfigurationManager.AppSettings["NewAccessPermission.FgaddLowerLevel"].ToString();
        readonly static string structID = WebConfigurationManager.AppSettings["SoftExpert.StructID"];
        readonly static SEClient seClient = SEConnection.GetConnection();
        readonly static SEAdministration seAdministration = SEConnection.GetConnectionAdm();

        #endregion

        #region .: Private Methods :.        

        public static documentDataReturn GetDocumentProperties(string documentid)
        {
            return seClient.viewDocumentData(documentid, "", "");
        }

        public static documentReturn CheckRegisteredDocument(string registration, string category)
        {
            try
            {
                attributeData[] attributes = new attributeData[1];
                attributes[0] = new attributeData { IDATTRIBUTE = searchAttributeOwnerRegistration, VLATTRIBUTE = registration };

                searchDocumentFilter searchDocumentFilter = new searchDocumentFilter { IDCATEGORY = category };

                searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributes);

                if (searchDocumentReturn.RESULTS.Count() > 0)
                    return searchDocumentReturn.RESULTS[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SEDocumentDataSave(ECMDocumentSaveIn eCMDocumentSaveIn, documentDataReturn documentDataReturnOwner)
        {
            try
            {
                SEDocumentDataSaveAttributtes(eCMDocumentSaveIn.DocumentId, documentDataReturnOwner);
                SEDocumentDataSaveAttributtesSpecific(eCMDocumentSaveIn.DocumentId, eCMDocumentSaveIn.additionalFields);
                SEDocumentDataSaveAttributtesSpecificSlice(eCMDocumentSaveIn.DocumentId, eCMDocumentSaveIn.sliceUser, eCMDocumentSaveIn.sliceUserRegistration, eCMDocumentSaveIn.classificationUser, eCMDocumentSaveIn.classificationUserRegistration);
                SEDocumentDataSavePermission(eCMDocumentSaveIn.DocumentId, documentDataReturnOwner);
                SEDocumentDataSaveAssociation(documentDataReturnOwner.IDDOCUMENT, searchAttributeOwnerCategory, eCMDocumentSaveIn.DocumentId, eCMDocumentSaveIn.categoryId);
                SEDocumentDataSaveUploadFile(eCMDocumentSaveIn.FileBinary, eCMDocumentSaveIn.FileName, eCMDocumentSaveIn.DocumentId, eCMDocumentSaveIn.user, eCMDocumentSaveIn.categoryId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SEDocumentDataSave(ECMJobSaveIn eCMJobSaveIn, documentDataReturn documentDataReturnOwner)
        {
            try
            {
                SEDocumentDataSaveAttributtes(eCMJobSaveIn.DocumentId, documentDataReturnOwner);
                SEDocumentDataSaveAttributtesSpecific(eCMJobSaveIn.DocumentId, eCMJobSaveIn.additionalFields);
                SEDocumentDataSavePermission(eCMJobSaveIn.DocumentId, documentDataReturnOwner);
                SEDocumentDataSaveAssociation(documentDataReturnOwner.IDDOCUMENT, searchAttributeOwnerCategory, eCMJobSaveIn.DocumentId, eCMJobSaveIn.categoryId);
                SEDocumentDataSaveUploadFile(eCMJobSaveIn.FileBinary, eCMJobSaveIn.FileName, eCMJobSaveIn.DocumentId, eCMJobSaveIn.user, eCMJobSaveIn.categoryId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SEDocumentDataSave(ECMJobCategorySaveIn eCMJobCategorySaveIn, documentDataReturn documentDataReturnOwner)
        {
            try
            {
                SEDocumentDataSaveAttributtes(eCMJobCategorySaveIn.DocumentId, documentDataReturnOwner);
                SEDocumentDataSaveAttributtesSpecificJob(eCMJobCategorySaveIn.DocumentId, eCMJobCategorySaveIn.dataJob, eCMJobCategorySaveIn.categoryId, eCMJobCategorySaveIn.user);
                SEDocumentDataSavePermission(eCMJobCategorySaveIn.DocumentId, documentDataReturnOwner);
                SEDocumentDataSaveAssociation(documentDataReturnOwner.IDDOCUMENT, searchAttributeOwnerCategory, eCMJobCategorySaveIn.DocumentId, jobCategory);
                SEDocumentDataSaveUploadFile(eCMJobCategorySaveIn.FileBinary, eCMJobCategorySaveIn.FileName, eCMJobCategorySaveIn.DocumentId, eCMJobCategorySaveIn.user, jobCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SEDocumentDataSaveAttributtes(string documentId, documentDataReturn documentDataReturnOwner)
        {
            #region .: Insert Attibuttes Owner :.

            foreach (var item in documentDataReturnOwner.ATTRIBUTTES)
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
                        seClient.setAttributeValue(documentId, "", item.ATTRIBUTTENAME, value);
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format(i18n.Resource.FieldWithError, item.ATTRIBUTTENAME));
                    }
                }
            }

            #endregion
        }

        private static void SEDocumentDataSaveAttributtesSpecific(string documentId, List<ECMAdditionalFieldSaveIn> additionalFields)
        {
            #region .: Insert Attibuttes Specific :.

            foreach (var item in additionalFields)
            {
                try
                {
                    if (item.additionalFieldId == (int)EAdditionalField.Identifier)
                    {
                        seClient.setAttributeValue(documentId, "", EAttribute.SER_Input_NumDoc.ToString(), item.value);
                    }
                    else if (item.additionalFieldId == (int)EAdditionalField.Competence)
                    {
                        DateTime competence = DateTime.MinValue;
                        DateTime.TryParse(item.value, out competence);

                        seClient.setAttributeValue(documentId, "", EAttribute.SER_Input_DataRef.ToString(), competence.ToString("yyyy-MM-dd"));
                    }
                    else if (item.additionalFieldId == (int)EAdditionalField.Validity)
                    {
                        DateTime validity = DateTime.MinValue;
                        DateTime.TryParse(item.value, out validity);

                        seClient.setAttributeValue(documentId, "", EAttribute.SER_Input_Data_Vencto.ToString(), validity.ToString("yyyy-MM-dd"));
                    }
                    else if (item.additionalFieldId == (int)EAdditionalField.DocumentView)
                    {
                        seClient.setAttributeValue(documentId, "", EAttribute.SER_Input_Compl.ToString(), item.value);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            #endregion
        }

        private static void SEDocumentDataSaveAttributtesSpecificJob(string documentId, DateTime dataJob, string categoryId, string user)
        {
            #region .: Insert Attibuttes Specific :.

            try
            {
                seClient.setAttributeValue(documentId, "", EAttribute.MFP_Data_Job.ToString(), dataJob.ToString("yyyy-MM-dd"));
                seClient.setAttributeValue(documentId, "", EAttribute.MFP_Status.ToString(), jobStatusInitial);
                seClient.setAttributeValue(documentId, "", EAttribute.MFP_Categoria.ToString(), categoryId);
                seClient.setAttributeValue(documentId, "", EAttribute.MFP_Usuario.ToString(), user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            #endregion
        }

        private static void SEDocumentDataSaveAttributtesSpecificSlice(string documentId, string sliceUser, string sliceUserRegistration, string classificationUser, string classificationUserRegistration)
        {
            #region .: Insert Attibuttes Specific :.

            try
            {
                seClient.setAttributeValue(documentId, "", EAttribute.SER_id_classificador.ToString(), classificationUserRegistration);
                seClient.setAttributeValue(documentId, "", EAttribute.SER_id_recortador.ToString(), sliceUserRegistration);
                seClient.setAttributeValue(documentId, "", EAttribute.SER_nome_classificador.ToString(), classificationUser);
                seClient.setAttributeValue(documentId, "", EAttribute.SER_nome_recortador.ToString(), sliceUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            #endregion
        }

        private static void SEDocumentDataSavePermission(string documentId, documentDataReturn documentDataReturnOwner)
        {
            try
            {
                #region .: Insert Permission :.

                if (documentDataReturnOwner.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()))
                {
                    string unityCode = documentDataReturnOwner.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault();

                    seAdministration.newPosition(unityCode, unityCode, out string status, out string detail, out int code, out string recordid, out string recordKey);
                    seAdministration.newDepartment(unityCode, unityCode, unityCode, "", "", "1");

                    seClient.newAccessPermission(documentId, unityCode + ";" + unityCode, newAccessPermissionUserType, newAccessPermissionPermission, newAccessPermissionPermissionType, newAccessPermissionFgaddLowerLevel);
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SEDocumentDataSaveAssociation(string documentIdOwner, string categoryIdOwner, string documentId, string categoryId)
        {
            #region .: Insert Association :.

            try
            {
                seClient.newDocumentContainerAssociation(categoryIdOwner, documentIdOwner, "", structID, categoryId, documentId, out long codeAssociation, out string detailAssociation);
            }
            catch
            { }

            #endregion
        }

        private static void SEDocumentDataSaveUploadFile(byte[] fileBinary, string fileName, string documentId, string user, string categoryId)
        {
            try
            {
                #region .: Upload File :.

                if (physicalFile)
                {
                    SEDocumentPhysicalFile(fileBinary, fileName, documentId, user, categoryId, physicalPath, physicalPathSE);
                }
                else
                {
                    SEDocumentUpload(fileBinary, fileName, documentId, user);
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool SEDocumentUpload(byte[] fileBinary, string fileName, string documentId, string user)
        {
            try
            {
                eletronicFile[] eletronicFiles = new eletronicFile[2];
                eletronicFiles[0] = new eletronicFile
                {
                    BINFILE = fileBinary,
                    ERROR = "",
                    NMFILE = fileName
                };

                var response = seClient.uploadEletronicFile(documentId, "", user, eletronicFiles);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool SEDocumentPhysicalFile(byte[] fileBinary, string fileName, string documentId, string user, string categoryId, string folder, string folderSE)
        {
            try
            {
                #region .: Save File :.

                SaveFile(physicalPath, fileName, fileBinary);

                #endregion

                #region .: Query :.

                string queryStringInsert = @"INSERT INTO ADINTERFACE (CDINTERFACE, FGIMPORT, CDISOSYSTEM, FGOPTION, NMFIELD01, NMFIELD02, NMFIELD03, NMFIELD04, NMFIELD05, NMFIELD07) VALUES((SELECT COALESCE(MAX(CDINTERFACE),0)+1 FROM ADINTERFACE), 1, 73, 97, '{0}','00','{1}','{2}','{3}','{4}')";
                string connectionString = WebConfigurationManager.ConnectionStrings["DefaultSesuite"].ConnectionString;

                #endregion

                #region .: Insert Sesuite :.

                using (SqlConnection connectionInsert = new SqlConnection(connectionString))
                {
                    var queryInsert = string.Format(queryStringInsert,
                        documentId /*Identificador do Documento*/,
                        fileName /*Nome do Arquivo*/,
                        user /*Matrícula do Usuário*/,
                        folderSE + fileName /*Caminho do Arquivo*/,
                        categoryId /*Identificador da categoria*/);

                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionInsert))
                    {
                        connectionInsert.Open();
                        commandInsert.ExecuteNonQuery();
                    }
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SaveFile(string folder, string fileName, byte[] fileBinary)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (File.Exists(Path.Combine(folder, fileName)))
            {
                File.Delete(Path.Combine(folder, fileName));
            }

            File.WriteAllBytes(Path.Combine(physicalPath, fileName), fileBinary);
        }

        #endregion
    }
}
