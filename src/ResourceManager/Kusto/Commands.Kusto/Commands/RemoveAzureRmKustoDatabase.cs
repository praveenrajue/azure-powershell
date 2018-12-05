﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Management.Automation;
using System.Security.Permissions;
using Microsoft.Azure.Commands.Kusto.Models;
using Microsoft.Azure.Commands.Kusto.Properties;
using Microsoft.Azure.Commands.Kusto.Utilities;

namespace Microsoft.Azure.Commands.Kusto.Commands
{
    
    [Cmdlet(VerbsCommon.Remove, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "KustoDatabase", SupportsShouldProcess = true, DefaultParameterSetName = CmdletParametersSet),
      OutputType(typeof(PSKustoDatabase))]
    public class RemoveAzureRmKustoDatabase : KustoCmdletBase
    {
        protected const string CmdletParametersSet = "ByNameAndResourceGroup";
        protected const string ObjectParameterSet = "ByInputObject";
        protected const string ResourceIdParameterSet = "ByResourceId";

        [Parameter(
            ParameterSetName = CmdletParametersSet,
            Position = 0,
            Mandatory = true,
            HelpMessage = "Name of database to be removed.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(
            ParameterSetName = CmdletParametersSet,
            Position = 0,
            Mandatory = true,
            HelpMessage = "Name of the cluster under which the database exists.")]
        [ValidateNotNullOrEmpty]
        public string ClusterName { get; set; }

        [Parameter(
            ParameterSetName = CmdletParametersSet,
            Mandatory = false,
            HelpMessage = "Name of resource group under which the cluster exists.")]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            ParameterSetName = ResourceIdParameterSet,
            Mandatory = true,
            Position = 1,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Kusto database ResourceID.")]
        [ValidateNotNullOrEmpty]
        public string ResourceId { get; set; }

        [Parameter(
            ParameterSetName = ObjectParameterSet,
            Mandatory = true,
            Position = 2,
            ValueFromPipeline = true,
            HelpMessage = "Kusto database object.")]
        [ValidateNotNullOrEmpty]
        public PSKustoDatabase InputObject { get; set; }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override void ExecuteCmdlet()
        {
            string databaseName = Name;
            string clusterName = ClusterName;
            string resourceGroupName = ResourceGroupName;

            if (!string.IsNullOrEmpty(ResourceId))
            {
                KustoUtils.GetResourceGroupNameClusterNameAndDatabaseNameFromDatabaseId(ResourceId, out resourceGroupName, out clusterName, out databaseName);
            }
            else if (InputObject != null)
            {
                KustoUtils.GetResourceGroupNameClusterNameAndDatabaseNameFromDatabaseId(InputObject.Id, out resourceGroupName, out clusterName, out databaseName);
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                WriteExceptionError(new PSArgumentNullException("Name", "Name of cluster not specified"));
            }

            if (ShouldProcess(databaseName, Resources.RemovingKustoDatabase))
            {
                PSKustoDatabase database = null;
                if (!KustoClient.CheckIfDatabaseExists(resourceGroupName, clusterName, databaseName, out database))
                {
                    throw new InvalidOperationException(string.Format(Resources.KustoDatabaseNotExist, databaseName));
                }

                KustoClient.DeleteDatabase(resourceGroupName, clusterName, databaseName);
            }
        }
    }

}
