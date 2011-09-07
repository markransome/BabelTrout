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
namespace dropkick.Tasks.Security.Msmq
{
    using System;
    using System.Messaging;
    using Configuration.Dsl.Msmq;
    using DeploymentModel;

    public class LocalMsmqGrantReadTask :
        BaseSecurityTask
    {
        string _group;
        QueueAddress _address;

        public LocalMsmqGrantReadTask(QueueAddress address, string group)
        {
            _group = group;
            _address = address;
        }

        public LocalMsmqGrantReadTask(PhysicalServer server, string queueName, string group)
        {
            _group = group;
            var ub = new UriBuilder("msmq", server.Name) { Path = queueName };
            _address = new QueueAddress(ub.Uri);
        }



        public override string Name
        {
            get { return "Grant read to '{0}' for queue '{1}'".FormatWith(_group, _address.ActualUri); }
        }

        public override DeploymentResult VerifyCanRun()
        {
            var result = new DeploymentResult();

            VerifyInAdministratorRole(result);

            return result;
        }

        public override DeploymentResult Execute()
        {
            var result = new DeploymentResult();

            Logging.Coarse("[msmq] Setting permissions for '{0}' on local queue '{1}'", _group, _address.ActualUri);

            var q = new MessageQueue(_address.FormatName);
            q.SetPermissions(_group, MessageQueueAccessRights.GetQueueProperties, AccessControlEntryType.Allow);
            q.SetPermissions(_group, MessageQueueAccessRights.GetQueuePermissions, AccessControlEntryType.Allow);

            result.AddGood("Successfully granted Read permissions to '{0}' for queue '{1}'".FormatWith(_group, _address.ActualUri));

            return result;
        }
    }
}