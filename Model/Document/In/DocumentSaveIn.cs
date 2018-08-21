namespace Model.In
{
    public class SEDocumentSaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string category { get; set; }

        public byte[] archive { get; set; }

        public string archiveName { get; set; }
    }
}
