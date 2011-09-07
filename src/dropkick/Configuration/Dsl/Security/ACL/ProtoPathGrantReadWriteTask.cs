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
namespace dropkick.Configuration.Dsl.Security.ACL
{
    using DeploymentModel;
    using FileSystem;
    using Tasks;
    using Tasks.Security.Acl;

    public class ProtoPathGrantReadWriteTask :
        BaseProtoTask
    {
        readonly string _group;
        string _path;

        public ProtoPathGrantReadWriteTask(string path, string group)
        {
            _path = ReplaceTokens(path);
            _group = ReplaceTokens(group);
        }

        public override void RegisterRealTasks(PhysicalServer site)
        {
            var path = PathConverter.Convert(site, _path);
            //path = RemotePathHelper.Convert(site, path);

            var task = new GrantReadWriteTask(path, _group, new DotNetPath());
            site.AddTask(task);
        }
    }
}