using Helper.ServerMap;
using Model.In;
using Model.Out;
using RegisterEvent.Events;
using SoftExpert;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading;
using System.Web.Configuration;

namespace Repository
{
    public class DocumentRepository
    {
        Events events = new Events();
        private bool _proxy = WebConfigurationManager.AppSettings["Proxy"] == "true" ? true : false;
        private string _proxyUrl = WebConfigurationManager.AppSettings["ProxyUrl"];

        public ECMDocumentOut GetECMDocument(ECMDocumentIn ecmDocumentIn)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            string archive = "";
            events.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocument", "");

            string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.DocumentRepository.Path"]);
            string name = ecmDocumentIn.externalId + ".pdf";
            string pathFile = Path.Combine(path, name);

            if (!File.Exists(pathFile))
            {
                try
                {
                    archive = SEDocument.GetSEDocument(ecmDocumentIn);
                }
                catch (Exception ex)
                {
                    events.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Erro", "Repository.DocumentRepository.GetECMDocument.DownloadFile", string.Format("DownloadFile: {0}, ExternalId: {1}", ex.Message, ecmDocumentIn.externalId));
                }

                if (!string.IsNullOrEmpty(archive))
                {
                    events.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocument.DownloadFile", string.Format("DownloadFile: {0}, ExternalId: {1}", archive, ecmDocumentIn.externalId));

                    DownloadFile(ecmDocumentIn.externalId, path, archive, pathFile, 1, ecmDocumentIn.userId, ecmDocumentIn.key);

                    events.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocument.DownloadFile", string.Format("DownloadFile: {0}, ExternalId: {1}", archive, ecmDocumentIn.externalId));
                }
                else
                {
                    events.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Erro", "Repository.DocumentRepository.GetECMDocument.DownloadFile", string.Format("DownloadFile: {0}, ExternalId: {1}", i18n.Resource.FileNotFound, ecmDocumentIn.externalId));

                    throw new Exception(i18n.Resource.FileNotFound);
                }

            }

            events.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocument", "");
            return ecmDocumentOut;
        }

        public ECMDocumentsOut GetECMDocuments(ECMDocumentsIn ecmDocumentsIn)
        {
            ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();
            events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocuments", "");

            ecmDocumentsOut = SEDocument.GetSEDocuments(ecmDocumentsIn);

            events.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocuments", "");
            return ecmDocumentsOut;
        }

        public ECMDocumentSaveOut PostECMDocumentSave(ECMDocumentSaveIn ecmDocumentSaveIn)
        {
            ECMDocumentSaveOut ecmDocumentSaveOut = new ECMDocumentSaveOut();

            events.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - Start", "Repository.DocumentRepository.PostECMDocumentSave", "");

            SEDocument.SEDocumentSave(ecmDocumentSaveIn);

            events.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - End", "Repository.DocumentRepository.PostECMDocumentSave", "");
            return ecmDocumentSaveOut;
        }

        public ECMDocumentDeleteOut DeleteECMDocument(ECMDocumentDeleteIn eCMDocumentDeleteIn)
        {
            ECMDocumentDeleteOut eCMDocumentDeleteOut = new ECMDocumentDeleteOut();
            events.SaveRegisterEvent(eCMDocumentDeleteIn.userId, eCMDocumentDeleteIn.key, "Log - Start", "Repository.DocumentRepository.DeleteECMDocument", "");

            SEDocument.SEDocumentDelete(eCMDocumentDeleteIn.externalId);

            events.SaveRegisterEvent(eCMDocumentDeleteIn.userId, eCMDocumentDeleteIn.key, "Log - End", "Repository.DocumentRepository.DeleteECMDocument", "");
            return eCMDocumentDeleteOut;
        }

        private void DownloadFile(string externalId, string path, string archive, string pathFile, int exec, string userId, string key)
        {
            try
            {
                WebClient wc = new WebClient();

                if (_proxy)
                {
                    WebProxy wp = new WebProxy(_proxyUrl);
                    wc.Proxy = wp;
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                wc.DownloadFile(archive, pathFile);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(userId, key, "Erro", "Repository.DocumentRepository.GetECMDocument", string.Format("ExternalId: {0}. Arquivo: {1}. Erro: {2}", externalId, archive, ex.Message));

                if (exec < 5)
                {
                    exec++;
                    int sleep = 3000;
                    int.TryParse(ConfigurationManager.AppSettings["SLEEP"], out sleep);

                    Thread.Sleep(sleep);
                    DownloadFile(externalId, path, archive, pathFile, exec, userId, key);
                }
                else
                {
                    throw new Exception(i18n.Resource.DownloadFailed);
                }
            }
        }
    }
}
