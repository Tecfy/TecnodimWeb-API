using Model.In;
using Model.Out;
using RegisterEvent.Events;
using SoftExpert;

namespace Repository
{
    public class AttributeRepository
    {
        Events events = new Events();

        public ECMAttributeOut ECMAttributeUpdate(ECMAttributeIn ecmAttributeIn)
        {
            ECMAttributeOut ecmAttributeOut = new ECMAttributeOut();
            events.SaveRegisterEvent(ecmAttributeIn.userId, ecmAttributeIn.key, "Log - Start", "Repository.AttributeRepository.ECMAttributeUpdate", "");

            SEAttribute.SEAttributeUpdate(ecmAttributeIn);

            events.SaveRegisterEvent(ecmAttributeIn.userId, ecmAttributeIn.key, "Log - End", "Repository.AttributeRepository.ECMAttributeUpdate", "");
            return ecmAttributeOut;
        }
    }
}
