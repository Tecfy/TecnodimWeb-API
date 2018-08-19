using SoftExpert.com.softexpert.tecfy;

namespace SoftExpert
{
    public static class Document
    {
        public static documentDataReturn GetDocumentData(string documentid)
        {
            SEClient seClient = Connection.GetConnection();
            return seClient.viewDocumentData(documentid, "", "");
        }
    }
}
