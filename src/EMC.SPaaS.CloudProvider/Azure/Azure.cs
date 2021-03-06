﻿using EMC.SPaaS.CloudProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMC.SPaaS.DesignManager;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure;
using EMC.SPaaS.AuthenticationProviders;
using Microsoft.Extensions.Configuration;
using EMC.SPaaS.Utility;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Storage;
using Microsoft.WindowsAzure.Management.Storage.Models;
using Microsoft.WindowsAzure.Management.Models;
using EMC.SPaaS.Entities;
using System.Text;
using System.Data;
using System.Dynamic;
using Newtonsoft.Json;

namespace EMC.SPaaS.CloudProvider
{
    public class Azure : ICloudProvider
    {
        //TODO:CONFIG
        readonly string storageAccountName;

        SubscriptionCloudCredentials Credentials { get; set; }

        IAuthenticationProvider OAuthProvider { get; set; }

        public Azure(IConfigurationSection configuration, string token)
        {
            OAuthProvider = new AzureAdOAuthProvider(configuration);

            storageAccountName = configuration[GlobalConstants.CloudProviders.Azure.ConfigurationKeys.StorageAccountName];

            Credentials = new TokenCloudCredentials(
                configuration[GlobalConstants.CloudProviders.Azure.ConfigurationKeys.SubscriptionId],
                OAuthProvider.GetApiAccessToken(token)
            );
        }

