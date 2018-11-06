using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MST
{
    public class XML_Node : IXML_Node
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
