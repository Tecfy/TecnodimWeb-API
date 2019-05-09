using Helper.ServerMap;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Repository
{
    public class DocumentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMDocumentOut GetECMDocument(ECMDocumentIn ecmDocumentIn)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            string archive = "";
            registerEventRepository.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocument", "");

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
                    registerEventRepository.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Erro", "Repository.DocumentRepository.GetECMDocument.DownloadFile", string.Format("DownloadFile: {0}, ExternalId: {1}", ex.Message, ecmDocumentIn.externalId));
                }

                if (!string.IsNullOrEmpty(archive))
                {
                    registerEventRepository.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocument.DownloadFile", string.Format("DownloadFile: {0}, ExternalId: {1}", archive, ecmDocumentIn.externalId));

                    DownloadFile(ecmDocumentIn.externalId, path, archive, pathFile, 1, ecmDocumentIn.userId, ecmDocumentIn.key);

                    registerEventRepository.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocument.DownloadFile", string.Format("DownloadFile: {0}, ExternalId: {1}", archive, ecmDocumentIn.externalId));
                }
                else
                {
                    registerEventRepository.SaveRegisterEvent(ecmDocumentIn.userId, ecmDocumentIn.key, "Erro", "Repository.DocumentRepository.GetECMDocument.DownloadFile", string.Format("DownloadFile: {0}, ExternalId: {1}", i18n.Resource.FileNotFound, ecmDocumentIn.externalId));

                    throw new Exception(i18n.Resource.FileNotFound);
                }

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

        public ECMDocumentsValidateOut GetECMValidateDocuments(ECMDocumentsValidateIn eCMDocumentsValidateIn)
        {
            ECMDocumentsValidateOut eCMDocumentsValidateOut = new ECMDocumentsValidateOut();
            eCMDocumentsValidateOut.result = new List<ECMDocumentsValidateVM>();
            ECMDocumentsValidateVM eCMDocumentsValidateVM = new ECMDocumentsValidateVM();

            registerEventRepository.SaveRegisterEvent(eCMDocumentsValidateIn.userId, eCMDocumentsValidateIn.key, "Log - Start", "Repository.DocumentRepository.GetECMValidateDocuments", "");

            string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.DocumentRepository.Path"]);
            int.TryParse(ConfigurationManager.AppSettings["Days"], out int days);

            foreach (var item in Directory.GetFiles(path))
            {
                if (File.GetCreationTime(item).AddDays(days) < DateTime.Now)
                {
                    string externalId = Path.GetFileName(item).Replace(".pdf", "");

                    if (!Common.CheckDocument(externalId))
                    {
                        eCMDocumentsValidateVM = new ECMDocumentsValidateVM
                        {
                            externalId = externalId
                        };
                        eCMDocumentsValidateOut.result.Add(eCMDocumentsValidateVM);

                        File.Delete(item);
                    }
                }
            }

            registerEventRepository.SaveRegisterEvent(eCMDocumentsValidateIn.userId, eCMDocumentsValidateIn.key, "Log - End", "Repository.DocumentRepository.GetECMValidateDocuments", "");
            return eCMDocumentsValidateOut;
        }

        public ECMDocumentsValidateAdInterfaceOut GetECMValidateAdInterfaceDocuments(ECMDocumentsValidateAdInterfaceIn eCMDocumentsValidateAdInterfaceIn)
        {
            ECMDocumentsValidateAdInterfaceOut eCMDocumentsValidateAdInterfaceOut = new ECMDocumentsValidateAdInterfaceOut();

            registerEventRepository.SaveRegisterEvent(eCMDocumentsValidateAdInterfaceIn.userId, eCMDocumentsValidateAdInterfaceIn.key, "Log - Start", "Repository.DocumentRepository.GetECMValidateAdInterfaceDocuments", "");

            string path = ConfigurationManager.AppSettings["Sesuite.Physical.Path"];
            int.TryParse(ConfigurationManager.AppSettings["Days"], out int days);

            foreach (var item in Directory.GetFiles(path))
            {
                if (File.GetCreationTime(item).AddDays(days) < DateTime.Now)
                {
                    File.Delete(item);
                }
            }

            registerEventRepository.SaveRegisterEvent(eCMDocumentsValidateAdInterfaceIn.userId, eCMDocumentsValidateAdInterfaceIn.key, "Log - End", "Repository.DocumentRepository.GetECMValidateAdInterfaceDocuments", "");
            return eCMDocumentsValidateAdInterfaceOut;
        }

        public ECMDocumentSaveOut PostECMDocumentSave(ECMDocumentSaveIn ecmDocumentSaveIn)
        {
            ECMDocumentSaveOut ecmDocumentSaveOut = new ECMDocumentSaveOut();

            registerEventRepository.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - Start", "Repository.DocumentRepository.PostECMDocumentSave", "");

            SEDocument.SEDocumentSave(ecmDocumentSaveIn);

            registerEventRepository.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - End", "Repository.DocumentRepository.PostECMDocumentSave", "");
            return ecmDocumentSaveOut;
        }

        private void DownloadFile(string externalId, string path, string archive, string pathFile, int exec, string userId, string key)
        {
            try
            {
                WebClient wc = new WebClient();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                wc.DownloadFile(archive, pathFile);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(userId, key, "Erro", "Repository.DocumentRepository.GetECMDocument", string.Format("ExternalId: {0}. Arquivo: {1}. Erro: {2}", externalId, archive, ex.Message));

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
