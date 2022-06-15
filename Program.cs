using KVSurfaceUpdater;
using System.Diagnostics;
using System.Reflection;
using ValveKeyValue;

Console.WriteLine("KV Updater by rob5300");

Converter? converter = null;
var type = typeof(Converter);
var converterTypes = type.Assembly.GetTypes().Where(x => !x.IsAbstract && x.IsSubclassOf(type)).ToList();

string allConverters = string.Join(", ", converterTypes.Select(x => $"{converterTypes.IndexOf(x)}: {x.Name.Replace("Converter", "")}"));

while(converter == null)
{
    Console.WriteLine("Enter name/num of converter to use:");
    Console.WriteLine(allConverters);
    string input = Console.ReadLine();
    try
    {
        int index = int.Parse(input);
        converter = (Converter?)Activator.CreateInstance(converterTypes[index]);
    }
    catch (Exception)
    {
        foreach(var converterType in converterTypes)
        {
            if(converterType.Name.ToLower().Contains(input.ToLower()))
            {
                converter = (Converter?)Activator.CreateInstance(converterType);
            }
        }
    }
}

string targetFolder = null;
while(string.IsNullOrEmpty(targetFolder))
{
    Console.WriteLine("-= Input target folder:");
    targetFolder = Console.ReadLine();
    string relativePath = Path.Combine(Directory.GetCurrentDirectory(), targetFolder);
    if (Directory.Exists(relativePath))
    {
        targetFolder = relativePath;
    }
    else if(!Directory.Exists(targetFolder))
    {
        Console.WriteLine("Folder does not exist!");
        targetFolder = null;
    }
}

Console.WriteLine("Input prefix for converted files (leave empty for no prefix):");
KV3Object.Prefix = Console.ReadLine();
if(KV3Object.Prefix != "")
{
    Console.WriteLine($"Will use prefix '{KV3Object.Prefix}'.");
}

if (Directory.Exists(targetFolder))
{
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    await converter.ConvertAsync(targetFolder);

    stopwatch.Stop();
    Console.WriteLine($"Done! Took {stopwatch.Elapsed}.");
}
else
{
    Console.WriteLine("-= Directory doesnt exist, exiting...");
}
Console.ReadKey();

