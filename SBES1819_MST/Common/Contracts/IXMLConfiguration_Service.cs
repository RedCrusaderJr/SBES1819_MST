using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace Common.Contracts
{
    [ServiceContract]
    public interface IXMLConfiguration_Service
    {
        [OperationContract]
        void XML_Write(List<IXML_Node> nodes);
    }
}
