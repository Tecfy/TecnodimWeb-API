namespace WebService.SE
{
    interface IDocumento<t>
    {
        bool inserirDocumento(t Indice);
        bool uploadDocumento(t Indice, string id);
    }
}
