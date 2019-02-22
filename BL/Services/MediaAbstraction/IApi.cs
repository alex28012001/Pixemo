using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.MediaAbstraction
{
    public interface IApi:IImagesFunctional,IVideosFunctional
    {
        string AccessToken { get; }
        bool IsAuthorized { get; }
    }
}
