using CompanionCLI.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CompanionCLI.Services;

internal class JsonService
{
    public const string JsonFile = "data.json";

    public bool Exists()
    {
        return File.Exists(JsonFile);
    }

    public bool EmptyFile()
    {
        return new FileInfo(JsonFile).Length == 0;
    }

    public static GlobalMerger Load()
    {
        return JsonSerializer.Deserialize<GlobalMerger>
            (
                File.ReadAllText(JsonFile)
            ) ?? new();
    }

    public void Save(GlobalMerger data)
    {
        File.WriteAllText(JsonFile,
            JsonSerializer.Serialize(data,
            new JsonSerializerOptions
            {
                WriteIndented = true,
            })
        );
    }

    public void Purge()
    {
        Save(new GlobalMerger());
    }

    public static bool IsCorrupted()
    {
        try
        {
            Load();
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return true;
        }
    }
}
