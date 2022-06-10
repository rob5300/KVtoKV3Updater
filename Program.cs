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

Console.WriteLine("Input prefix for converted surfaces (leave empty for no prefix):");
KV3Surface.Prefix = Console.ReadLine();
if(KV3Surface.Prefix != "")
{
    Console.WriteLine($"Will use prefix '{KV3Surface.Prefix}'.");
}

var serialiser = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
KVObject decals_subrect;
Dictionary<string, string> translationData= new Dictionary<string, string>();
List<KVObject> decals = new List<KVObject>();
Dictionary<string, KV3Decal> newDecals = new Dictionary<string, KV3Decal>();

string decalsSubrectPath = Path.Combine(targetFolder, "decals_subrect.txt");
try
{
    Console.WriteLine($"-= Will load '{decalsSubrectPath}' for translation data and decals...");
    using (var stream = File.Open(decalsSubrectPath, FileMode.Open))
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
                if(!translationData.ContainsKey(child.Name))
                {
                    translationData.Add(child.Name, (string)child.Value);
                }
                else
                {
                    Console.WriteLine($"! Duplicate translation key '{child.Name}' with value '{(string)child.Value}' will be ignored ({child.Name} will instead translate to '{translationData[child.Name]}').");
                }
            }
        }
    }
}
catch (FileNotFoundException e)
{
    ExitFromError(e, decalsSubrectPath);
}
catch(UnauthorizedAccessException e)
{
    Console.WriteLine("!!! Program did not have permission to access this file. Move it elsewhere or run this as admin !!!");
    ExitFromError(e, decalsSubrectPath);
}
catch (Exception e)
{
    ExitFromError(e);
}

string outputFolder;

if (Directory.Exists(targetFolder))
{
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    outputFolder = Path.Combine(targetFolder, "data", "surface");
    if (!Directory.Exists(outputFolder))
    {
        Directory.CreateDirectory(outputFolder);
    }

    Console.WriteLine("-= Will process decals...");

    string decalOutPath = Path.Combine(targetFolder, "data", "decal");
    if (!Directory.Exists(decalOutPath))
    {
        Directory.CreateDirectory(decalOutPath);
    }

    var createdDecals = CreateDecalFiles(decals, decalOutPath);
    newDecals = createdDecals.ToDictionary(x => x.Name);

    Console.WriteLine("-= Will process surfaces files...");

    //Look for surfaceproperties files and process them
    string surfacePropertiesFilePath = null;
    foreach (var potentialSurfaceProperties in Directory.EnumerateFiles(targetFolder))
    {
        string name = Path.GetFileNameWithoutExtension(potentialSurfaceProperties).ToLower();
        if (name.Contains("surfaceproperties") && !name.Contains("manifest"))
        {
            Console.WriteLine($"Found '{potentialSurfaceProperties}' to extract surfaces from.");
            ProcessSurfacesFile(potentialSurfaceProperties, outputFolder);
            surfacePropertiesFilePath = potentialSurfaceProperties;
        }
    }

    stopwatch.Stop();
    Console.WriteLine($"Done! Took {stopwatch.Elapsed}.");
}
else
{
    Console.WriteLine("-= Directory doesnt exist, exiting...");
}
Console.ReadKey();

void ProcessSurfacesFile(string filePath, string outputFolder)
{
    try
    {
        using (var stream = File.Open(filePath, FileMode.Open))
        {
            KVObject? kvObject = serialiser.Deserialize(stream);
            //Make a list of all the surfaces
            List<KVObject> surfaceObjects = new List<KVObject>();
            surfaceObjects.Add(kvObject);
            surfaceObjects.AddRange(kvObject.Children.Where(x => x.Children.Count() > 0));

            foreach (var surfaceObject in surfaceObjects)
            {
                string surfaceName = KV3Surface.Prefix + surfaceObject.Name;
                string surfaceFile = Path.Combine(outputFolder, $"{surfaceName.ToLower()}.surface");
                KV3Surface newSurface = new KV3Surface(surfaceObject);
                newSurface.Name = KV3Surface.Prefix + newSurface.Name;
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
    catch(Exception e)
    {
        ExitFromError(e, filePath);
    }
}

List<KV3Decal> CreateDecalFiles(IEnumerable<KVObject> decals, string outputFolder)
{
    try
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
    catch(Exception e)
    {
        ExitFromError(e);
        return null;
    }
}

void ExitFromError(Exception e, string attemptedPath = null)
{
    if(!string.IsNullOrEmpty(attemptedPath))
    {
        Console.WriteLine($"! Unable to open '{Path.GetFileName(attemptedPath)}'. Program will now exit.");
    }
    else
    {
        Console.WriteLine("!!! Critical error occurred. Program will now exit.");
    }
    
    if(!string.IsNullOrEmpty(attemptedPath))
    {
        Console.WriteLine($"Attempted path: '{decalsSubrectPath}'");
    }
    Console.WriteLine($"!!! ERROR: {e.GetType().Name}, {e.Message}.");
    Console.WriteLine("Error Stack:\n" + e.StackTrace);
    Environment.Exit(1);
}