using Model.In;
using Model.Out;
using SoftExpert;

namespace Repository
{
    public class AttributeRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMAttributeOut ECMAttributeUpdate(ECMAttributeIn ecmAttributeIn)
        {
            ECMAttributeOut ecmAttributeOut = new ECMAttributeOut();
            registerEventRepository.SaveRegisterEvent(ecmAttributeIn.userId, ecmAttributeIn.key, "Log - Start", "Repository.AttributeRepository.ECMAttributeUpdate", "");

            SEAttribute.SEAttributeUpdate(ecmAttributeIn);

            registerEventRepository.SaveRegisterEvent(ecmAttributeIn.userId, ecmAttributeIn.key, "Log - End", "Repository.AttributeRepository.ECMAttributeUpdate", "");
            return ecmAttributeOut;
        }
    }
}
