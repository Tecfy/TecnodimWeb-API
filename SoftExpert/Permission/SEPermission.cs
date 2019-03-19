using Helper.Enum;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System.Linq;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEPermission
    {
        #region .: Attributes :.

        readonly static string searchAttributePermissionStatus = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePermissionStatus"];
        readonly static string searchAttributePermissionUpdate = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePermissionUpdate"];
        readonly static string searchAttributePermissionCategory = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePermissionCategory"];
        readonly static SEClient seClient = SEConnection.GetConnection();

        #endregion

        #region .: Public Methods :.

        public static ECMPermissionsOut GetSEPermissions(ECMPermissionsIn ecmPermissionsIn)
        {
            ECMPermissionsOut ecmPermissionsOut = new ECMPermissionsOut();

            attributeData[] attributeDatas = new attributeData[0];
            //attributeDatas[0] = new attributeData
            //{
            //    //search enrollment
            //    IDATTRIBUTE = searchAttributePermissionStatus,
            //    VLATTRIBUTE = searchAttributePermissionUpdate
            //};

            searchDocumentFilter searchDocumentFilter = new searchDocumentFilter
            {
                IDCATEGORY = searchAttributePermissionCategory
            };

            searchDocumentReturn searchDocumentReturn = seClient.searchDocument(searchDocumentFilter, "", attributeDatas);
            documentReturn retorno = new documentReturn();
            if (searchDocumentReturn.RESULTS.Count() > 0)
            {
                foreach (var item in (searchDocumentReturn.RESULTS))
                {
                    documentDataReturn documentDataReturn = Common.GetDocumentProperties(item.IDDOCUMENT);

                    ecmPermissionsOut.result.Add(new ECMPermissionsVM()
                    {
                        externalId = item.IDDOCUMENT,
                        cappservice = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_cappservice.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_cappservice.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() == "sim" : false,
                        classify = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_classifica.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_classifica.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() == "sim" : false,
                        slice = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_recorte.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_recorte.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() == "sim" : false,
                        name = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_nomeusuario.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_nomeusuario.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        registration = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_userid.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_userid.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,
                        group = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_grupocappservice.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_grupocappservice.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,                                               
                        status = documentDataReturn.ATTRIBUTTES.Any(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_status.ToString()) ? documentDataReturn.ATTRIBUTTES.Where(x => x.ATTRIBUTTENAME == EAttribute.tfyacess_status.ToString()).FirstOrDefault().ATTRIBUTTEVALUE.FirstOrDefault() : null,                        
                    });
                }
            }
            else
            {
                ecmPermissionsOut.result = null;
            }

            return ecmPermissionsOut;
        }

        #endregion
    }
}
