using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Business.Abstract
{
    public interface ICacheManagementService
    {
        Task<bool> Clear();
    }
}
