using System.Text;

namespace CourseAsCode.Readers.Helper
{
    public static class PathUtils
    {
        public static bool IsValidFilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return false;
            }

            try
            {
                var fullPath = Path.GetFullPath(path);

                if (fullPath.EndsWith(Path.DirectorySeparatorChar) || fullPath.EndsWith(Path.AltDirectorySeparatorChar))
                {
                    return false;
                }

                var fileName = Path.GetFileName(fullPath);
                return !string.IsNullOrWhiteSpace(fileName);
            }
            catch
            {

                return false;
            }
        }

        public static string InlineIfFile(string? value, string baseDir)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value ?? string.Empty;
            }

            if (!IsValidFilePath(value))
            {
                return value;
            }

            try
            {
                var resolved = ResolveToFullPath(value, baseDir);
                if (!File.Exists(resolved))
                {
                    return value; // looks like a path, but no file — leave as-is
                }

                // Read as UTF-8 (no BOM). Adjust encoding if your files differ.
                var text = File.ReadAllText(resolved, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

                // Optional: normalize line endings if needed
                // text = text.Replace("\r\n", "\n");

                return text;
            }
            catch
            {
                // On any IO/permission/path error, keep original value
                return value;
            }
        }

        private static string ResolveToFullPath(string path, string baseDir)
        {
            return Path.GetFullPath(Path.IsPathRooted(path) ? path : Path.Combine(baseDir, path));
        }
    }
}
