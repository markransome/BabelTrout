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
    using System;
    using System.Messaging;
    using Configuration.Dsl.Msmq;
    using DeploymentModel;
    using Exceptions;
    using Tasks.Msmq;

    public class SetSensibleMsmqDefaults :
        BaseSecurityTask
    {
        readonly PhysicalServer _server;
        readonly QueueAddress _address;

        public SetSensibleMsmqDefaults(QueueAddress path)
        {
            _address = path;
        }

        public SetSensibleMsmqDefaults(PhysicalServer server, QueueAddress path)
        {
            _server = server;
            _address = path;
        }

        public override string Name
        {
            get { return "Setting sensible defaults for queue '{0}'".FormatWith(_address.ActualUri); }
        }

        public override DeploymentResult VerifyCanRun()
        {
            var result = new DeploymentResult();

            if (_address.IsLocal)
                VerifyInAdministratorRole(result);
            else
                result.AddAlert("Cannot set permissions for the private remote queue '{0}' while on server '{1}'".FormatWith(_address.ActualUri, Environment.MachineName));


            result.AddGood(Name);
            return result;
        }

        public override DeploymentResult Execute()
        {
            var result = new DeploymentResult();

            if (_address.IsLocal)
                ProcessLocalQueue(result);
            else
                ProcessRemoteQueue(result);

            return result;
        }

        void ProcessLocalQueue(DeploymentResult result)
        {
            Logging.Coarse("[msmq] Setting default permissions for on local queue '{0}'", _address.ActualUri);

            try
            {
                var q = new MessageQueue(_address.LocalName);

                q.SetPermissions(WellKnownSecurityRoles.Administrators, MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
                result.AddGood("Successfully set permissions for '{0}' on queue '{1}'".FormatWith(WellKnownSecurityRoles.Administrators, _address.LocalName));

                q.SetPermissions(WellKnownSecurityRoles.CurrentUser, MessageQueueAccessRights.FullControl, AccessControlEntryType.Revoke);
                result.AddGood("Successfully set permissions for '{0}' on queue '{1}'".FormatWith(WellKnownSecurityRoles.Administrators, _address.LocalName));

                q.SetPermissions(WellKnownSecurityRoles.Everyone, MessageQueueAccessRights.FullControl, AccessControlEntryType.Revoke);
                result.AddGood("Successfully set permissions for '{0}' on queue '{1}'".FormatWith(WellKnownSecurityRoles.Administrators, _address.LocalName));

                q.SetPermissions(WellKnownSecurityRoles.Anonymous, MessageQueueAccessRights.FullControl, AccessControlEntryType.Revoke);
                result.AddGood("Successfully set permissions for '{0}' on queue '{1}'".FormatWith(WellKnownSecurityRoles.Administrators, _address.LocalName));
            }
            catch (MessageQueueException ex)
            {
                if (ex.Message.Contains("does not exist"))
                {
                    var msg = "The queue '{0}' doesn't exist.";
                    throw new DeploymentException(msg, ex);
                }
                throw;
            }


        }

        void ProcessRemoteQueue(DeploymentResult result)
        {
            VerifyInAdministratorRole(result);

            Logging.Coarse("[msmq][remote] Setting default permissions on remote queue '{0}'.", _address.ActualUri);

            using (var remote = new RemoteDropkickExecutionTask(_server))
            {
                var vresult = remote.GrantMsmqPermission(QueuePermission.SetSensibleDefaults, _address, WellKnownSecurityRoles.Administrators);
                foreach (var r in vresult) result.Add(r);
            }

        }

    }
}