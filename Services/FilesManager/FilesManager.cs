using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.XPath;
using System.Data.SqlTypes;
using System.Windows.Documents;
using System.Collections.Generic;
using PlanSwiftApi.Helpers;

namespace PlanSwiftApi.Services
{
    public class FilesManager : IFilesManager
    {
        public bool PathFound { get; private set; }

        JsonManager _jsonManager;


        public FilesManager(JsonManager jsonManager)
        {

            _jsonManager = jsonManager;
            return;
        }

        public List<string> FindStorages()
        {
            string[] stgs;

            List<string> storages = new List<string>();
            List<string> aux = new List<string>();

            string storagesPath = _jsonManager.GetConfigs() + Constants.Storages;
            if (Directory.Exists(storagesPath))
            {
                Console.WriteLine("se encontro el path");
                stgs = Directory.GetDirectories(storagesPath);

                PathFound = true;

                foreach (var st in stgs)
                {
                    var storage = Path.GetFileName(st);

                    if (storage.ToLowerInvariant().Trim() == "local")
                        storages.Add(storage);
                    else
                        aux.Add(storage);
                }

                storages.AddRange(aux);
            }
            else
            {

                PathFound = false;
                //Function to select another folder path


            }

            foreach (var test in storages)
            {
                Console.WriteLine(test);
            }
            return storages;
        }

        public string GetProcessTitle()
        {
            Process[] processes = Process.GetProcessesByName("PlanSwift");  // Nombre del proceso

            string jobName = null;

            if (processes.Length > 0)
            {
                Process planSwiftProcess = processes[0];

                int n = planSwiftProcess.MainWindowTitle.IndexOf("-");

                jobName = planSwiftProcess.MainWindowTitle.Substring(n + 1);


                
            }

            return (jobName);

        }




        public void FindJobPath()
        {

            

            string path = "C:\\Program Files (x86)\\PlanSwift11\\Data\\Storages\\New FolderStorage\\Data.xml";

            if(path != "C:\\Program Files (x86)\\PlanSwift11\\Data\\Storages\\Local\\Data.xml")
            {
                XmlDocument doc = new XmlDocument();




                doc.Load(path);



                XmlNode nodo = doc.SelectSingleNode("/Item");

                if (nodo != null)
                {

                    XmlNodeList properties = doc.SelectNodes("/Item/Properties/Property");


                    if (properties != null)
                    {
                        foreach (XmlNode property in properties)
                        {
                            if (property.Attributes != null)
                            {

                                string propertyName = property.Attributes["Name"]?.Value ?? "Sin nombre";
                                string propertyValue = property.InnerText.Trim();

                                if (propertyName == "Folder")
                                {
                                    Console.WriteLine(propertyValue);
                                }
                            }
                        }
                    }
                }
            }

            








            // func to manual set path
        }


        /// 

        public void SaveJob()
        {
            int i = 1;
            List<string> storages;
            string pathDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string teamSwiftPath = Path.Combine(pathDocuments, "TeamSwift", "Saves");
            string jobName = GetProcessTitle().TrimStart();
            if (!Directory.Exists(teamSwiftPath))
            {
                Directory.CreateDirectory(teamSwiftPath);
            }

            storages = FindStorages();

            string storagePath = storages[0] + "\\jobs\\" + jobName;

            string jobPath = Path.Combine(teamSwiftPath, "jobs", jobName + $"{i:D3}");


            Console.WriteLine(jobName);
            if (jobName != "PlanSwift")
            {
                while (Directory.Exists(jobPath))
                {
                    i++;
                    jobPath = Path.Combine(teamSwiftPath, "jobs", jobName + $"{i:D3}");

                }


                Directory.CreateDirectory(jobPath);
                FileSystem.CopyDirectory(storagePath, jobPath, true);
                
            }
            
            
            /*
            if (jobName != "PlanSwift")
            {
                foreach (string job in jobsPath)
                {
                    if(job == jobName)
                    {

                    }
                }
            }

            */


            //Directory.GetDirectories(storages[0]); 





            

        }



    }
}