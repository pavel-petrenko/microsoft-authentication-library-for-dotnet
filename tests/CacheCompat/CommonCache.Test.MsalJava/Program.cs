﻿// ------------------------------------------------------------------------------
// 
// Copyright (c) Microsoft Corporation.
// All rights reserved.
// 
// This code is licensed under the MIT License.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ------------------------------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CommonCache.Test.Common;

namespace CommonCache.Test.MsalJava
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new MsalJavaCacheExecutor().Execute(args);
        }

        private class MsalJavaCacheExecutor : AbstractCacheExecutor
        {
            protected override async Task<CacheExecutorResults> InternalExecuteAsync(CommandLineOptions options)
            {
                var v1App = PreRegisteredApps.CommonCacheTestV1;
                string resource = PreRegisteredApps.MsGraph;
                string scope = resource + "/user.read";

                CommonCacheTestUtils.EnsureCacheFileDirectoryExists();

                // TODO: figure out how we setup the public main program, compile it from .java to .class, and execute it
                // May need to have the JavaLanguageExecutor take a .java file and then have a separate compile
                // step on that class to build the java code and run it.
                string javaClassFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SomeJavaClass.class");

                return await LanguageRunner.ExecuteAsync(
                    new JavaLanguageExecutor(javaClassFilePath),
                    v1App.ClientId,
                        v1App.Authority,
                        scope,
                        options.Username,
                        options.UserPassword,
                        CommonCacheTestUtils.MsalV3CacheFilePath,
                        CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}