using System.IO;
using System.Windows.Controls;
using DirectorySync.Model;

namespace DirectorySync.Service
{
    public class FileService
    {
        public static void CopyDirectory(string source, string target, TextBox textBox)
        {
            if (!source.EndsWith("\\")) source += "\\";
            if (!target.EndsWith("\\")) target += "\\";

            textBox.Clear();

            var stack = new Stack<Directories>();
            stack.Push(new Directories(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                try
                {
                    Directory.CreateDirectory(folders.Target);
                    foreach (var sourceFile in Directory.GetFiles(folders.Source, "*.*"))
                    {
                        var sourceFileName = Path.GetFileName(sourceFile);

                        var targetFilePath = Path.Combine(folders.Target, sourceFileName);
                        CopyOrUpdate(sourceFile, targetFilePath, textBox);
                    }

                    foreach (var folder in Directory.GetDirectories(folders.Source))
                    {
                        stack.Push(new Directories(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                    }
                }
                catch (Exception ex)
                {
                    textBox.AppendText(ex.Message + "\n");
                }
            }
        }

        private static void CopyOrUpdate(string sourceFile, string targetFile, TextBox textBox)
        {
            if (!File.Exists(targetFile))
            {
                File.Copy(sourceFile, targetFile);
                textBox.AppendText(sourceFile + " -> " + targetFile + "\n");
            }
            else
            {
                var targetInfo = new FileInfo(targetFile);
                var sourceInfo = new FileInfo(sourceFile);

                if (targetInfo.Length != sourceInfo.Length) // simple comparison for unsuccesfull copy
                {
                    File.Copy(sourceFile, targetFile, true);
                    textBox.AppendText("replace " + sourceFile + " -> " + targetFile + "\n");
                }
                else
                {
                    textBox.AppendText("skipped " + sourceFile + "\n");
                }
            }
            textBox.ScrollToEnd();
        }
    }
}
