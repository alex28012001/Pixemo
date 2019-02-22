using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.MediaAbstraction
{
    public interface IMediaService:IApi
    {
        IEnumerable<string> AccessTokens { get; }
        event Action<int ?> IncrementGroupOffset; //событие для отслеживания сдвига групп вк 
    }
}
