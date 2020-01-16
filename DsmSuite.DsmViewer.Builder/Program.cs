﻿using System.Diagnostics;
using System.IO;
using System.Reflection;
using DsmSuite.Common.Util;
using DsmSuite.DsmViewer.Application.Core;
using DsmSuite.DsmViewer.Builder.Settings;
using DsmSuite.DsmViewer.Model.Core;
using System;

namespace DsmSuite.DsmViewer.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LogAssemblyInfo(Assembly.GetExecutingAssembly());

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (args.Length < 1)
            {
                Logger.LogUserMessage("Usage: DsmSuite.DsmViewer.Builder <settingsfile>");
            }
            else
            {
                FileInfo settingsFileInfo = new FileInfo(args[0]);
                if (!settingsFileInfo.Exists)
                {
                    BuilderSettings.WriteToFile(settingsFileInfo.FullName, BuilderSettings.CreateDefault());
                    Logger.LogUserMessage("Settings file does not exist. Default one created");
                }
                else
                {
                    BuilderSettings builderSettings = BuilderSettings.ReadFromFile(settingsFileInfo.FullName);
                    Logger.EnableLogging(Assembly.GetExecutingAssembly(), builderSettings.LoggingEnabled);

                    if (!File.Exists(builderSettings.InputFilename))
                    {
                        Logger.LogUserMessage($"Input file '{builderSettings.InputFilename}' does not exist.");
                    }
                    else
                    {
                        var progress = new Progress<ProgressInfo>(p =>
                        {
                            Update(p);
                        });

                        DsmModel model = new DsmModel("Builder", Assembly.GetExecutingAssembly());
                        DsmApplication application = new DsmApplication(model);
                        application.ImportModel(builderSettings.InputFilename, 
                                                builderSettings.OutputFilename, 
                                                builderSettings.ApplyPartitioningAlgorithm, 
                                                builderSettings.OverwriteOutputFile, 
                                                builderSettings.CompressOutputFile,
                                                progress);
                    }
                }
            }

            Logger.LogResourceUsage();

            stopWatch.Stop();
            Logger.LogUserMessage($" total elapsed time = {stopWatch.Elapsed}");
        }

        private static void Update(ProgressInfo progress)
        {
            Console.Write($"\r {progress.ActionText} progress={progress.Progress:000}%");
        }
    }
}
