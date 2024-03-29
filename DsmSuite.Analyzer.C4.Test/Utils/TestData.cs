﻿using System;
using System.IO;

namespace DsmSuite.Analyzer.C4.Test.Utils
{
    class TestData
    {
        public static string RootDirectory
        {
            get
            {
                string testData = "Examples";
                string pathExecutingAssembly = AppDomain.CurrentDomain.BaseDirectory;
                return Path.GetFullPath(Path.Combine(pathExecutingAssembly, testData));
            }
        }

        public const string TestWorkspace1 = "workspace1.json";
    }
}