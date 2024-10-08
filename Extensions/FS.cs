using CodeMechanic.Diagnostics;
using CodeMechanic.Types;

namespace CodeMechanic.FileSystem;

public record FileSave(string filename, string text, string root, params string[] subfolders);

public static class FS
{
    public static void OpenWith(this string filepath, string program)
    {
        if (!Path.HasExtension(filepath)) throw new ArgumentException("File type not supported");
        if (System.IO.File.Exists(filepath))
        {
            System.Diagnostics.Process.Start(program, filepath);
        }
    }

    public static FileInfo SaveAs(SaveAs saveAs, params string[] lines)
    {
        if (saveAs == null) throw new ArgumentNullException(nameof(saveAs));
        if (saveAs.file_name == null) throw new ArgumentNullException(nameof(saveAs.file_name));

        // at worst, use cwd:
        if (saveAs.save_folder.IsEmpty() && saveAs.root_path.IsEmpty())
            saveAs.root_path = Environment.CurrentDirectory.Dump("had to use current working directory");

        string save_folder_path = Path.Combine(saveAs.root_path, saveAs.save_folder);

        Console.WriteLine("filename :>> " + saveAs.file_name);
        if (saveAs.create_nonexistent_directory)
            MakeDir(save_folder_path);

        string save_path = Path.Combine(save_folder_path, saveAs.file_name);
        Console.WriteLine("saving to path :>> " + save_path);

        // handle both single and double lines in one method without being retarded <3
        // if (lines.Length > 1)
        File.WriteAllLines(save_path, lines);
        // if (lines.Length == 1)
        //     File.WriteAllText(save_path, lines[0]);

        return new FileInfo(save_path);
    }

    public static DirectoryInfo MakeDir(string folder_path)
    {
        if (!Directory.Exists(folder_path))
            Directory.CreateDirectory(folder_path);
        return new DirectoryInfo(folder_path);
    }
}

public record SaveAs
{
    public SaveAs(string file_name)
    {
        this.file_name = file_name;
    }

    public bool create_nonexistent_directory { get; set; } = true;
    public bool debug { get; set; } = false;
    public string root_path { get; set; } = string.Empty;
    public string save_folder { get; set; } = string.Empty;
    public string file_name { get; set; } = string.Empty;
}

public static class FS_V2
{
    public static void SaveAs(this FileSave file_save, bool debug = false)
    {
        if (file_save == null) throw new NullReferenceException(nameof(file_save));
        if (file_save.root.IsEmpty()) throw new NullReferenceException(nameof(file_save.root));
        if (file_save.filename.IsEmpty()) throw new NullReferenceException(nameof(file_save.filename));

        string save_folder = file_save.root;
        foreach (var subfolder in file_save.subfolders ?? Array.Empty<string>())
        {
            save_folder = Path.Combine(save_folder, subfolder);
        }

        if (debug)
            Console.WriteLine("save folder: \n" + save_folder);

        string save_path = Path.Combine(save_folder, file_save.filename);

        if (debug)
            Console.WriteLine("save path: \n" + save_path);

        if (!Directory.Exists(save_folder)) Directory.CreateDirectory(save_folder);

        File.WriteAllText(save_path, file_save.text);
    }
}