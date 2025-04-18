using System;
using System.Threading;
using PlanSwiftApi.Services;
using PlanSwift9;
using PlanSwiftApi.ApiConfigs;


namespace PlanSwiftApi.Config
{
    public class DependencyManager
    {
        private readonly ApiService _apiService;

        private readonly FilesManager _jobsManager;

        private readonly ApiConfig _apiConfig;


        public FilesManager JobsManager => _jobsManager;

        public ApiService ApiService => _apiService;

        public ApiConfig ApiConfig => _apiConfig;
        






        public DependencyManager(FilesManager jobsManager, ApiService apiService, ApiConfig apiConfig)
        {

            _jobsManager = jobsManager;

            _apiService = apiService;

            _apiConfig = apiConfig;


        }




    }
}
