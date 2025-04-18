using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace PlanSwiftApi.Services
{
    public interface IFilesManager
    {


        void FindJobPath();
        List<string> FindStorages();
        void SaveJob();
        string GetProcessTitle();
    }
}