using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValveKeyValue;

namespace KVSurfaceUpdater
{
    internal class SoundConverter : Converter
    {
        public bool FolderPerSoundFile = true;

        public override async Task ConvertAsync(string targetPath)
        {
            string outputFolder = Path.Combine(targetPath, "data", "sound");
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            await base.ConvertAsync(targetPath);
        }

        protected override bool ShouldConvertFile(string filename)
        {
            return Regex.Match(filename, @"game_sounds.*\.txt").Success;
        }

        protected override async Task<bool> ProcessFile(string filePath, string outputFolder)
        {
            var soundKV = OpenKVFile(filePath);
            List<KVObject> allSounds = ListAllObjects(soundKV);
            foreach(KVObject sound in allSounds)
            {
                await Task.Run(() =>
                {
                    var kv3Sound = new KV3Sound(sound);
                    string newName = $"{kv3Sound.Name.ToLower().Replace("/", "_")}.sound";
                    string newSoundFolder = FolderPerSoundFile ? Path.Combine(outputFolder, "sound", Path.GetFileNameWithoutExtension(filePath)) : Path.Combine(outputFolder, "sound");
                    string newSoundFilePath = Path.Combine(newSoundFolder, newName);

                    if (!Directory.Exists(newSoundFolder))
                    {
                        Directory.CreateDirectory(newSoundFolder);
                    }
                    File.WriteAllText(newSoundFilePath, kv3Sound.ToString());
                    Console.WriteLine($"Made sound file for '{kv3Sound.Name}'.");
                });
            }
            return true;
        }
    }
}
