// ----------------------------------------------------------------------------------
// Copyright (c) Tunisian .NET Open Source Community (TNOSC). All rights reserved.
// This code is provided by TNOSC and is freely available under the MIT License.
// Author: Ahmed HEDFI (ahmed.hedfi@gmail.com)
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using ADotNet.Clients.Builders;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;

namespace Tnosc.OtripleS.Server.Build.Services;

internal static class ScriptGenerationService
{
    public static void GenerateBuildScript()
    {
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
                    .RunsOn(machine: BuildMachines.WindowsLatest)
                        .AddCheckoutStep(name: "Check out")
                        .AddSetupDotNetStep(version: "9.0.303")
                        .AddRestoreStep()
                        .AddBuildStep()
                        .AddTestStep(
                            command: "dotnet test --no-build --verbosity normal"))

        .SaveToFile(path: buildScriptPath);
    }

    public static void GenerateProvisionScript()
    {
        string buildScriptPath = "../../../../../.github/workflows/provision.yml";

        string directoryPath = Path.GetDirectoryName(path: buildScriptPath) ?? throw new InvalidOperationException(message: "path does not exist.");

        if (!Directory.Exists(path: directoryPath))
        {
            Directory.CreateDirectory(path: directoryPath);
        }

        GitHubPipelineBuilder.CreateNewPipeline()
            .SetName(name: "OtripleS Server Provision")
                .OnPush(branches: "main")
                .OnPullRequest(branches: "main")
                    .AddJob(jobIdentifier: "build", configureJob: job => job
                    .WithName(name: "Provision")
                    .AddEnvironmentVariable("AZURE_CLIENT_ID", "${{ secrets.AZURE_CLIENT_ID }}")
                    .AddEnvironmentVariable("AZURE_TENANT_ID", "${{ secrets.AZURE_TENANT_ID }}")
                    .AddEnvironmentVariable("AZURE_CLIENT_SECRET", "${{ secrets.AZURE_CLIENT_SECRET }}")
                    .AddEnvironmentVariable("AzureAdminName", "${{ secrets.AzureAdminName }}")
                    .AddEnvironmentVariable("AzureAdminAccess", "${{ secrets.AzureAdminAccess }}")
                    .RunsOn(machine: BuildMachines.UbuntuLatest)
                        .AddCheckoutStep(name: "Check out")
                        .AddSetupDotNetStep(version: "9.0.303")
                        .AddRestoreStep()
                        .AddBuildStep()
                        .AddGenericStep(
                            name: "Provision",
                            runCommand: "dotnet run --project ./devops/Tnosc.OtripleS.Server.Provision/Tnosc.OtripleS.Server.Provision.csproj"))

        .SaveToFile(path: buildScriptPath);
    }
}
