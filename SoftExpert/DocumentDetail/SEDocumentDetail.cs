using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEDocumentDetail
    {
        public static ECMDocumentDetailsByRegistrationOut GetSEDocumentDetailsByRegistration(ECMDocumentDetailsByRegistrationIn ecmDocumentDetailsByRegistrationIn)
        {
            ECMDocumentDetailsByRegistrationOut ecmDocumentDetailsByRegistrationOut = new ECMDocumentDetailsByRegistrationOut();

            SEClient seClient = SEConnection.GetConnection();

            attributeData[] attributeDatas = new attributeData[2];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerUnity"],
                VLATTRIBUTE = ecmDocumentDetailsByRegistrationIn.unity
            };
            attributeDatas[1] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerRegistration"],
                VLATTRIBUTE = ecmDocumentDetailsByRegistrationIn.registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter();
            searchDocumentFilter.IDCATEGORY = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"];
            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                foreach (var item in (searchDocumentReturn.RESULTS))
                {
                    documentDataReturn documentDataReturn = SEDocument.GetDocumentData(item.IDDOCUMENT);

                    ecmDocumentDetailsByRegistrationOut.result.Add(new ECMDocumentDetailsByRegistrationVM()
                    {
                        unity = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        course = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        cpf = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
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

        public static ECMDocumentDetailOut GetSEDocumentDetail(ECMDocumentDetailIn ecmDocumentDetailIn)
        {
            ECMDocumentDetailOut ecmDocumentDetailOut = new ECMDocumentDetailOut();

            SEClient seClient = SEConnection.GetConnection();
            attributeData[] attributeDatas = new attributeData[1];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerRegistration"],
                VLATTRIBUTE = ecmDocumentDetailIn.registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter();
            searchDocumentFilter.IDCATEGORY = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"];
            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                documentDataReturn documentDataReturn = SEDocument.GetDocumentData(searchDocumentReturn.RESULTS.Select(x => x.IDDOCUMENT).FirstOrDefault());

                ecmDocumentDetailOut.result = new ECMDocumentDetailVM()
                {
                    unity = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Unidade.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    course = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Curso.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    cpf = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    name = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                };
            }
            else
            {
                ecmDocumentDetailOut.result = null;
            }

            return ecmDocumentDetailOut;
        }

        public static bool SEDocumentDetailSave(ECMDocumentDetailSaveIn eCMDocumentDetailSaveIn)
        {
            try
            {
                SEClient seClient = SEConnection.GetConnection();

                string prefix = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePrefixReplicate"];

                //Check if there is a registered owner document
                documentReturn documentReturn = SEDocument.GetSEDocumentByRegistrationAndCategory(eCMDocumentDetailSaveIn.registration, WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"]);

                //If the document already exists in the specified category, it uploads the document and properties
                if (documentReturn != null)
                {
                    try
                    {
                        documentDataReturn documentDataReturn = SEDocument.GetDocumentData(documentReturn.IDDOCUMENT);

                        seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_cod_unidade.ToString(), eCMDocumentDetailSaveIn.unityCode);
                        seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_Cpf.ToString(), eCMDocumentDetailSaveIn.cpf);
                        seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_Curso.ToString(), eCMDocumentDetailSaveIn.course);
                        seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_Matricula.ToString(), eCMDocumentDetailSaveIn.registration);
                        seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_NomedoAluno.ToString(), eCMDocumentDetailSaveIn.name);
                        seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_SituacaoAluno.ToString(), eCMDocumentDetailSaveIn.status);
                        seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_Unidade.ToString(), eCMDocumentDetailSaveIn.unity);

                        List<string> units = documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_unidades.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.ToList();
                        if (!units.Any(x => x == eCMDocumentDetailSaveIn.unity))
                        {
                            var s = seClient.setAttributeValue(documentReturn.IDDOCUMENT, "", EAttribute.SER_cad_unidades.ToString(), eCMDocumentDetailSaveIn.unity);
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
                    var document = seClient.newDocument(WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"], eCMDocumentDetailSaveIn.registration.Trim(), eCMDocumentDetailSaveIn.name, "", "", "", "", null, 0);

                    var documentMatrix = document.Split(':');

                    if (documentMatrix.Count() > 0)
                    {
                        if (documentMatrix.Count() >= 3 && documentMatrix[2].ToUpper().Contains("SUCESSO"))
                        {
                            seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_cod_unidade.ToString(), eCMDocumentDetailSaveIn.unityCode);
                            seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Cpf.ToString(), eCMDocumentDetailSaveIn.cpf);
                            seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Curso.ToString(), eCMDocumentDetailSaveIn.course);
                            seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Matricula.ToString(), eCMDocumentDetailSaveIn.registration);
                            seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_NomedoAluno.ToString(), eCMDocumentDetailSaveIn.name);
                            seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_SituacaoAluno.ToString(), eCMDocumentDetailSaveIn.status);
                            seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_Unidade.ToString(), eCMDocumentDetailSaveIn.unity);
                            seClient.setAttributeValue(eCMDocumentDetailSaveIn.registration.Trim(), "", EAttribute.SER_cad_unidades.ToString(), eCMDocumentDetailSaveIn.unity);
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
    }
}
