﻿using System.ComponentModel;
using System.Diagnostics;

namespace WinGetUpdCore
{
    public sealed class PrerequisitesHelper : IPrerequisitesHelper
    {
        public bool PackageFileExists()
        {
            return File.Exists($"{AppData.PkgFile}");
        }

        public async Task<bool> WinGetExistsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "winget.exe",
                    Arguments = "--version",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                });

                if (process != null)
                {
                    await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

                    return true;
                }
            }
            catch (Win32Exception)
            {
                // Suppress exception
            }

            return false;
        }

        public async Task<bool> CanWriteLogFileAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await File.WriteAllTextAsync(AppData.LogFile, string.Empty, cancellationToken).ConfigureAwait(false);

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
