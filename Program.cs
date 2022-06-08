using KVSurfaceUpdater;
using System.Diagnostics;
using ValveKeyValue;

Console.WriteLine("KV Surfaces Updater by rob5300");

string targetFolder = null;
while(string.IsNullOrEmpty(targetFolder))
{
    Console.WriteLine("-= Input foler for surfaces file and decals_subrect:");
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

var serialiser = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
KVObject decals_subrect;
Dictionary<string, string> translationData= new Dictionary<string, string>();
List<KVObject> decals = new List<KVObject>();
Dictionary<string, KV3Decal> newDecals = new Dictionary<string, KV3Decal>();

try
{
    using (var stream = File.Open(Path.Combine(targetFolder, "decals_subrect.txt"), FileMode.Open))
    {
        decals_subrect = serialiser.Deserialize(stream);
        //Sort out the translation data from the other decals
        foreach(var child in decals_subrect)
        {
            if(child.Children.Count() > 0)
            {
                decals.Add(child);
            }
            else
            {
                translationData.Add(child.Name, (string)child.Value);
            }
        }
    }
}
catch (Exception)
{
    Console.WriteLine("ERROR: Unable to find 'decals_subrect.txt'.");
    return;
}
    
string outputFolder;

if (Directory.Exists(targetFolder))
{
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    outputFolder = Path.Combine(Path.GetDirectoryName(targetFolder), "data", "surface");
    if (!Directory.Exists(outputFolder))
    {
        Directory.CreateDirectory(outputFolder);
    }

    Log.WriteLine($"-= Will process decals...");

    string decalOutPath = Path.Combine(targetFolder, "data", "decal");
    if (!Directory.Exists(decalOutPath))
    {
        Directory.CreateDirectory(decalOutPath);
    }
    
    var createdDecals = CreateDecalFiles(decals, decalOutPath);
    newDecals = createdDecals.ToDictionary(x => x.Name);

    Log.WriteLine($"-= Will process surfaces files...");

    //Look for surfaceproperties files and process them
    string surfacePropertiesFilePath = null;
    foreach(var potentialSurfaceProperties in Directory.EnumerateFiles(targetFolder))
    {
        string name = Path.GetFileNameWithoutExtension(potentialSurfaceProperties).ToLower();
        if (name.Contains("surfaceproperties") && !name.Contains("manifest"))
        {
            Console.WriteLine($"Found '{potentialSurfaceProperties}' to extract surfaces from.");
            ProcessSurfacesFile(potentialSurfaceProperties, outputFolder);
            surfacePropertiesFilePath = potentialSurfaceProperties;
        }
    }
    
    //Write log to program dir
    //Log.WriteToFile(Directory.GetCurrentDirectory());

    stopwatch.Stop();
    Console.WriteLine($"Done! Took {stopwatch.Elapsed}.");
}
else
{
    Log.WriteLine("-= Directory doesnt exist, exiting...");
}
Console.ReadKey();

void ProcessSurfacesFile(string filePath, string outputFolder)
{
    using (var stream = File.Open(filePath, FileMode.Open))
    {
        KVObject? kvObject = serialiser.Deserialize(stream);
        //Make a list of all the surfaces
        List<KVObject> surfaceObjects = new List<KVObject>();
        surfaceObjects.Add(kvObject);
        surfaceObjects.AddRange(kvObject.Children.Where(x => x.Children.Count() > 0));
        
        foreach(var surfaceObject in surfaceObjects)
        {
            string surfaceName = surfaceObject.Name;
            string surfaceFile = Path.Combine(outputFolder, $"{surfaceName.ToLower()}.surface");
            KV3Surface newSurface = new KV3Surface(surfaceObject);
            KVValue? gamematerial = newSurface.gamematerial;
            if (gamematerial != null)
            {
                if (translationData.TryGetValue((string)gamematerial, out string decalName))
                {
                    newDecals.TryGetValue(decalName, out KV3Decal? decal);
                    newSurface.gamematerial_decal = decal;
                }
                else
                {
                    Console.WriteLine($"WARNING: Unable to find decal '{gamematerial}' translation for surface '{surfaceName}'.");
                }
            }
           
            File.WriteAllText(surfaceFile, newSurface.ToString());
            Console.WriteLine($"Made surface file for '{surfaceName}'.");
        }
    }
}

List<KV3Decal> CreateDecalFiles(IEnumerable<KVObject> decals, string outputFolder)
{
    List<KV3Decal> newDecals = new List<KV3Decal>();
    foreach (var decal in decals)
    {
        string decalName = decal.Name;
        string decalFile = Path.Combine(outputFolder, $"{decalName.ToLower()}.decal");
        KV3Decal newDecal = new KV3Decal(decal);
        File.WriteAllText(decalFile, newDecal.ToString());
        Console.WriteLine($"Made decal file for '{newDecal.Name}'.");
        newDecals.Add(newDecal);
    }
    return newDecals;
}