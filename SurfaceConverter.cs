using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace KVSurfaceUpdater
{
    internal class SurfaceConverter : Converter
    {
        KVObject decals_subrect;
        Dictionary<string, string> translationData = new Dictionary<string, string>();
        List<KVObject> decals = new List<KVObject>();
        Dictionary<string, KV3Decal> newDecals = new Dictionary<string, KV3Decal>();

        public override async Task ConvertAsync(string targetPath)
        {
            LoadDecalsSubrect(targetPath);
            
            string outputFolder = Path.Combine(targetPath, "surface");
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            Console.WriteLine("-= Will process decals...");

            string decalOutPath = Path.Combine(outputFolder, "decal");
            if (!Directory.Exists(decalOutPath))
            {
                Directory.CreateDirectory(decalOutPath);
            }

            var createdDecals = CreateDecalFiles(decals, decalOutPath);
            newDecals = createdDecals.ToDictionary(x => x.Name);

            await base.ConvertAsync(targetPath);
        }

        protected override bool ShouldConvertFile(string filename)
        {
            return filename.Contains("surfaceproperties") && !filename.Contains("manifest");
        }

        private void LoadDecalsSubrect(string targetFolder)
        {
            string decalsSubrectPath = Path.Combine(targetFolder, "decals_subrect.txt");
            try
            {
                Console.WriteLine($"-= Will load '{decalsSubrectPath}' for translation data and decals...");
                decals_subrect = OpenKVFile(decalsSubrectPath);
                //Sort out the translation data from the other decals
                foreach (var child in decals_subrect)
                {
                    if (child.Children.Count() > 0)
                    {
                        decals.Add(child);
                    }
                    else
                    {
                        if (!translationData.ContainsKey(child.Name))
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
            catch (FileNotFoundException e)
            {
                ExitFromError(e, decalsSubrectPath);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("!!! Program did not have permission to access this file. Move it elsewhere or run this as admin !!!");
                ExitFromError(e, decalsSubrectPath);
            }
            catch (Exception e)
            {
                ExitFromError(e);
            }
        }

        protected override async Task<bool> ProcessFile(string filePath, string outputFolder)
        {
            try
            {
                KVObject? kvObject = OpenKVFile(filePath);
                //Make a list of all the surfaces
                List<KVObject> surfaceObjects = ListAllObjects(kvObject);

                foreach (var surfaceObject in surfaceObjects)
                {
                    string surfaceName = KV3Surface.Prefix + surfaceObject.Name;
                    string surfaceFile = Path.Combine(outputFolder, "surface", $"{surfaceName.ToLower()}.surface");
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
                    return true;
                }
            }
            catch (Exception e)
            {
                ExitFromError(e, filePath);
            }
            return false;
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
            catch (Exception e)
            {
                ExitFromError(e);
                return null;
            }
        }
    }
}
