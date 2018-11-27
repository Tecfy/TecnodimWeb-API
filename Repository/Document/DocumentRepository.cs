using Helper.ServerMap;
using Model.In;
using Model.Out;
using SoftExpert;
using System.Configuration;

namespace Repository
{
    public class DocumentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMDocumentOut GetECMDocument(ECMDocumentIn ecmDocumentIn)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            registerEventRepository.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocument", "");

            string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.DocumentRepository.Path"]);
            string name = ecmDocumentIn.externalId + ".pdf";

            if (System.IO.File.Exists(path + "\\" + name))
            {
                byte[] archive = System.IO.File.ReadAllBytes(path + "\\" + name);

                ecmDocumentOut.result.archive = System.Convert.ToBase64String(archive);
            }
            else
            {
                ecmDocumentOut = SEDocument.GetSEDocument(ecmDocumentIn);

                byte[] archive = System.Convert.FromBase64String(ecmDocumentOut.result.archive);

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                System.IO.File.WriteAllBytes(path + "\\" + name, archive);
            }

            registerEventRepository.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocument", "");
            return ecmDocumentOut;
        }

        public ECMDocumentsOut GetECMDocuments(ECMDocumentsIn ecmDocumentsIn)
        {
            ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();
            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocuments", "");

            ecmDocumentsOut = SEDocument.GetSEDocuments();

            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocuments", "");
            return ecmDocumentsOut;
        }

        public ECMDocumentSaveOut PostECMDocumentSave(ECMDocumentSaveIn ecmDocumentSaveIn)
        {
            ECMDocumentSaveOut ecmDocumentSaveOut = new ECMDocumentSaveOut();

            registerEventRepository.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - Start", "Repository.DocumentRepository.PostECMDocumentSave", "");

            SEDocument.SEDocumentSave(ecmDocumentSaveIn);

            registerEventRepository.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - End", "Repository.DocumentRepository.PostECMDocumentSave", "");
            return ecmDocumentSaveOut;
        }

        public bool DeleteECMDocumentArchive(ECMDocumentDeletedIn ecmDocumentDeletedIn)
        {
            try
            {
                string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.DocumentRepository.Path"]);
                string name = ecmDocumentDeletedIn.externalId + ".pdf";

                if (System.IO.File.Exists(path + "\\" + name))
                {
                    System.IO.File.Delete(path + "\\" + name);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