        public void CreateVM(InstanceEntity instance)
        {
            #region vars
            //string hostedService = instance.Name;//"spaashost";
            //string hostedServiceLabel = instance.Name;//"spaas";

            //string vmName = vmDesign.Name;//"vm1";
            //string vmUserName = vmDesign.UserName;//"SPaaSAdmin";
            //string vmPassword = vmDesign.Password;//"Welcome@123";
            //string vmSize = vmDesign.Type;

            //string deploymentName = instance.Name + vmDesign.Name;// "testdeploy"; 
            #endregion

            try
            {
                #region init
                //DONE Works!
                //storage account
                //using (var storageClient = new StorageManagementClient(Credentials))
                //{
                //    storageClient.StorageAccounts.Create(new StorageAccountCreateParameters
                //    {
                //        Label = "SPaaS Storage Account",
                //        Location = LocationNames.WestUS,
                //        Name = storageAccountName,
                //        AccountType = StorageAccountTypes.StandardLRS
                //    });
                //}


                //DONE Works!
                //create cloud service
                //using (var computeClient = new ComputeManagementClient(Credentials))
                //{
                //    computeClient.HostedServices.Create(new HostedServiceCreateParameters
                //    {
                //        Label = hostedServiceLabel,
                //        Location = LocationNames.WestUS,
                //        ServiceName = hostedService
                //    });
                //} 
                #endregion


                //create vm
                using (var computeClient = new ComputeManagementClient(Credentials))
                {
                    var operatingSystemImageListResult = computeClient.VirtualMachineOSImages.List().Images;

                    //DataTable table = new DataTable();
                    //table.Columns.Add("AffinityGroup");
                    //table.Columns.Add("Category");
                    //table.Columns.Add("Description");
                    //table.Columns.Add("Eula");
                    //table.Columns.Add("ImageFamily");
                    //table.Columns.Add("IsPremium");
                    //table.Columns.Add("Label");
                    //table.Columns.Add("Language");
                    //table.Columns.Add("Location");
                    //table.Columns.Add("LogicalSizeInGB");
                    //table.Columns.Add("MediaLinkUri");
                    //table.Columns.Add("Name");
                    //table.Columns.Add("OperatingSystemType");
                    //table.Columns.Add("PricingDetailUri");
                    //table.Columns.Add("PrivacyUri");
                    //table.Columns.Add("PublishedDate");
                    //table.Columns.Add("PublisherName");
                    //table.Columns.Add("RecommendedVMSize");
                    //table.Columns.Add("SmallIconUri");

                    //foreach(var os in operatingSystemImageListResult)
                    //{
                    //    table.Rows.Add(os.AffinityGroup, os.Category, os.Description, os.Eula, os.ImageFamily, os.IsPremium, os.Label, os.Language, os.Location,
                    //    os.LogicalSizeInGB, os.MediaLinkUri, os.Name, os.OperatingSystemType, os.PricingDetailUri, os.PrivacyUri, os.PublishedDate, os.PublisherName, os.RecommendedVMSize, os.SmallIconUri);
                    //}

                    var vmRoles = GetAzureRolesForVmDesignes(instance, operatingSystemImageListResult);

                    #region commented

                    //Configurable??
                    //var imageName = operatingSystemImageListResult.FirstOrDefault(os => os.Label.Contains(vmDesign.Name)).Name;

                    ////OS config
                    //var windowsConfigSet = new ConfigurationSet
                    //{
                    //    ConfigurationSetType = ConfigurationSetTypes.WindowsProvisioningConfiguration,
                    //    AdminPassword = vmPassword,
                    //    AdminUserName = vmUserName,
                    //    ComputerName = vmName,
                    //    HostName = string.Format("{0}.cloudapp.net", hostedService)
                    //};

                    ////remote powershell and rdp
                    //var networkConfigSet = new ConfigurationSet
                    //{
                    //    ConfigurationSetType = "NetworkConfiguration",
                    //    InputEndpoints = new List<InputEndpoint>
                    //      {
                    //        new InputEndpoint
                    //        {
                    //          Name = "PowerShell",
                    //          LocalPort = 5986,
                    //          Protocol = "tcp",
                    //          Port = 5986,
                    //        },
                    //        new InputEndpoint
                    //        {
                    //          Name = "Remote Desktop",
                    //          LocalPort = 3389,
                    //          Protocol = "tcp",
                    //          Port = 3389,
                    //        }
                    //      }
                    //};

                    ////virtual harddisk
                    //var vhd = new OSVirtualHardDisk
                    //{
                    //    SourceImageName = imageName,
                    //    HostCaching = VirtualHardDiskHostCaching.ReadWrite,
                    //    MediaLink = new Uri(string.Format("https://{0}.blob.core.windows.net/vhds/{1}.vhd", storageAccountName, imageName))
                    //};

                    ////vm configuration
                    //var vmAttributes = new Role
                    //{
                    //    RoleName = vmName,

                    //    //Make configurable
                    //    RoleSize = vmSize,//VirtualMachineRoleSize.Small,
                    //    RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                    //    OSVirtualHardDisk = vhd,
                    //    ConfigurationSets = new List<ConfigurationSet> { windowsConfigSet, networkConfigSet },

                    //    //Optional?
                    //    ProvisionGuestAgent = true
                    //}; 
                    #endregion

                    //deployment config
                    var deploymentParameters = new VirtualMachineCreateDeploymentParameters
                    {
                        Name = instance.Design.DesignName + instance.Name,
                        Label = instance.Design.DesignName + instance.Name,
                        DeploymentSlot = DeploymentSlot.Production,
                        Roles = vmRoles
                    };

                    //create VMs
                    var deploymentResult = computeClient.VirtualMachines.CreateDeployment(instance.Name, deploymentParameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Role> GetAzureRolesForVmDesignes(InstanceEntity instance, IList<VirtualMachineOSImageListResponse.VirtualMachineOSImage> images)
        {
            List<Role> roles = new List<Role>();

            foreach (var vmDesign in instance.Design.VMs)
            {
                var imageName = (from os in images where os.Label.Contains(vmDesign.OS) orderby os.PublishedDate descending select os).FirstOrDefault();
                //var imageName = images.FirstOrDefault(os => os.Label.Contains(vmDesign.OS)).Name;

                //OS config
                var windowsConfigSet = new ConfigurationSet
                {
                    ConfigurationSetType = ConfigurationSetTypes.WindowsProvisioningConfiguration,
                    AdminPassword = vmDesign.Password,
                    AdminUserName = vmDesign.UserName,
                    ComputerName = vmDesign.Name,
                    HostName = string.Format("{0}.cloudapp.net", instance.Name)
                };

                //remote powershell and rdp
                var remoteSettings = new WindowsRemoteManagementSettings();
                var httpListner = new WindowsRemoteManagementListener(VirtualMachineWindowsRemoteManagementListenerType.Http);
                remoteSettings.Listeners.Add(httpListner);
                var networkConfigSet = new ConfigurationSet
                {
                    ConfigurationSetType = "NetworkConfiguration",
                    InputEndpoints = new List<InputEndpoint>
                          {
                            new InputEndpoint
                            {
                              Name = "PowerShell",
                              LocalPort = 5985,
                              Protocol = "tcp",
                              Port = 5985,
                            },
                            new InputEndpoint
                            {
                              Name = "Remote Desktop",
                              LocalPort = 3389,
                              Protocol = "tcp",
                              Port = 3389,
                            }
                          },
                    WindowsRemoteManagement = remoteSettings
                };

                //virtual harddisk
                var vhd = new OSVirtualHardDisk
                {
                    SourceImageName = imageName.Name,
                    HostCaching = VirtualHardDiskHostCaching.ReadWrite,
                    MediaLink = new Uri(string.Format("https://{0}.blob.core.windows.net/vhds/{1}.vhd", storageAccountName, imageName.Name))
                };

                //vm configuration
                var vmAttributes = new Role
                {
                    RoleName = vmDesign.Name,

                    //Make configurable
                    RoleSize = vmDesign.Type,//VirtualMachineRoleSize.Small,
                    RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                    OSVirtualHardDisk = vhd,

                    //VMImageName = imageName.Name,
                    ConfigurationSets = new List<ConfigurationSet> { windowsConfigSet, networkConfigSet },

                    //Optional?
                    ProvisionGuestAgent = true
                };

                roles.Add(vmAttributes);
            }

            return roles;
        }

        public bool UpdateVMDetailsIfInstanceRunning(InstanceEntity instance)
        {
            using (var computeClient = new ComputeManagementClient(Credentials))
            {
                var vmdeployment = computeClient.Deployments.GetByName(instance.Name, instance.Design.DesignName + instance.Name);
                var operationStatus = computeClient.GetOperationStatus(vmdeployment.RequestId).Status;

                if (operationStatus == OperationStatus.Failed)
                    throw new Exception("Unknown error...");

                if (operationStatus == OperationStatus.Succeeded)
                {
                    var zippedVmIp = instance.VMs.Zip(vmdeployment.VirtualIPAddresses, (vm, ip) =>
                    {
                        return new { VM = vm, IP = ip.Address };
                    });

                    foreach (var vmIp in zippedVmIp)
                    {
                        vmIp.VM.IP = vmIp.IP;
                    }

                    return true;
                }

                return false;
            }
        }

        public bool IsDeployedInstanceOff(InstanceEntity instance)
        {
            using (var computeClient = new ComputeManagementClient(Credentials))
            {
                var vmdeployment = computeClient.Deployments.GetByName(instance.Name, instance.Design.DesignName + instance.Name);
                return vmdeployment.Status == DeploymentStatus.Suspended;
            }
        }

        public bool DeleteVM(ProvisionedVmEntity vm)
        {
            throw new NotImplementedException();
        }

        public bool TurnOnVM(ProvisionedVmEntity vm)
        {
            throw new NotImplementedException();
        }

        public bool TurnOffVM(ProvisionedVmEntity vm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Server> GetAvailableVMOptions()
        {
            List<Server> serverOptionsList = new List<Server>(3);

            //Not Real
            serverOptionsList.Add(new Server(Name: VirtualMachineRoleSize.ExtraSmall, Processors: "shared core", RAM: "768 MB"));
            serverOptionsList.Add(new Server(Name: VirtualMachineRoleSize.Small, Processors: "1 core", RAM: "1.75 GB"));
            serverOptionsList.Add(new Server(Name: VirtualMachineRoleSize.Medium, Processors: "2 cores", RAM: "3.5 GB"));
            serverOptionsList.Add(new Server(Name: VirtualMachineRoleSize.Large, Processors: "4 cores", RAM: "7 GB"));
            serverOptionsList.Add(new Server(Name: VirtualMachineRoleSize.ExtraLarge, Processors: "8 cores", RAM: "14 GB"));
            serverOptionsList.Add(new Server(Name: VirtualMachineRoleSize.A5, Processors: "2 cores", RAM: "14 GB"));
            serverOptionsList.Add(new Server(Name: VirtualMachineRoleSize.A6, Processors: "4 cores", RAM: "28 GB"));
            serverOptionsList.Add(new Server(Name: VirtualMachineRoleSize.A7, Processors: "8 cores", RAM: "56 GB"));
            //serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.A8, Processors = 2, RAM = 3 });
            //serverOptionsList.Add(new AzureServer() { Name = VirtualMachineRoleSize.A9, Processors = 2, RAM = 3 });

            return serverOptionsList.ToArray();
        }

        public void Initialize(InstanceEntity instance)
        {
            string hostedService = instance.Name;//"spaashost";
            string hostedServiceLabel = instance.Name;//"spaas";

            //create cloud service
            using (var computeClient = new ComputeManagementClient(Credentials))
            {
                try
                {
                    var result = computeClient.HostedServices.Create(new HostedServiceCreateParameters
                    {
                        Label = hostedServiceLabel,
                        Location = LocationNames.EastAsia,
                        ServiceName = hostedService
                    });

                    var id = result.RequestId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public IEnumerable<Server> VMOptions()
        {
            return GetAvailableVMOptions();
        }

        public IEnumerable<string> OSImageOptions()
        {
            using (var computeClient = new ComputeManagementClient(Credentials))
            {
                var operatingSystemImageListResult = computeClient.VirtualMachineOSImages.List().Images;
                return (from os in operatingSystemImageListResult select os.Label).AsEnumerable();
            }
        }
    }
}

