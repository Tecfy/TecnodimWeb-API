using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEDocument
    {
        #region .: Attributes :.

        readonly static bool physicalFile = Convert.ToBoolean(WebConfigurationManager.AppSettings["Sesuite.Folder.Physical"]);
        readonly static string physicalPath = WebConfigurationManager.AppSettings["Sesuite.Physical.Path"];
        readonly static string username = WebConfigurationManager.AppSettings["Physical.Username"];

        #endregion

        #region .: Public Methods :.

        public static documentDataReturn GetDocumentData(string documentid)
        {
            SEClient seClient = SEConnection.GetConnection();
            return seClient.viewDocumentData(documentid, "", "");
        }

        public static ECMDocumentOut GetSEDocument(ECMDocumentIn ecmDocumentIn)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();

            SEClient seClient = SEConnection.GetConnection();

            eletronicFile[] eletronicFiles = seClient.downloadEletronicFile(ecmDocumentIn.externalId, "", "", "", ecmDocumentIn.categoryId, "", "", "");
            if (eletronicFiles.Count() > 0)
            {
                ecmDocumentOut.result = new ECMDocumentVM()
                {
                    archive = System.Convert.ToBase64String(eletronicFiles.FirstOrDefault().BINFILE),
                };
            }
            else
            {
                ecmDocumentOut.result = null;
            }

            return ecmDocumentOut;
        }

        public static ECMDocumentsOut GetSEDocuments()
        {
            ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();

            SEClient seClient = SEConnection.GetConnection();
            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingName"],
                VLATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingValue"]
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter();
            searchDocumentFilter.IDCATEGORY = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingCategory"];
            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                documentDataReturn documentDataReturn = new documentDataReturn();

                foreach (var item in searchDocumentReturn.RESULTS)
                {
                    documentDataReturn = new documentDataReturn();

                    documentDataReturn = SEDocument.GetDocumentData(item.IDDOCUMENT);

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

        public static bool SEDocumentSave(ECMDocumentSaveIn seDocumentSaveIn)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                string prefix = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePrefixReplicate"];

                //Check if there is a registered owner document
                documentReturn documentReturnOwner = GetSEDocumentByRegistrationAndCategory(seDocumentSaveIn.registration, WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"]);

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

                        foreach (var item in seDocumentSaveIn.additionalFields)
                        {
                            try
                            {
                                if (item.additionalFieldId == (int)EAdditionalField.Identifier)
                                {
                                    var retorno = seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_Input_NumDoc.ToString(), item.value);
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.Competence)
                                {
                                    DateTime competence = DateTime.MinValue;
                                    DateTime.TryParse(item.value, out competence);

                                    var retorno = seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_Input_DataRef.ToString(), competence.ToString("yyyy-MM-dd"));
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.Validity)
                                {
                                    DateTime validity = DateTime.MinValue;
                                    DateTime.TryParse(item.value, out validity);

                                    var retorno = seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_Input_Data_Vencto.ToString(), validity.ToString("yyyy-MM-dd"));
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.DocumentView)
                                {
                                    var retorno = seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_Input_Compl.ToString(), item.value);
                                }
                            }
                            catch
                            {
                            }
                        }

                        if (physicalFile)
                        {
                            SEDocumentPhysicalFile(seDocumentSaveIn, documentReturn.IDDOCUMENT);
                        }
                        else
                        {
                            SEDocumentUpload(seDocumentSaveIn, documentReturn.IDDOCUMENT);
                        }

                        if (documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()))
                        {
                            string unityCode = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault();

                            SEAdministration seAdministration = SEConnection.GetConnectionAdm();

                            seAdministration.newPosition(unityCode, unityCode, out string status, out string detail, out int code, out string recordid, out string recordKey);
                            seAdministration.newDepartment(unityCode, unityCode, unityCode, "", "", "1");

                            var n = seClient.newAccessPermission(seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(),
                                    unityCode + ";" + unityCode,
                                    int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.UserType"].ToString()),
                                    WebConfigurationManager.AppSettings["NewAccessPermission.Permission"].ToString(),
                                    int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.PermissionType"].ToString()),
                                    WebConfigurationManager.AppSettings["NewAccessPermission.FgaddLowerLevel"].ToString());
                        }

                        try
                        {
                            string returnAssociation = seClient.newDocumentContainerAssociation(WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"], documentReturnOwner.IDDOCUMENT, "", WebConfigurationManager.AppSettings["SoftExpert.StructID"], seDocumentSaveIn.categoryId, seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(), out long codeAssociation, out string detailAssociation);
                        }
                        catch
                        { }
                    }
                }

                //If you do not insert a new document
                else
                {
                    documentDataReturn documentDataReturn = GetDocumentData(documentReturnOwner.IDDOCUMENT);
                    if (documentDataReturn.ATTRIBUTTES.Count() > 0)
                    {
                        var document = seClient.newDocument(seDocumentSaveIn.categoryId, seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(), seDocumentSaveIn.title, "", "", "", "", null, 0);

                        var documentMatrix = document.Split(':');

                        if (documentMatrix.Count() > 0)
                        {
                            if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                            {
                                if (physicalFile)
                                {
                                    SEDocumentPhysicalFile(seDocumentSaveIn, seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim());
                                }
                                else
                                {
                                    SEDocumentUpload(seDocumentSaveIn, seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim());
                                }
                            }
                            else
                            {
                                throw new Exception(document);
                            }
                        }

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
                                    var s = seClient.setAttributeValue(seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(), "", item.ATTRIBUTTENAME, value);
                                }
                                catch (Exception)
                                {
                                    throw new Exception(string.Format(i18n.Resource.FieldWithError, item.ATTRIBUTTENAME));
                                }
                            }
                        }

                        foreach (var item in seDocumentSaveIn.additionalFields)
                        {
                            try
                            {
                                if (item.additionalFieldId == (int)EAdditionalField.Identifier)
                                {
                                    var retorno = seClient.setAttributeValue(seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(), "", EAttribute.SER_Input_NumDoc.ToString(), item.value);
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.Competence)
                                {
                                    DateTime competence = DateTime.MinValue;
                                    DateTime.TryParse(item.value, out competence);

                                    var retorno = seClient.setAttributeValue(seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(), "", EAttribute.SER_Input_DataRef.ToString(), competence.ToString("yyyy-MM-dd"));
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.Validity)
                                {
                                    DateTime validity = DateTime.MinValue;
                                    DateTime.TryParse(item.value, out validity);

                                    var retorno = seClient.setAttributeValue(seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(), "", EAttribute.SER_Input_Data_Vencto.ToString(), validity.ToString("yyyy-MM-dd"));
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.DocumentView)
                                {
                                    var retorno = seClient.setAttributeValue(seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(), "", EAttribute.SER_Input_Compl.ToString(), item.value);
                                }
                            }
                            catch (Exception)
                            {
                                throw new Exception(string.Format(i18n.Resource.FieldWithError, item.additionalFieldId));
                            }
                        }

                        if (documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()))
                        {
                            string unityCode = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault();

                            SEAdministration seAdministration = SEConnection.GetConnectionAdm();

                            seAdministration.newPosition(unityCode, unityCode, out string status, out string detail, out int code, out string recordid, out string recordKey);
                            seAdministration.newDepartment(unityCode, unityCode, unityCode, "", "", "1");

                            var n = seClient.newAccessPermission(seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(),
                                    unityCode + ";" + unityCode,
                                    int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.UserType"].ToString()),
                                    WebConfigurationManager.AppSettings["NewAccessPermission.Permission"].ToString(),
                                    int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.PermissionType"].ToString()),
                                    WebConfigurationManager.AppSettings["NewAccessPermission.FgaddLowerLevel"].ToString());
                        }

                        try
                        {
                            string returnAssociation = seClient.newDocumentContainerAssociation(WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"], documentReturnOwner.IDDOCUMENT, "", WebConfigurationManager.AppSettings["SoftExpert.StructID"], seDocumentSaveIn.categoryId, seDocumentSaveIn.registration.Trim() + "-" + seDocumentSaveIn.categoryId.Trim(), out long codeAssociation, out string detailAssociation);
                        }
                        catch
                        { }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static documentReturn GetSEDocumentByRegistrationAndCategory(string registration, string category)
        {
            SEClient seClient = SEConnection.GetConnection();
            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerRegistration"],
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

        #endregion

        #region .: Private Methods :.

        private static bool SEDocumentUpload(ECMDocumentSaveIn seDocumentSaveIn, string documentid)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                eletronicFile[] eletronicFiles = new eletronicFile[2];
                eletronicFiles[0] = new eletronicFile
                {
                    BINFILE = Convert.FromBase64String(seDocumentSaveIn.archive),
                    ERROR = "",
                    NMFILE = seDocumentSaveIn.title
                };

                var response = seClient.uploadEletronicFile(documentid, "", "", eletronicFiles);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SEDocumentPhysicalFile(ECMDocumentSaveIn seDocumentSaveIn, string documentid)
        {
            try
            {
                #region .: Save File :.

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                if (File.Exists(Path.Combine(physicalPath, Path.GetFileName(seDocumentSaveIn.title))))
                {
                    File.Delete(Path.Combine(physicalPath, Path.GetFileName(seDocumentSaveIn.title)));
                }

                File.WriteAllBytes(Path.Combine(physicalPath, Path.GetFileName(seDocumentSaveIn.title)), Convert.FromBase64String(seDocumentSaveIn.archive));

                #endregion

                #region .: Query :.

                string queryStringInsert = @"INSERT INTO ADINTERFACE (CDINTERFACE, FGIMPORT, CDISOSYSTEM, FGOPTION, NMFIELD01, NMFIELD02, NMFIELD03, NMFIELD04, NMFIELD05, NMFIELD07) VALUES((SELECT COALESCE(MAX(CDINTERFACE),0)+1 FROM ADINTERFACE), 1, 73, 97, '{0}','00','{1}','{2}','{3}','{4}')";
                string connectionString = WebConfigurationManager.ConnectionStrings["DefaultSesuite"].ConnectionString;

                #endregion

                #region .: Insert Sesuite :.

                using (SqlConnection connectionInsert = new SqlConnection(connectionString))
                {
                    var queryInsert = string.Format(queryStringInsert,
                                                   documentid, //Identificador do Documento
                                                   Path.GetFileName(seDocumentSaveIn.title), //Nome do Arquivo
                                                   username, //Matrícula do Usuário
                                                   physicalPath, //Caminho do Arquivo
                                                   seDocumentSaveIn.categoryId.Trim() //Identificador da categoria
                                                   );
                    SqlCommand commandInsert = new SqlCommand(queryInsert, connectionInsert);
                    connectionInsert.Open();
                    commandInsert.ExecuteNonQuery();
                }

                #endregion

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
