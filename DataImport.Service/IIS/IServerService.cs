using System.Collections.Generic;

namespace DataImport.Service.IIS
{
    public interface IServerService
    {

        List<string> GetSites(string path);
    }
}
