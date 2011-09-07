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
namespace dropkick.Configuration.Dsl.Security
{
    using System;
    using ACL;
    using LocalPolicy;
    using Msmq;
    using MsSql;
    using Certificate;

    public interface SecurityOptions
    {
        void LocalPolicy(Action<LocalPolicyConfig> func);
        void ForPath(string path, Action<FileSecurityConfig> action);
        void ForQueue(string queue, Action<QueueSecurityConfig> action);
        void ForSqlServer(string database, Action<MsSqlSecurity> action);
        void ForCertificate(string thumbprint, Action<CertificateSecurityConfig> action);
    }
}