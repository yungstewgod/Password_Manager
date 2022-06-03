using System.IO;
using System.Reflection;

namespace Saugumo_4_praktinis.Files
{
    class FilePath
    {
        private static string directory;

        public static string Directory { get => directory; set => directory = value; }

        static FilePath()
        {
            Directory = @"C:\Testas\test\";
        }

        public static string GetPathTxt(string name)
        {
            return Directory + name + ".txt";
        }

        public static string GetPathAes(string name)
        {
            return Directory + name + ".txt.aes";
        }
    }
}