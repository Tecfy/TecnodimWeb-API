using Helper.ServerMap;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

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

            if (File.Exists(path + "\\" + name))
            {
                byte[] archive = File.ReadAllBytes(path + "\\" + name);

                ecmDocumentOut.result.archive = Convert.ToBase64String(archive);
            }
            else
            {
                ecmDocumentOut = SEDocument.GetSEDocument(ecmDocumentIn);

                if (ecmDocumentOut.result != null)
                {
                    byte[] archive = Convert.FromBase64String(ecmDocumentOut.result.archive);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    File.WriteAllBytes(path + "\\" + name, archive);
                }
                else
                {
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

        public bool DeleteECMDocumentArchive(ECMDocumentDeletedIn ecmDocumentDeletedIn)
        {
            try
            {
                string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.DocumentRepository.Path"]);
                string name = ecmDocumentDeletedIn.externalId + ".pdf";

                if (File.Exists(path + "\\" + name))
                {
                    File.Delete(path + "\\" + name);
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
