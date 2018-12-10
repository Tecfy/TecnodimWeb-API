using Helper.ServerMap;
using Model.In;
using Model.Out;
using SoftExpert;
using System.Configuration;

namespace Repository
{
    public class JobCategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMJobCategorySaveOut SetECMJobCategorySave(ECMJobCategorySaveIn eCMJobCategorySaveIn)
        {
            ECMJobCategorySaveOut eCMJobCategorySaveOut = new ECMJobCategorySaveOut();

            registerEventRepository.SaveRegisterEvent(eCMJobCategorySaveIn.userId, eCMJobCategorySaveIn.key, "Log - Start", "Repository.JobCategoryRepository.SetECMJobCategorySave", "");

            SEJobCategory.SEJobCategorySave(eCMJobCategorySaveIn);

            registerEventRepository.SaveRegisterEvent(eCMJobCategorySaveIn.userId, eCMJobCategorySaveIn.key, "Log - End", "Repository.JobCategoryRepository.SetECMJobCategorySave", "");
            return eCMJobCategorySaveOut;
        }

        public ECMJobCategoryOut GetECMJobCategory(ECMJobCategoryIn eCMJobCategoryIn)
        {
            ECMJobCategoryOut eCMJobCategoryOut = new ECMJobCategoryOut();
            registerEventRepository.SaveRegisterEvent(eCMJobCategoryIn.userId, eCMJobCategoryIn.key, "Log - Start", "Repository.JobCategoryRepository.GetECMJobCategory", "");

            string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.JobCategoryRepository.Path"]);
            string name = eCMJobCategoryIn.externalId + ".pdf";

            if (System.IO.File.Exists(path + "\\" + name))
            {
                byte[] archive = System.IO.File.ReadAllBytes(path + "\\" + name);

                eCMJobCategoryOut.result.archive = System.Convert.ToBase64String(archive);
            }
            else
            {
                eCMJobCategoryOut = SEJobCategory.GetSEJobCategory(eCMJobCategoryIn);

                byte[] archive = System.Convert.FromBase64String(eCMJobCategoryOut.result.archive);

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                System.IO.File.WriteAllBytes(path + "\\" + name, archive);
            }

            registerEventRepository.SaveRegisterEvent(eCMJobCategoryIn.userId, eCMJobCategoryIn.key, "Log - End", "Repository.JobCategoryRepository.GetECMJobCategory", "");
            return eCMJobCategoryOut;
        }

        public ECMJobSaveOut PostECMJobSave(ECMJobSaveIn eCMJobSaveIn)
        {
            ECMJobSaveOut ecmJobSaveOut = new ECMJobSaveOut();

            registerEventRepository.SaveRegisterEvent(eCMJobSaveIn.userId, eCMJobSaveIn.key, "Log - Start", "Repository.JobRepository.PostECMJobSave", "");

            SEJobCategory.SEJobSave(eCMJobSaveIn);

            registerEventRepository.SaveRegisterEvent(eCMJobSaveIn.userId, eCMJobSaveIn.key, "Log - End", "Repository.JobRepository.PostECMJobSave", "");
            return ecmJobSaveOut;
        }

        public bool DeleteECMJobArchive(ECMJobDeletedIn eCMJobDeletedIn)
        {
            try
            {
                string path = ServerMapHelper.GetServerMap(ConfigurationManager.AppSettings["Repository.JobRepository.Path"]);
                string name = eCMJobDeletedIn.externalId + ".pdf";

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
