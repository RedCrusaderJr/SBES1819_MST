using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class XML_Node
    {
        string userId;
        string userGroup;
        string processName;

        public XML_Node(string userId, string userGroup, string processName)
        {
            this.userId = userId;
            this.userGroup = userGroup;
            this.processName = processName;
        }

        [DataMember]
        public string UserId {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        [DataMember]
        public string UserGroup
        {
            get
            {
                return userGroup;
            }
            set
            {
                userGroup = value;
            }
        }

        [DataMember]
        public string ProcessName
        {
            get
            {
                return processName;
            }
            set
            {
                processName = value;
            }
        }
    }
}
