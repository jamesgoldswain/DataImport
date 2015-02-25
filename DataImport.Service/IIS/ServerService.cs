using System.Collections.Generic;
using System.DirectoryServices;
using Microsoft.Web.Administration;

namespace DataImport.Service.IIS
{
    public class ServerService : IServerService
    {
        public List<string> GetSites(string path)
        {
            var siteList = new List<string>();

            using (ServerManager sm = ServerManager.OpenRemote(path))
            {
                foreach (var site in sm.Sites)
                {
                    siteList.Add(site.Name);
                }
            }

            return siteList;
        }
    }
}
