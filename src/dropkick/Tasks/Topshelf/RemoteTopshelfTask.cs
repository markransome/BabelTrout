﻿// Copyright 2007-2010 The Apache Software Foundation.
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
namespace dropkick.Tasks.Topshelf
{
    using CommandLine;
    using DeploymentModel;
    using Prompting;

    public class RemoteTopshelfTask :
        BaseTask
    {
        readonly RemoteCommandLineTask _task;
        readonly PromptService _prompt = new ConsolePromptService();

        public RemoteTopshelfTask(string exeName, string location, string instanceName, PhysicalServer site, string username, string password)
        {
            string args = string.IsNullOrEmpty(instanceName)
                              ? ""
                              : " /instance:" + instanceName;

            if (username != null && password != null)
            {
                var user = username;
                var pass = password;
                if (username.ShouldPrompt())
                    user = _prompt.Prompt("Win Service '{0}' UserName".FormatWith(exeName));
                if (password.ShouldPrompt())
                    pass = _prompt.Prompt("Win Service '{0}' For User '{1}' Password".FormatWith(exeName, username));

                args += " /username:{0} /password:{1}".FormatWith(user, pass);
            }

            _task = new RemoteCommandLineTask(exeName)
                        {
                            Args = "install" + args,
                            ExecutableIsLocatedAt = location,
                            Machine = site.Name,
                            WorkingDirectory = location
                        };
        }

        public override string Name
        {
            get { return "[topshelf] remote Installing"; }
        }

        public override DeploymentResult VerifyCanRun()
        {
            return _task.VerifyCanRun();
        }

        public override DeploymentResult Execute()
        {
            Logging.Coarse("[topshelf] Installing a remote Topshelf service");
            return _task.Execute();
        }
    }
}