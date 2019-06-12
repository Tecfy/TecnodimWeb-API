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
    public static class SEStudent
    {
        #region .: Attributes :.

        readonly static string searchAttributeOwnerCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"];
        readonly static string searchAttributePendingCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingCategory"];

        #endregion

        #region .: Public Methods :.

        public static StudentDocumentsOut GetSEStudentDocuments(StudentDocumentsIn studentDocumentsIn)
        {
            StudentDocumentsOut studentDocumentsOut = new StudentDocumentsOut();
            SEClient seClient = SEConnection.GetConnection(studentDocumentsIn.username, studentDocumentsIn.password);

            attributeData[] attributeDatas = new attributeData[3];
            if (!string.IsNullOrEmpty(studentDocumentsIn.cpf))
            {
                attributeDatas[0] = new attributeData
                {
                    //search cpf
                    IDATTRIBUTE = EAttribute.SER_cad_Cpf.ToString(),
                    VLATTRIBUTE = studentDocumentsIn.cpf
                };
            }
            if (!string.IsNullOrEmpty(studentDocumentsIn.name))
            {
                attributeDatas[1] = new attributeData
                {
                    //search name
                    IDATTRIBUTE = EAttribute.SER_cad_NomedoAluno.ToString(),
                    VLATTRIBUTE = studentDocumentsIn.name
                };
            }
            if (!string.IsNullOrEmpty(studentDocumentsIn.registration))
            {
                attributeDatas[2] = new attributeData
                {
                    //search registration
                    IDATTRIBUTE = EAttribute.SER_cad_Matricula.ToString(),
                    VLATTRIBUTE = studentDocumentsIn.registration
                };
            }

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter();

            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                foreach (var item in (searchDocumentReturn.RESULTS))
                {
                    documentDataReturn documentDataReturn = Common.GetDocumentProperties(item.IDDOCUMENT);

                    studentDocumentsOut.result.Add(new StudentDocumentsVM()
                    {
                        iddocument = documentDataReturn.IDDOCUMENT,
                        name = documentDataReturn.NMTITLE,
                        code = documentDataReturn.IDCATEGORY
                    });
                }

                studentDocumentsOut.result = studentDocumentsOut.result.Where(x => x.code != searchAttributeOwnerCategory && x.code != searchAttributePendingCategory).ToList();
            }
            else
            {
                studentDocumentsOut.result = null;
            }

            return studentDocumentsOut;
        }

        public static StudentDocumentOut GetSEStudentDocument(StudentDocumentIn studentDocumentIn)
        {
            StudentDocumentOut studentDocumentOut = new StudentDocumentOut();
            SEClient seClient = SEConnection.GetConnection(studentDocumentIn.username, studentDocumentIn.password);

            eletronicFile[] eletronicFiles = seClient.downloadEletronicFile(studentDocumentIn.iddocument, "", "", "", "", "", "", "1");
            if (!eletronicFiles.Any(x => x.ERROR != null))
            {
                foreach (var item in (eletronicFiles))
                {
                    studentDocumentOut.result.Add(new StudentDocumentVM()
                    {
                        uri = Encoding.ASCII.GetString(item.BINFILE)
                    });
                }
            }
            else
            {
                studentDocumentOut.result = null;
            }

            return studentDocumentOut;
        }

        #endregion
    }
}
