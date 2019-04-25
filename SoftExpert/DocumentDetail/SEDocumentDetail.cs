using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEDocumentDetail
    {
        #region .: Attributes :.

        readonly static string searchAttributeOwnerUnity = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerUnity"];
        readonly static string searchAttributeOwnerRegistration = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerRegistration"];
        readonly static string searchAttributeOwnerCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"];
        readonly static int newAccessPermissionUserType = int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.UserType"].ToString());
        readonly static string newAccessPermissionPermission = WebConfigurationManager.AppSettings["NewAccessPermission.Permission"].ToString();
        readonly static int newAccessPermissionPermissionType = int.Parse(WebConfigurationManager.AppSettings["NewAccessPermission.PermissionType"].ToString());
        readonly static string newAccessPermissionFgaddLowerLevel = WebConfigurationManager.AppSettings["NewAccessPermission.FgaddLowerLevel"].ToString();
        readonly static SEClient seClient = SEConnection.GetConnection();

        #endregion

        #region .: Public Methods :.

        public static ECMDocumentDetailsByRegistrationOut GetSEDocumentDetailsByRegistration(ECMDocumentDetailsByRegistrationIn ecmDocumentDetailsByRegistrationIn)
        {
            ECMDocumentDetailsByRegistrationOut ecmDocumentDetailsByRegistrationOut = new ECMDocumentDetailsByRegistrationOut();

            attributeData[] attributeDatas = new attributeData[2];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = searchAttributeOwnerUnity,
                VLATTRIBUTE = ecmDocumentDetailsByRegistrationIn.unity
            };
            attributeDatas[1] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = searchAttributeOwnerRegistration,
                VLATTRIBUTE = ecmDocumentDetailsByRegistrationIn.registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter
            {
                IDCATEGORY = searchAttributeOwnerCategory
            };

            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                foreach (var item in (searchDocumentReturn.RESULTS))
                {
                    documentDataReturn documentDataReturn = Common.GetDocumentProperties(item.IDDOCUMENT);

                    ecmDocumentDetailsByRegistrationOut.result.Add(new ECMDocumentDetailsByRegistrationVM()
                    {
                        unity = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        course = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        cpf = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        rg = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_RG.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_RG.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        name = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    });
                }
            }
            else
            {
                ecmDocumentDetailsByRegistrationOut.result = null;
            }

            return ecmDocumentDetailsByRegistrationOut;
        }

        public static ECMDocumentDetailByRegistrationOut GetSEDocumentDetailByRegistration(ECMDocumentDetailByRegistrationIn ecmDocumentDetailByRegistrationIn)
        {
            ECMDocumentDetailByRegistrationOut ecmDocumentDetailByRegistrationOut = new ECMDocumentDetailByRegistrationOut();

            SEClient seClient = SEConnection.GetConnection();
            attributeData[] attributeDatas = new attributeData[2];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = searchAttributeOwnerUnity,
                VLATTRIBUTE = ecmDocumentDetailByRegistrationIn.unity
            };
            attributeDatas[1] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = searchAttributeOwnerRegistration,
                VLATTRIBUTE = ecmDocumentDetailByRegistrationIn.registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter
            {
                IDCATEGORY = searchAttributeOwnerCategory
            };

            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                documentDataReturn documentDataReturn = Common.GetDocumentProperties(searchDocumentReturn.RESULTS.Select(x => x.IDDOCUMENT).FirstOrDefault());

                ecmDocumentDetailByRegistrationOut.result = new ECMDocumentDetailByRegistrationVM()
                {
                    unity = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    course = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    cpf = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    rg = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_RG.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_RG.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    name = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                };
            }
            else
            {
                ecmDocumentDetailByRegistrationOut.result = null;
            }

            return ecmDocumentDetailByRegistrationOut;
        }

        public static ECMDocumentDetailOut GetSEDocumentDetail(ECMDocumentDetailIn ecmDocumentDetailIn)
        {
            ECMDocumentDetailOut ecmDocumentDetailOut = new ECMDocumentDetailOut();

            SEClient seClient = SEConnection.GetConnection();
            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = searchAttributeOwnerRegistration,
                VLATTRIBUTE = ecmDocumentDetailIn.registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter
            {
                IDCATEGORY = searchAttributeOwnerCategory
            };

            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                documentDataReturn documentDataReturn = Common.GetDocumentProperties(searchDocumentReturn.RESULTS.Select(x => x.IDDOCUMENT).FirstOrDefault());

                ecmDocumentDetailOut.result = new ECMDocumentDetailVM()
                {
                    unity = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    course = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    cpf = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    rg = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_RG.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_RG.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    name = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                };
            }
            else
            {
                ecmDocumentDetailOut.result = null;
            }

            return ecmDocumentDetailOut;
        }

        public static string SEDocumentDetailSave(ECMDocumentDetailSaveIn eCMDocumentDetailSaveIn)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();
                string registration = string.Empty;

                //Check if there is a registered owner document
                documentReturn documentReturn = Common.CheckRegisteredDocument(eCMDocumentDetailSaveIn.registration, searchAttributeOwnerCategory);

                //If the document already exists in the specified category, it uploads the document and properties
                if (documentReturn != null)
                {
                    registration = documentReturn.IDDOCUMENT;

                    try
                    {
                        documentDataReturn documentDataReturn = Common.GetDocumentProperties(documentReturn.IDDOCUMENT);

                        var returnUnityCodes = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_cod_unidade.ToString(), eCMDocumentDetailSaveIn.unityCode);
                        if (!string.IsNullOrEmpty(eCMDocumentDetailSaveIn.cpf))
                        {
                            var returnCPF = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Cpf.ToString(), eCMDocumentDetailSaveIn.cpf);
                        }
                        if (!string.IsNullOrEmpty(eCMDocumentDetailSaveIn.rg))
                        {
                            var returnRG = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_RG.ToString(), eCMDocumentDetailSaveIn.rg);
                        }
                        var returnCourse = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Curso.ToString(), eCMDocumentDetailSaveIn.course);
                        var returnRegistration = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Matricula.ToString(), eCMDocumentDetailSaveIn.registration);
                        var returnName = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_NomedoAluno.ToString(), eCMDocumentDetailSaveIn.name);
                        var returnStatus = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_SituacaoAluno.ToString(), eCMDocumentDetailSaveIn.status);
                        var returnUnity = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Unidade.ToString(), eCMDocumentDetailSaveIn.unity);

                        List<string> units = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_unidades.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.ToList();
                        if (!units.Any(x => x == eCMDocumentDetailSaveIn.unityCode))
                        {
                            SEAdministration seAdministration = SEConnection.GetConnectionAdm();

                            seAdministration.newPosition(eCMDocumentDetailSaveIn.unityCode, eCMDocumentDetailSaveIn.unityCode, out string status, out string detail, out int code, out string recordid, out string recordKey);
                            seAdministration.newDepartment(eCMDocumentDetailSaveIn.unityCode, eCMDocumentDetailSaveIn.unityCode, eCMDocumentDetailSaveIn.unityCode, "", "", "1");

                            var s = seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_unidades.ToString(), eCMDocumentDetailSaveIn.unityCode);

                            var n = seClient.newAccessPermission(documentReturn.IDDOCUMENT,
                                eCMDocumentDetailSaveIn.unityCode + ";" + eCMDocumentDetailSaveIn.unityCode,
                                newAccessPermissionUserType,
                                newAccessPermissionPermission,
                                newAccessPermissionPermissionType,
                                newAccessPermissionFgaddLowerLevel);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                //If you do not insert a new document
                else
                {
                    var document = seClient.newDocument(searchAttributeOwnerCategory, eCMDocumentDetailSaveIn.registration.Trim(), eCMDocumentDetailSaveIn.name, "", "", "", "", null, 0, null);

                    var documentMatrix = document.Split(':');

                    if (documentMatrix.Count() > 0)
                    {
                        if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                        {
                            registration = documentMatrix[1];

                            var returnUnityCodes = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_cod_unidade.ToString(), eCMDocumentDetailSaveIn.unityCode);
                            if (!string.IsNullOrEmpty(eCMDocumentDetailSaveIn.cpf))
                            {
                                var returnCPF = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Cpf.ToString(), eCMDocumentDetailSaveIn.cpf);
                            }
                            if (!string.IsNullOrEmpty(eCMDocumentDetailSaveIn.rg))
                            {
                                var returnRG = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_RG.ToString(), eCMDocumentDetailSaveIn.rg);
                            }
                            var returnCourse = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Curso.ToString(), eCMDocumentDetailSaveIn.course);
                            var returnRegistration = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Matricula.ToString(), eCMDocumentDetailSaveIn.registration);
                            var returnName = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_NomedoAluno.ToString(), eCMDocumentDetailSaveIn.name);
                            var returnStatus = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_SituacaoAluno.ToString(), eCMDocumentDetailSaveIn.status);
                            var returnUnity = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Unidade.ToString(), eCMDocumentDetailSaveIn.unity);
                            var returnUnityCode = seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_unidades.ToString(), eCMDocumentDetailSaveIn.unityCode);

                            SEAdministration seAdministration = SEConnection.GetConnectionAdm();

                            seAdministration.newPosition(eCMDocumentDetailSaveIn.unityCode, eCMDocumentDetailSaveIn.unityCode, out string status, out string detail, out int code, out string recordid, out string recordKey);
                            seAdministration.newDepartment(eCMDocumentDetailSaveIn.unityCode, eCMDocumentDetailSaveIn.unityCode, eCMDocumentDetailSaveIn.unityCode, "", "", "1");

                            var n = seClient.newAccessPermission(eCMDocumentDetailSaveIn.registration.Trim(),
                                eCMDocumentDetailSaveIn.unityCode + ";" + eCMDocumentDetailSaveIn.unityCode,
                                newAccessPermissionUserType,
                                newAccessPermissionPermission,
                                newAccessPermissionPermissionType,
                                newAccessPermissionFgaddLowerLevel);
                        }
                        else
                        {
                            throw new Exception(document);
                        }
                    }
                }

                return registration;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion
    }
}
