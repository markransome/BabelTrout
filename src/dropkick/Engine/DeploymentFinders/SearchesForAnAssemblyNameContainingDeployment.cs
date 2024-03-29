// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace dropkick.Engine.DeploymentFinders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Configuration.Dsl;

    public class SearchesForAnAssemblyNameContainingDeployment :
        DeploymentFinder
    {
        public Deployment Find(string assemblyName)
        {
            var pathToSearch = AppDomain.CurrentDomain.BaseDirectory;
            var ass = Directory.GetFiles(pathToSearch);

            var deploymentAssemblyCandidates = ass.Where(x => x.Contains("Deployment"));

            if (deploymentAssemblyCandidates.Count() == 0)
                return new NullDeployment();

            return new AssemblyWasSpecifiedAssumingOnlyOneDeploymentClass().Find(deploymentAssemblyCandidates.First());
        }

        public string Name
        {
            get { return "Searched for assembly with 'Deployment' in its name."; }
        }
    }

    public class NullDeployment :
        Deployment
    {
        public void InspectWith(DeploymentInspector inspector)
        {
            throw new NotImplementedException();
        }

        public bool HardPrompt
        {
            get { return false; }
        }

        public void Initialize(object settings)
        {
            throw new NotImplementedException();
        }
    }
}