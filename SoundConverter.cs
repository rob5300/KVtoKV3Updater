using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVSurfaceUpdater
{
    internal class SoundConverter : Converter
    {
        public override void Convert(string targetPath)
        {
            string outputFolder = Path.Combine(targetPath, "data", "sound");
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            base.Convert(targetPath);
        }

        protected override bool ShouldConvertFile(string filename)
        {
            return filename.EndsWith(".txt");
        }

        protected override bool ProcessFile(string filePath, string outputFolder)
        {
            var soundKV = OpenKVFile(filePath);
            var kv3Sound = new KV3Sound(soundKV);
            string newSoundPath = Path.Combine(outputFolder, "sound", $"{kv3Sound.Name.ToLower()}.sound");
            File.WriteAllText(newSoundPath, kv3Sound.ToString());
            Console.WriteLine($"Made sound file for '{kv3Sound.Name}'.");
            return true;
        }
    }
}
