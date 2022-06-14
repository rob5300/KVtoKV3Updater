using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValveKeyValue;

namespace KVSurfaceUpdater
{
    internal abstract class Converter
    {
        public Converter() { }

        public virtual void Convert(string targetPath)
        {
            string outputFolder = Path.Combine(targetPath, "data");
            foreach (var file in Directory.EnumerateFiles(targetPath, "*", SearchOption.AllDirectories))
            {
                string filename = Path.GetFileName(file);
                if (ShouldConvertFile(filename))
                {
                    try
                    {
                        ProcessFile(file, outputFolder);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Error processing file '{file}': {e.Message}");
                    }
                }
            }
        }
        
        protected virtual bool ShouldConvertFile(string filename)
        {
            return true;
        }

        protected abstract bool ProcessFile(string filePath, string outputFolder);

        protected KVObject OpenKVFile(string filepath)
        {
            string fileText = File.ReadAllText(filepath);

            //Remove conditionals as this parser just breaks on them?
            fileText = Regex.Replace(fileText, @"\[\$.*\]", "");

            //Write back to stream for parser
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(fileText);
                writer.Flush();
                stream.Position = 0;
                KVSerializer serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
                return serializer.Deserialize(stream);
            }
        }

        protected List<KVObject> ListAllObjects(KVObject kvObject)
        {
            List<KVObject> listedObjects = new List<KVObject>();
            listedObjects.Add(kvObject);
            listedObjects.AddRange(kvObject.Children.Where(x => x.Children.Count() > 0));
            return listedObjects;
        }

        public static void ExitFromError(Exception e, string attemptedPath = null)
        {
            if (!string.IsNullOrEmpty(attemptedPath))
            {
                Console.WriteLine($"! Unable to open '{Path.GetFileName(attemptedPath)}'. Program will now exit.");
            }
            else
            {
                Console.WriteLine("!!! Critical error occurred. Program will now exit.");
            }

            if (!string.IsNullOrEmpty(attemptedPath))
            {
                Console.WriteLine($"Attempted path: '{attemptedPath}'");
            }
            Console.WriteLine($"!!! ERROR: {e.GetType().Name}, {e.Message}.");
            Console.WriteLine("Error Stack:\n" + e.StackTrace);
            Environment.Exit(1);
        }
    }
}
