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
        
        private static readonly string physicalPath = WebConfigurationManager.AppSettings["Sesuite.Physical.Path"];
        private static readonly string physicalPathSE = WebConfigurationManager.AppSettings["Sesuite.Physical.Path.SE"];
        private static readonly SEClient seClient = SEConnection.GetConnection();
        private static readonly SEAdministration seAdministration = SEConnection.GetConnectionAdm();

        #endregion

        #region .: Public Methods :.        

        public static documentDataReturn GetDocumentProperties(string documentid)
        {
            return seClient.viewDocumentData(documentid, "", "", "");
        }

        #endregion

        #region .: Private Methods :.  

        public static bool SEDocumentPhysicalFile(byte[] fileBinary, string fileName, string documentId, string user, string categoryId)
        {
            try
            {
                #region .: Save File Local :.

                SaveFile(physicalPath, fileName, fileBinary);

                #endregion

                #region .: Query :.

                string queryStringInsert = @"INSERT INTO ADINTERFACE (CDINTERFACE, FGIMPORT, CDISOSYSTEM, FGOPTION, NMFIELD01, NMFIELD02, NMFIELD03, NMFIELD04, NMFIELD05, NMFIELD07) VALUES((SELECT COALESCE(MAX(CDINTERFACE),0)+1 FROM ADINTERFACE), 1, 73, 97, '{0}','00','{1}','{2}','{3}','{4}')";
                string connectionString = WebConfigurationManager.ConnectionStrings["DefaultSesuite"].ConnectionString;

                #endregion

                #region .: Insert Sesuite :.

                using (SqlConnection connectionInsert = new SqlConnection(connectionString))
                {
                    string queryInsert = string.Format(queryStringInsert,
                        documentId /*Identificador do Documento*/,
                        fileName /*Nome do Arquivo*/,
                        user /*Matrícula do Usuário*/,
                        physicalPathSE + fileName /*Caminho do Arquivo*/,
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

            File.WriteAllBytes(Path.Combine(folder, fileName), fileBinary);
        }

        #endregion
    }
}
