using Helper.Enum;
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
    public static class SEStudent
    {
        #region .: Attributes :.

        private static readonly string searchAttributeOwnerCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributeOwnerCategory"];
        private static readonly string searchAttributePendingCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingCategory"];

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
                foreach (documentReturn item in (searchDocumentReturn.RESULTS))
                {
                    documentDataReturn documentDataReturn = Common.GetDocumentProperties(item.IDDOCUMENT);

                    studentDocumentsOut.result.Add(new StudentDocumentsVM()
                    {
                        iddocument = documentDataReturn.IDDOCUMENT,
                        name = documentDataReturn.NMTITLE,
                        code = documentDataReturn.IDCATEGORY,
                        date = documentDataReturn.DTDOCUMENT
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
                foreach (eletronicFile item in eletronicFiles)
                {
                    string filename = item.NMFILE;
                    string[] arr = filename.Split('-');
                    if (arr.Length >= 2)
                    {
                        if (arr.Length >= 2)
                        {
                            string d = arr[arr.Length - 2];
                            string h = arr[arr.Length - 1].Split('.')[0];
                            if (d.Length == 8 && h.Length == 6)
                            {
                                int year = int.Parse(d[4].ToString() + d[5].ToString() + d[6].ToString() + d[7].ToString());
                                int month = int.Parse(d[2].ToString() + d[3].ToString());
                                int day = int.Parse(d[0].ToString() + d[1].ToString());
                                int hour = int.Parse(h[0].ToString() + h[1].ToString());
                                int minute = int.Parse(h[2].ToString() + h[3].ToString());
                                int second = int.Parse(h[4].ToString() + h[5].ToString());

                                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                                
                                item.NMFILE = dateTime.ToString("yyyy-MM-dd-HH-mm-ss") + "\\" + filename;
                            }
                        }
                    }
                    
                }

                eletronicFiles = eletronicFiles.OrderByDescending(a => a.NMFILE).ToArray();

                foreach (eletronicFile item in (eletronicFiles))
                {
                    var arrFilename = item.NMFILE.Split('\\');
                    if (arrFilename.Length == 2)
                    {
                        item.NMFILE = arrFilename[1];
                    }
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
