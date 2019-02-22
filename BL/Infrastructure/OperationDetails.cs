using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Infrastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool successed, string message, string prop)
        {
            Successed = successed;
            Message = message;
            Property = prop;
        }
        public bool Successed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }
    }
}
