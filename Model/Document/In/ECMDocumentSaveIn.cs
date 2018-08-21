namespace Model.In
{
    public class ECMDocumentSaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string category { get; set; }

        public byte[] archive { get; set; }

        public string title { get; set; }
    }
}
