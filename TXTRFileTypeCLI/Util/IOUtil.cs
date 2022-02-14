/*
TXTRFileType
Copyright (C) 2021 xchellx

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.IO;

namespace TXTRFileTypeCLI.Util
{
    public static class IOUtil
    {
        public static bool FileExists(string filePath, out string failReason)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                if (!IsDirectory(filePath))
                {
                    if (File.Exists(filePath))
                    {
                        failReason = string.Empty;
                        return true;
                    }
                    else
                    {
                        failReason = $"File not found: \"{filePath}\"";
                        return false;
                    }
                }
                else
                {
                    failReason = $"Must be a file and not a directory: \"{filePath}\"";
                    return false;
                }
            }
            else
            {
                failReason = filePath == null ? "File path is null" : "File path is empty.";
                return false;
            }
        }

        public static bool DirectoryExists(string directoryPath, out string failReason)
        {
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                if (IsDirectory(directoryPath))
                {
                    failReason = string.Empty;
                    return true;
                }
                else
                {
                    if (Directory.Exists(directoryPath))
                    {
                        failReason = $"Must be a directory and not a file: \"{directoryPath}\"";
                        return false;
                    }
                    else
                    {
                        failReason = $"Directory not found: \"{directoryPath}\"";
                        return false;
                    }
                }
            }
            else
            {
                failReason = directoryPath == null ? "Directory path is null" : "Directory path is empty.";
                return false;
            }
        }

        public static bool IsDirectory(string path)
            => string.IsNullOrEmpty(Path.GetFileName(path)) || Directory.Exists(path);
    }
}
