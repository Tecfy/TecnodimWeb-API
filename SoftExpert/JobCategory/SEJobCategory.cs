﻿using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEJobCategory
    {
        #region .: Attributes :.
        
        private static readonly SEClient seClient = SEConnection.GetConnection();

        #endregion

        #region .: Public Methods :.

        public static bool SEDocumentSave(ECMJobSaveIn eCMJobSaveIn)
        {
            try
            {
                #region .: Connection :.

                string connectionString = WebConfigurationManager.ConnectionStrings["DefaultSesuite"].ConnectionString;

                #endregion

                #region .: Synchronization :.

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("p_Scan_Document", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@Registration", SqlDbType.VarChar).Value = eCMJobSaveIn.registration;
                        command.Parameters.Add("@Document", SqlDbType.VarChar).Value = eCMJobSaveIn.DocumentId;
                        command.Parameters.Add("@Category", SqlDbType.VarChar).Value = eCMJobSaveIn.categoryId;
                        command.Parameters.Add("@User", SqlDbType.VarChar).Value = eCMJobSaveIn.user;
                        command.Parameters.Add("@Title", SqlDbType.VarChar).Value = eCMJobSaveIn.title;
                        command.Parameters.Add("@UnityCode", SqlDbType.VarChar).Value = eCMJobSaveIn.unityCode;
                        command.Parameters.Add("@UnityName", SqlDbType.VarChar).Value = eCMJobSaveIn.unityName;
                        command.Parameters.Add("@Identifier", SqlDbType.VarChar).Value = eCMJobSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.Identifier) ? eCMJobSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.Identifier).FirstOrDefault().value : null;
                        DateTime? Competence;
                        if (eCMJobSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.Competence))
                        {
                            Competence = Convert.ToDateTime(eCMJobSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.Competence).FirstOrDefault().value).Date;
                        }
                        else
                        {
                            Competence = null;
                        }
                        command.Parameters.Add("@Competence", SqlDbType.DateTime).Value = Competence;
                        DateTime? Validity;
                        if (eCMJobSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.Validity))
                        {
                            Validity = Convert.ToDateTime(eCMJobSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.Validity).FirstOrDefault().value).Date;
                        }
                        else
                        {
                            Validity = null;
                        }
                        command.Parameters.Add("@Validity", SqlDbType.DateTime).Value = Validity;
                        command.Parameters.Add("@DocumentView", SqlDbType.VarChar).Value = eCMJobSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.DocumentView) ? eCMJobSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.DocumentView).FirstOrDefault().value : null;
                        command.Parameters.Add("@Note", SqlDbType.VarChar).Value = eCMJobSaveIn.additionalFields.Any(x => x.additionalFieldId == (int)EAdditionalField.Note) ? eCMJobSaveIn.additionalFields.Where(x => x.additionalFieldId == (int)EAdditionalField.Note).FirstOrDefault().value : null;
                        
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        Common.SEDocumentPhysicalFile(eCMJobSaveIn.FileBinary, eCMJobSaveIn.FileName, eCMJobSaveIn.DocumentId, eCMJobSaveIn.user, eCMJobSaveIn.categoryId);
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

        #endregion
    }
}
