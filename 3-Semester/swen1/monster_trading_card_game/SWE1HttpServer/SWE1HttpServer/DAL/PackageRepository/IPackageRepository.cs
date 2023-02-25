using SWE1HttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.DAL.PackageRepository
{
    public interface IPackageRepository
    {
        bool InsertPackage(IList<Card> package);

        IList<string> GetRandomPackage(User user);

        IList<string> GetFirstPackage(User user);

        bool DeletePackage(string card1);


    }
}
