using Model.In;

namespace SoftExpert
{
    public static class SEAttribute
    {
        public static bool SEAttributeUpdate(ECMAttributeIn ecmAttributeIn)
        {
            SEClient seClient = SEConnection.GetConnection();
            string attribute = seClient.setAttributeValue(ecmAttributeIn.externalId, "", ecmAttributeIn.attribute, ecmAttributeIn.value);

            if (attribute.ToUpper().Contains("SUCESSO"))
            {
                return true;
            }
            else
            {
                throw new System.Exception(attribute);
            }
        }
    }
}
