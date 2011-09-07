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
namespace dropkick.Configuration.Dsl.Security.MsSql
{
    using DeploymentModel;
    using Tasks;
    using Tasks.Security.MsSql;

    public class ProtoCreateLoginTask :
        BaseProtoTask
    {
        readonly string _database;
        readonly string _login;

        public ProtoCreateLoginTask(string database, string login)
        {
            _database = database;
            _login = login;
        }

        public override void RegisterRealTasks(PhysicalServer site)
        {
            var task = new CreateLoginTask(site.Name, _database, _login);
            site.AddTask(task);
        }
    }
}