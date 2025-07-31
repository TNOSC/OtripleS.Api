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

string directoryPath = Path.GetDirectoryName(buildScriptPath) ?? throw new InvalidOperationException("path does not exist.");

if (!Directory.Exists(directoryPath))
{
    Directory.CreateDirectory(directoryPath);
}

GitHubPipelineBuilder.CreateNewPipeline()
    .SetName("OtripleS Server Build")
        .OnPush("main")
        .OnPullRequest("main")
            .AddJob("build", job => job
            .WithName("Build")
            .RunsOn(BuildMachines.UbuntuLatest)
                .AddCheckoutStep("Check out")
                .AddSetupDotNetStep(version: "9.0.303")
                .AddRestoreStep()
                .AddBuildStep()
                .AddTestStep(
                    command: "dotnet test --no-build --verbosity normal"))

.SaveToFile(buildScriptPath);
