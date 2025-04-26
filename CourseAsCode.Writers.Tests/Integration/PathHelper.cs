using System;
using System.IO;

public static class PathHelper
{
    /// <summary>
    /// Berechnet den absoluten Pfad zu einer Datei basierend auf einem relativen Pfad.
    /// </summary>
    /// <param name="relativePath">Der relative Pfad zur Datei.</param>
    /// <returns>Der absolute Pfad zur Datei.</returns>
    public static string GetAbsolutePath(string relativePath)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        return Path.GetFullPath(Path.Combine(basePath, relativePath));
    }

    /// <summary>
    /// Stellt sicher, dass ein Verzeichnis existiert, und gibt den absoluten Pfad zurück.
    /// </summary>
    /// <param name="relativePath">Der relative Pfad zum Verzeichnis.</param>
    /// <returns>Der absolute Pfad zum Verzeichnis.</returns>
    public static string EnsureDirectoryExists(string relativePath)
    {
        string absolutePath = GetAbsolutePath(relativePath);
        Directory.CreateDirectory(absolutePath);
        return absolutePath;
    }
}
