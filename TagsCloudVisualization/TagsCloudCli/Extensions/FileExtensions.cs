using System;
using System.IO;
using ResultOf;

namespace TagsCloudCli.Extensions
{
    public static class FileExtensions
    {
        public static Result<string> HaveReadAccess(string filename)
        {
            return Result.FromFunction(() =>
            {
                using (File.OpenRead(filename))
                {
                }
                return filename;
            });
        }

        public static Result<string> HaveWriteAccess(string filename)
        {
            return Result.FromFunction(() =>
            {
                using (File.OpenWrite(filename))
                {
                }
                return filename;
            });
        }
    }
}