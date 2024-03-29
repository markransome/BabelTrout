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
using dropkick.Tasks.CommandLine;

namespace dropkick.Tasks.Security.Msmq
{
    using Configuration.Dsl.Msmq;
    using DeploymentModel;
    using Tasks.Msmq;

    public class RemoteMsmqGrantReadTask :
        BaseSecurityTask
    {
        readonly PhysicalServer _server;
        string _group;
        QueueAddress _address;

        public RemoteMsmqGrantReadTask(QueueAddress address, string group)
        {
            _group = group;
            _address = address;
        }

        public RemoteMsmqGrantReadTask(PhysicalServer server, QueueAddress address, string group)
        {
            _server = server;
            _address = address;
            _group = group;
        }



        public override string Name
        {
            get { return "Grant read to '{0}' for queue '{1}'".FormatWith(_group, _address.ActualUri); }
        }

        public override DeploymentResult VerifyCanRun()
        {
            var result = new DeploymentResult();

            //TODO add meaningful verification

            return result;
        }

        public override DeploymentResult Execute()
        {
            var result = new DeploymentResult();

            Logging.Coarse("[msmq][remote] Setting permission for '{0}' on remote queue '{1}'.", _group, _address.ActualUri);

            using (var remote = new RemoteDropkickExecutionTask(_server))
            {
                remote.GrantMsmqPermission(QueuePermission.Read, _address, _group);
            }

            return result;
        }
    }
}