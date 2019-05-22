using Helper.ServerMap;
using Model.In;
using Model.Out;
using SoftExpert;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace Repository
{
    public class JobCategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMJobCategorySaveOut SetECMJobCategorySave(ECMJobCategorySaveIn eCMJobCategorySaveIn)
        {
            ECMJobCategorySaveOut eCMJobCategorySaveOut = new ECMJobCategorySaveOut();

            registerEventRepository.SaveRegisterEvent(eCMJobCategorySaveIn.userId, eCMJobCategorySaveIn.key, "Log - Start", "Repository.JobCategoryRepository.SetECMJobCategorySave", "");

            string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.JobCategoryRepository.Path"]);
            string name = eCMJobCategorySaveIn.FileName;

            if (File.Exists(path + "\\" + name))
            {
                File.Delete(path + "\\" + name);
            }

            File.WriteAllBytes(path + "\\" + name, eCMJobCategorySaveIn.FileBinary);

            SEJobCategory.SEDocumentDeleteOldSaveNew(eCMJobCategorySaveIn);

            registerEventRepository.SaveRegisterEvent(eCMJobCategorySaveIn.userId, eCMJobCategorySaveIn.key, "Log - End", "Repository.JobCategoryRepository.SetECMJobCategorySave", "");
            return eCMJobCategorySaveOut;
        }

        public ECMJobCategoryOut GetECMJobCategory(ECMJobCategoryIn eCMJobCategoryIn)
        {
            ECMJobCategoryOut eCMJobCategoryOut = new ECMJobCategoryOut();
            registerEventRepository.SaveRegisterEvent(eCMJobCategoryIn.userId, eCMJobCategoryIn.key, "Log - Start", "Repository.JobCategoryRepository.GetECMJobCategory", "");

            string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.JobCategoryRepository.Path"]);
            string name = eCMJobCategoryIn.externalId + ".pdf";
            string pathFile = Path.Combine(path, name);

            if (File.Exists(pathFile))
            {
                byte[] archive = File.ReadAllBytes(pathFile);

                eCMJobCategoryOut.result.archive = Convert.ToBase64String(archive);
            }
            else
            {
                eCMJobCategoryOut = SEJobCategory.GetSEJobCategory(eCMJobCategoryIn);

                if (eCMJobCategoryOut.result != null && !string.IsNullOrEmpty(eCMJobCategoryOut.result.archive))
                {
                    try
                    {
                        WebClient wc = new WebClient();

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        wc.DownloadFile(eCMJobCategoryOut.result.archive, pathFile);

                        byte[] archive = File.ReadAllBytes(pathFile);

                        eCMJobCategoryOut.result.archive = Convert.ToBase64String(archive);
                    }
                    catch
                    {
                        throw new Exception(i18n.Resource.FileNotFound);
                    }
                }
                else
                {
                    throw new Exception(i18n.Resource.FileNotFound);
                }
            }

            registerEventRepository.SaveRegisterEvent(eCMJobCategoryIn.userId, eCMJobCategoryIn.key, "Log - End", "Repository.JobCategoryRepository.GetECMJobCategory", "");
            return eCMJobCategoryOut;
        }

        public ECMJobSaveOut PostECMJobSave(ECMJobSaveIn eCMJobSaveIn)
        {
            ECMJobSaveOut ecmJobSaveOut = new ECMJobSaveOut();

            registerEventRepository.SaveRegisterEvent(eCMJobSaveIn.userId, eCMJobSaveIn.key, "Log - Start", "Repository.JobRepository.PostECMJobSave", "");

            SEJobCategory.SEDocumentSave(eCMJobSaveIn);

            registerEventRepository.SaveRegisterEvent(eCMJobSaveIn.userId, eCMJobSaveIn.key, "Log - End", "Repository.JobRepository.PostECMJobSave", "");
            return ecmJobSaveOut;
        }
    }
}
