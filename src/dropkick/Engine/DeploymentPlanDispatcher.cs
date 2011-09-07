// Copyright 2007-2008 The Apache Software Foundation.
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
namespace dropkick.Engine
{
    using System;
    using System.Collections.Generic;
    using Configuration.Dsl;
    using DeploymentModel;
    using log4net;

    public static class DeploymentPlanDispatcher
    {
        static readonly IDictionary<DeploymentCommands, Action<DeploymentPlan>> _actions = new Dictionary<DeploymentCommands, Action<DeploymentPlan>>();
        static DropkickDeploymentInspector _inspector;
        //static readonly ILog _log = LogManager.GetLogger(typeof(DeploymentPlanDispatcher));

        static DeploymentPlanDispatcher()
        {
            //is this smelly?
            _actions.Add(DeploymentCommands.Execute, d => d.Execute());
            _actions.Add(DeploymentCommands.Verify, d => d.Verify());
            _actions.Add(DeploymentCommands.Trace, d => d.Trace());
        }

        public static void KickItOutThereAlready(Deployment deployment, DeploymentArguments args)
        {
            _inspector = new DropkickDeploymentInspector(args.ServerMappings);

            if (args.Role != "ALL") _inspector.RolesToGet(args.Role.Split(','));

            var plan = _inspector.GetPlan(deployment);

            //HOW TO PLUG IN   args.Role
            //TODO: should be able to block here
            _actions[args.Command](plan);
        }
    }
}