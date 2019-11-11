using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEResendDocument
    {
        #region .: Attributes :.

        private readonly static string searchAttributeOwnerUnity = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerUnity"];
        private readonly static string searchAttributeOwnerRegistration = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerRegistration"];
        private readonly static string searchAttributeResendCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeResendCategory"];
        private readonly static SEClient seClient = SEConnection.GetConnection();

        #endregion

        #region .: Public Methods :.

        public static ECMResendDocumentsOut GetECMResendDocuments(ECMResendDocumentsIn eCMResendDocumentsIn)
        {
            ECMResendDocumentsOut eCMResendDocumentsOut = new ECMResendDocumentsOut();

            attributeData[] attributeDatas = new attributeData[2];
            attributeDatas[0] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = searchAttributeOwnerUnity,
                VLATTRIBUTE = eCMResendDocumentsIn.unity
            };
            attributeDatas[1] = new attributeData
            {
                //search enrollment
                IDATTRIBUTE = searchAttributeOwnerRegistration,
                VLATTRIBUTE = eCMResendDocumentsIn.registration
            };

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter
            {
                IDCATEGORY = searchAttributeResendCategory
            };

            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                foreach (var item in (searchDocumentReturn.RESULTS))
                {
                    documentDataReturn documentDataReturn = Common.GetDocumentProperties(item.IDDOCUMENT);

                    ECMResendDocumentsVM eCMResendDocumentsVM = new ECMResendDocumentsVM()
                    {
                        name = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_NomedoAluno.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Matricula.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        cpf = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.SER_cad_Cpf.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                    };

                    eCMResendDocumentsVM.itens = new List<ECMResendDocumentItemVM>();

                    eletronicFile[] eletronicFiles = seClient.downloadEletronicFile(item.IDDOCUMENT, "", "", "", "", "", "", "1");

                    if (!eletronicFiles.Any(x => x.ERROR != null))
                    {
                        foreach (eletronicFile doc in (eletronicFiles))
                        {
                            eCMResendDocumentsVM.itens.Add(new ECMResendDocumentItemVM()
                            {
                                uri = Encoding.ASCII.GetString(doc.BINFILE),
                                title = doc.NMFILE
                            });
                        }
                    }
                    else
                    {
                        eCMResendDocumentsVM.itens = null;
                    }

                    if (eCMResendDocumentsVM.itens == null || eCMResendDocumentsVM.itens.Count > 0)
                    {
                        eCMResendDocumentsOut.result.Add(eCMResendDocumentsVM);
                    }
                }
            }
            else
            {
                eCMResendDocumentsOut.result = null;
            }

            return eCMResendDocumentsOut;
        }

        #endregion
    }
}
