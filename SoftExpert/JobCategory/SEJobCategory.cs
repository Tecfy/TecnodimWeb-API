﻿using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Linq;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEJobCategory
    {
        #region .: Public Methods :.

        public static ECMJobCategoryOut GetSEJobCategory(ECMJobCategoryIn eCMJobCategoryIn)
        {
            ECMJobCategoryOut eCMJobCategoryOut = new ECMJobCategoryOut();

            SEClient seClient = SEConnection.GetConnection();

            eletronicFile[] eletronicFiles = seClient.downloadEletronicFile(eCMJobCategoryIn.externalId, "", "", "", eCMJobCategoryIn.categoryId, "", "", "");
            if (eletronicFiles.Any(x => string.IsNullOrEmpty(x.ERROR)))
            {
                eCMJobCategoryOut.result = new ECMJobCategoryVM()
                {
                    archive = System.Convert.ToBase64String(eletronicFiles.FirstOrDefault().BINFILE),
                };
            }
            else
            {
                throw new Exception(eletronicFiles.Where(x => !string.IsNullOrEmpty(x.ERROR)).FirstOrDefault().ERROR);
            }

            return eCMJobCategoryOut;
        }

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
                documentDataReturn documentDataReturn = SEDocument.GetDocumentData(ecmJobCategorySaveIn.code);

                //If the document already exists in the specified category, it uploads the document and properties
                if (documentDataReturn.IDDOCUMENT != null)
                {
                    var ds = seClient.deleteDocument(ecmJobCategorySaveIn.categoryId, ecmJobCategorySaveIn.code, "", WebConfigurationManager.AppSettings["SoftExpert.MessageDeleteDocument"]);
                }

                //If you do not insert a new document

                documentDataReturn = SEDocument.GetDocumentData(documentReturnOwner.IDDOCUMENT);

                if (documentDataReturn.ATTRIBUTTES.Count() > 0)
                {
                    var document = seClient.newDocument(WebConfigurationManager.AppSettings["SoftExpert.JobCategory"], ecmJobCategorySaveIn.code.Trim(), ecmJobCategorySaveIn.title, "", "", "", "", null, 0);

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
                                var s = seClient.setAttributeValue(ecmJobCategorySaveIn.code.Trim(), "", item.ATTRIBUTTENAME, value);
                            }
                            catch (Exception)
                            {
                                throw new Exception(string.Format(i18n.Resource.FieldWithError, item.ATTRIBUTTENAME));
                            }
                        }
                    }

                    try
                    {
                        var retornoDataJob = seClient.setAttributeValue(ecmJobCategorySaveIn.code.Trim(), "", EAttribute.MFP_Data_Job.ToString(), ecmJobCategorySaveIn.dataJob.ToString("yyyy-MM-dd"));
                        var retornoStatus = seClient.setAttributeValue(ecmJobCategorySaveIn.code.Trim(), "", EAttribute.MFP_Status.ToString(), WebConfigurationManager.AppSettings["SoftExpert.JobStatusInitial"]);
                        var retornoCategoria = seClient.setAttributeValue(ecmJobCategorySaveIn.code.Trim(), "", EAttribute.MFP_Categoria.ToString(), ecmJobCategorySaveIn.categoryId);
                        var retornoUsuario = seClient.setAttributeValue(ecmJobCategorySaveIn.code.Trim(), "", EAttribute.MFP_Usuario.ToString(), ecmJobCategorySaveIn.user);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    if (documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()))
                    {
                        string unityCode = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault();

                        SEAdministration seAdministration = SEConnection.GetConnectionAdm();

                        seAdministration.newPosition(unityCode, unityCode, out string status, out string detail, out int code, out string recordid, out string recordKey);
                        seAdministration.newDepartment(unityCode, unityCode, unityCode, "", "", "1");

                        var n = seClient.newAccessPermission(ecmJobCategorySaveIn.code.Trim(),
                                unityCode + ";" + unityCode,
                                int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.UserType"].ToString()),
                                WebConfigurationManager.AppSettings["NewAccessPermission.Permission"].ToString(),
                                int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.PermissionType"].ToString()),
                                WebConfigurationManager.AppSettings["NewAccessPermission.FgaddLowerLevel"].ToString());
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static bool SEJobSave(ECMJobSaveIn eCMJobSaveIn)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                string prefix = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePrefixReplicate"];

                //Check if there is a registered owner document
                documentReturn documentReturnOwner = SEDocument.GetSEDocumentByRegistrationAndCategory(eCMJobSaveIn.registration, WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"]);

                if (documentReturnOwner == null)
                {
                    throw new Exception(i18n.Resource.StudentNotFound);
                }

                //Checks whether the document exists
                documentReturn documentReturn = SEDocument.GetSEDocumentByRegistrationAndCategory(eCMJobSaveIn.registration, eCMJobSaveIn.categoryId);

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

                        foreach (var item in eCMJobSaveIn.additionalFields)
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

                        var ds = SEDocumentUpload(eCMJobSaveIn, documentReturn.IDDOCUMENT);

                        if (documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()))
                        {
                            string unityCode = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_cod_unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault();

                            SEAdministration seAdministration = SEConnection.GetConnectionAdm();

                            seAdministration.newPosition(unityCode, unityCode, out string status, out string detail, out int code, out string recordid, out string recordKey);
                            seAdministration.newDepartment(unityCode, unityCode, unityCode, "", "", "1");

                            var n = seClient.newAccessPermission(documentReturn.IDDOCUMENT,
                                    unityCode + ";" + unityCode,
                                    int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.UserType"].ToString()),
                                    WebConfigurationManager.AppSettings["NewAccessPermission.Permission"].ToString(),
                                    int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.PermissionType"].ToString()),
                                    WebConfigurationManager.AppSettings["NewAccessPermission.FgaddLowerLevel"].ToString());
                        }
                    }
                }

                //If you do not insert a new document
                else
                {
                    documentDataReturn documentDataReturn = SEDocument.GetDocumentData(documentReturnOwner.IDDOCUMENT);
                    if (documentDataReturn.ATTRIBUTTES.Count() > 0)
                    {
                        var document = seClient.newDocument(eCMJobSaveIn.categoryId, eCMJobSaveIn.registration.Trim() + "-" + eCMJobSaveIn.categoryId.Trim(), eCMJobSaveIn.title, "", "", "", "", null, 0);

                        var documentMatrix = document.Split(':');

                        if (documentMatrix.Count() > 0)
                        {
                            if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                            {
                                SEDocumentUpload(eCMJobSaveIn, eCMJobSaveIn.registration.Trim() + "-" + eCMJobSaveIn.categoryId.Trim());
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
                                    var s = seClient.setAttributeValue(eCMJobSaveIn.registration.Trim() + "-" + eCMJobSaveIn.categoryId.Trim(), "", item.ATTRIBUTTENAME, value);
                                }
                                catch (Exception)
                                {
                                    throw new Exception(string.Format(i18n.Resource.FieldWithError, item.ATTRIBUTTENAME));
                                }
                            }
                        }

                        foreach (var item in eCMJobSaveIn.additionalFields)
                        {
                            try
                            {
                                if (item.additionalFieldId == (int)EAdditionalField.Identifier)
                                {
                                    var retorno = seClient.setAttributeValue(eCMJobSaveIn.registration.Trim() + "-" + eCMJobSaveIn.categoryId.Trim(), "", EAttribute.SER_Input_NumDoc.ToString(), item.value);
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.Competence)
                                {
                                    DateTime competence = DateTime.MinValue;
                                    DateTime.TryParse(item.value, out competence);

                                    var retorno = seClient.setAttributeValue(eCMJobSaveIn.registration.Trim() + "-" + eCMJobSaveIn.categoryId.Trim(), "", EAttribute.SER_Input_DataRef.ToString(), competence.ToString("yyyy-MM-dd"));
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.Validity)
                                {
                                    DateTime validity = DateTime.MinValue;
                                    DateTime.TryParse(item.value, out validity);

                                    var retorno = seClient.setAttributeValue(eCMJobSaveIn.registration.Trim() + "-" + eCMJobSaveIn.categoryId.Trim(), "", EAttribute.SER_Input_Data_Vencto.ToString(), validity.ToString("yyyy-MM-dd"));
                                }
                                else if (item.additionalFieldId == (int)EAdditionalField.DocumentView)
                                {
                                    var retorno = seClient.setAttributeValue(eCMJobSaveIn.registration.Trim() + "-" + eCMJobSaveIn.categoryId.Trim(), "", EAttribute.SER_Input_Compl.ToString(), item.value);
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

                            var n = seClient.newAccessPermission(eCMJobSaveIn.registration.Trim() + "-" + eCMJobSaveIn.categoryId.Trim(),
                                    unityCode + ";" + unityCode,
                                    int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.UserType"].ToString()),
                                    WebConfigurationManager.AppSettings["NewAccessPermission.Permission"].ToString(),
                                    int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.PermissionType"].ToString()),
                                    WebConfigurationManager.AppSettings["NewAccessPermission.FgaddLowerLevel"].ToString());
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

        private static bool SEDocumentUpload(ECMJobSaveIn eCMJobSaveIn, string documentid)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                eletronicFile[] eletronicFiles = new eletronicFile[2];
                eletronicFiles[0] = new eletronicFile
                {
                    BINFILE = Convert.FromBase64String(eCMJobSaveIn.archive),
                    ERROR = "",
                    NMFILE = eCMJobSaveIn.title
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
