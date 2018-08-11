using Model.Out;
using Repository.RegisterEvent;
using System;
using System.Web;
using System.Web.Http;

namespace ApiTecnodim.Controllers
{
    [RoutePrefix("api/Documents")]
    public class DocumentsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        [Authorize]
        [Route("")]
        public DocumentOut Get(int documentId)
        {
            DocumentOut documentOut = new DocumentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                //System.IO.File.ReadAllBytes(@"C:\\Temp\\\Tecnodim\\singlePage1.pdf")
                //System.IO.File.ReadAllBytes(@"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - DOCUMENTOS.pdf");
                //System.IO.File.ReadAllBytes(@"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - DOCUMENTOS DIVERSOS.pdf");
                //System.IO.File.ReadAllBytes(@"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - CONTRATOS.pdf");

                byte[] archive = System.IO.File.ReadAllBytes(@"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - CONTRATOS.pdf");

                documentOut.Result.Archive = System.Convert.ToBase64String(archive);

                return documentOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.Get", ex.Message);

                documentOut.SuccessMessage = null;
                documentOut.Messages.Add(i18n.Resource.UnknownError);

                return documentOut;
            }
        }
    }
}
