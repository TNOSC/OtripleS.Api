// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using ADotNet.Clients.Builders;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;

string buildScriptPath = "../../../../../.github/workflows/dotnet.yml";

string directoryPath = Path.GetDirectoryName(path: buildScriptPath) ?? throw new InvalidOperationException(message: "path does not exist.");

if (!Directory.Exists(path: directoryPath))
{
    Directory.CreateDirectory(path: directoryPath);
}

GitHubPipelineBuilder.CreateNewPipeline()
    .SetName(name: "OtripleS Server Build")
        .OnPush(branches: "main")
        .OnPullRequest(branches: "main")
            .AddJob(jobIdentifier: "build", configureJob: job => job
            .WithName(name: "Build")
            .RunsOn(machine: BuildMachines.UbuntuLatest)
                .AddCheckoutStep(name: "Check out")
                .AddSetupDotNetStep(version: "9.0.303")
                .AddRestoreStep()
                .AddBuildStep()
                .AddTestStep(
                    command: "dotnet test --no-build --verbosity normal"))

.SaveToFile(path: buildScriptPath);
