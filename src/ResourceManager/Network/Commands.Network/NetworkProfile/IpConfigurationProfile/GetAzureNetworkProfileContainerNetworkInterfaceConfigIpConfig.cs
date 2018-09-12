﻿// <auto-generated>
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// 
// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.
// 
// For documentation on code generator please visit
//   https://aka.ms/nrp-code-generation
// Please contact wanrpdev@microsoft.com if you need to make changes to this file.
// </auto-generated>

using Microsoft.Azure.Commands.Network.Models;
using Microsoft.Azure.Management.Network.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet(VerbsCommon.Get, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "NetworkProfileContainerNetworkInterfaceConfigIpConfig"), OutputType(typeof(PSContainerNetworkInterfaceConfiguration))]
    public partial class GetAzureNetworkProfileContainerNetworkInterfaceConfigIpConfigCommand : NetworkBaseCmdlet
    {
        [Parameter(
            Mandatory = true,
            HelpMessage = "The reference of the network profile resource.",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public PSContainerNetworkInterfaceConfiguration ContainerNetworkInterfaceConfiguration { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Name of the container network interface configuration.")]
        public string Name { get; set; }

        public override void Execute()
        {
            base.Execute();

            if (!string.IsNullOrEmpty(this.Name))
            {
                var vIPConfigProfile =
                        this.ContainerNetworkInterfaceConfiguration.IpConfigurations.First(
                            resource =>
                                string.Equals(resource.Name, this.Name, System.StringComparison.CurrentCultureIgnoreCase));
                WriteObject(vIPConfigProfile);
            }
            else
            {
                WriteObject(ContainerNetworkInterfaceConfiguration.IpConfigurations, true);
            }
        }
    }
}
