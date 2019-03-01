using Model.In;

namespace SoftExpert
{
    public static class SEAttribute
    {
        public static bool SEAttributeUpdate(ECMAttributeIn ecmAttributeIn)
        {
            SEClient seClient = SEConnection.GetConnection();

            foreach (var item in ecmAttributeIn.itens)
            {
                string attribute = seClient.setAttributeValue(ecmAttributeIn.externalId, "", item.attribute, item.value);

                if (!attribute.ToUpper().Contains("SUCESSO"))
                {
                    throw new System.Exception(attribute);
                }
            }

            return true;
        }
    }
}
