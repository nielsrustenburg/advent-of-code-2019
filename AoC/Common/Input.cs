using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Text;

//Such credit, much thanks to github.com/genveir, from who I stole most of this

namespace AoC.Common
{
    public static class Input
    {
        public enum InputMode
        {
            String,
            Embedded,
            File
        }

        public static string GetInput(InputMode mode, string input)
        {
            Stream inputStream;
            switch (mode)
            {
                case InputMode.String: return input;
                case InputMode.Embedded: inputStream = GetEmbeddedResourceStream(input); break;
                case InputMode.File: inputStream = GetFileStream(input); break;
                default: throw new ArgumentException("use a valid InputMode");
            }

            using (var streamReader = new StreamReader(inputStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static Stream GetFileStream(string input)
        {
            string path = null;
            if (File.Exists(input))
            {
                path = input;
            }
            else
            {
                var callingClass = GetCallingType();
                var folderInfo = new DirectoryInfo(callingClass.Assembly.CodeBase.Replace("file:///", "")).Parent;

                var folder = folderInfo.ToString();
                var dir = folderInfo.Parent.Parent.Parent.ToString();
                var nameSpace = callingClass.Namespace.Split('.');
                var inputDotTxt = $"{input}.txt";

                var options = new string[]
                {
                    Path.Join(dir, "inputs", input),
                    Path.Join(dir, nameSpace.Last(), input),
                    Path.Join(dir, nameSpace.Last(), inputDotTxt),
                    Path.Join(dir, input),
                    Path.Join(dir, inputDotTxt),
                    Path.Join(folder, input),
                    Path.Join(folder, inputDotTxt)
                };

                path = options.First(option => File.Exists(option));
            }

            if (path == null) throw new IOException($"file {input} does not exist");

            return new FileStream(path, FileMode.Open);
        }

        private static Stream GetEmbeddedResourceStream(string input)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            string resourceName = null;
            if (resourceNames.Contains(input))
            {
                resourceName = input;
            }
            else
            {
                var callingClass = GetCallingType();
                var inputDotTxt = $"{input}.txt";
                var namespaceDotInput = $"{callingClass.Namespace}.{input}";
                var namespaceDotInputDotTxt = $"{namespaceDotInput}.txt";
                var options = new string[] { inputDotTxt, namespaceDotInput, namespaceDotInputDotTxt };
                resourceName = options.First(name => resourceNames.Contains(name));
            }

            if (resourceName == null) throw new IOException($"embedded resource {input} does not exist");

            return assembly.GetManifestResourceStream(resourceName);
        }

        private static Type GetCallingType()
        {
            var stackTrace = new StackTrace();
            for(int frameCounter = 1; frameCounter < stackTrace.FrameCount; frameCounter++)
            {
                var frame = stackTrace.GetFrame(frameCounter);
                var type = frame.GetMethod().DeclaringType;

                if (type != typeof(Input) && !type.GetTypeInfo().IsAbstract) return type;
            }
            return null;
        }
    }
}
