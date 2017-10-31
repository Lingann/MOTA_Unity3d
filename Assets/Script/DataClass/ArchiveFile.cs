using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ArchiveFile {

    // 游戏初始化资产
    private static TextAsset Default = Resources.Load("savedata1") as TextAsset;

    // 游戏初始化文档
    private static string Data { get { return Default.text; } }

    // 存档文件名
    private static string SAVE_DATA_FILE_NAME_BASE = "savedata";

    // 存档文件扩展名
    private static string SAVE_DATA_FILE_EXTENSION = ".json";

    // 存档目录
    private static string SAVE_DATA_DIRECTORY { get { return Application.dataPath + "/Save/"; } }

    // 存档文件名
    private static string ArchiveText;

    // 存档目录引用
    public static DirectoryInfo SaveDirectory = new DirectoryInfo(SAVE_DATA_DIRECTORY);

    // 新建一个列表，该列表保存着所有存档文件的名
    public static List<string> ArchivesName = new List<string>();

    // 新建一个数组，该数组保存存档文件夹下的所有存档文件的FileInfo引用
    public static FileInfo[] Files;

    /// <summary>
    /// 完整存档文件路径
    /// </summary>
    /// <param name="profileNumber">序列化文件后缀</param>
    /// <returns></returns>
    public static string GetSaveFilePath(int profileNumber) {
        // 如果存档文件少于1 则抛出错误
        if (profileNumber < 1)
            throw new System.ArgumentException("profileNumber must be greater than 1. Was: " + profileNumber);

        // 确认存档目录，如果不存在文档文件夹目录，则创建一个存档文件夹目录，该操作可以防止玩家误删存档目录文件夹
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            // 新建一个文件夹作为存档目录文件夹
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        // 返回完整存档路径
        return SAVE_DATA_DIRECTORY + SAVE_DATA_FILE_NAME_BASE + profileNumber.ToString() + SAVE_DATA_FILE_EXTENSION;
    }

    /// <summary>
    /// 通过存档文件名(不包含扩展名)，获取完整存档文件路径
    /// 可用于创建自定义文件名的存档文件
    /// </summary>
    /// <param name="profileBase"></param>
    /// <returns></returns>
    private static string GetSaveFilePath(string archivePath) {

        // 确认存档目录，如果不存在文档文件夹目录，则创建一个存档文件夹目录，该操作可以防止玩家误删存档目录文件夹
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            // 新建一个文件夹作为存档目录文件夹
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        // 返回完整存档路径
        return SAVE_DATA_DIRECTORY + archivePath + SAVE_DATA_FILE_EXTENSION;
    }

    /// <summary>
    /// 通过指定一个存档索引，获取存档名
    /// </summary>
    /// <param name="profileNumber"></param>
    /// <returns></returns>
    public static string GetSaveFileName(int profileNumber) {
        return SAVE_DATA_FILE_NAME_BASE + profileNumber.ToString() + SAVE_DATA_FILE_EXTENSION;
    }

    /// <summary>
    /// 添加存档
    /// </summary>
    public static void AddArchive(string archiveText) {

        // 存档文件数量增加1
        int profileNumber = 1;

        // 获取存档文件夹下的所有存档
        Files = GetArchives();

        // 如果存档列表已经包含了该文件，则文件索引+1
        while (ArchivesName.Contains(GetSaveFileName(profileNumber))) {
            profileNumber += 1;
        }

        // 新建存档文件
        StreamWriter sw = File.CreateText(GetSaveFilePath(profileNumber));

        // 关闭存档文件
        sw.Close();

        // 写入内容到存档中
        File.WriteAllText(GetSaveFilePath(profileNumber), archiveText);

    }

    public static void AddArchive(int profileNumber, string archiveText) {
        // 新建存档文件
        StreamWriter sw = File.CreateText(GetSaveFilePath(profileNumber));

        // 关闭存档文件
        sw.Close();

        // 写入内容到存档中
        File.WriteAllText(GetSaveFilePath(profileNumber), archiveText);
    }

    /// <summary>
    /// 覆盖存档
    /// </summary>
    /// <param name="path">覆盖的文件完整路径</param>
    /// <param name="contents">写入存档的字符串</param>
    public static void OveriderArchive(string path, string contents) {
        File.WriteAllText(path, contents);
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="path">存档完整路径</param>
    public static void DeleteArchive(string archivePath) {
        File.Delete(archivePath);
    }

    /// <summary>
    /// 获取所有存档文件
    /// </summary>
    public static FileInfo[] GetArchives() {

        // 确认存档目录，如果不存在文档文件夹目录，则创建一个存档文件夹目录，该操作可以防止玩家误删存档目录文件夹
        if (!SaveDirectory.Exists)
            // 新建一个文件夹作为存档目录文件夹
            SaveDirectory.Create();

        // 找到存档目录下的所有存档文件
        FileInfo[] files = SaveDirectory.GetFiles("*" + SAVE_DATA_FILE_EXTENSION);

        foreach (FileInfo item in files) {
            ArchivesName.Add(item.Name);
        }

        return files;

    }

    /// <summary>
    /// 获取存档文件的文本内容
    /// </summary>
    /// <param name="fileName">包含后缀名的存档文件名</param>
    /// <returns></returns>
    public static string GetArchiveText(string fileName) {

        Files = GetArchives();

        // 如果没有找到存档，则调用初始化配置文件，将游戏进行初始化
        if (!ArchivesName.Contains(fileName))
            throw new System.ArgumentException("没有找到存档文件");


        // 遍历存档文件夹下的存档文件，找到指定的存档文件并返回 
        for (int i = 0; i < Files.Length;i++) {

            if (Files[i].Name == fileName)
                // 获取存档文本内容
                ArchiveText = File.ReadAllText( Files[i].FullName);
        }

        // 返回存档文本内容
        return ArchiveText;

    }

    public static string GetArchiveTextByPath(string filePath) {

        if (File.Exists(filePath))
            throw new System.ArgumentException("没有找到存档文件");
        else
            return ArchiveText = File.ReadAllText(filePath);

    }

    public static bool FileIsCanFound(string filePath) {
        if (File.Exists(filePath))
            return true;

        return false;
    }




}
