using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace KVSurfaceUpdater
{
    internal class KV3Sound : KV3Object
    {
        List<string> sounds = new List<string>();
        
        KVValue? channel;
        KVValue? soundlevel;

        float pitch = 1f;
        float volume = 1f;
        float randomVolume;
        float randomPitch;


        public KV3Sound(KVObject sourceObject) : base(sourceObject)
        {
        }

        protected override void SetupWithKVObject(KVObject sourceObject)
        {
            Name = sourceObject.Name;
            channel = sourceObject["channel"];
            soundlevel = sourceObject["soundlevel"];

            var kvvolume = sourceObject["volume"];
            var kvpitch = sourceObject["pitch"];

            if(kvvolume != null)
            {
                string stringVolume = (string)kvvolume;
                if(stringVolume.ToLower().Contains("norm"))
                {
                    volume = 1f;
                }
                else
                {
                    var volumeValues = GetFloatArray(stringVolume);
                    volume = volumeValues[0];
                    if (volumeValues.Length > 1)
                    {
                        randomVolume = volumeValues[1] - volume;
                    }
                }
            }

            if(kvpitch != null)
            {
                string stringPitch = (string)kvpitch;
                if(stringPitch.ToLower().Contains("norm"))
                {
                    pitch = 1f;
                }
                else
                {
                    var pitchValues = GetFloatArray(stringPitch);
                    pitch = pitchValues[0] / 100f;
                    if (pitchValues.Length > 1)
                    {
                        randomPitch = (pitchValues[1] / 100f) - pitch;
                    }
                }
            }

            var wave = sourceObject["wave"];
            var rndwave = sourceObject["rndwave"];
            if(wave != null)
            {
                sounds.Add(ToSoundResource(wave));
            }
            else if(rndwave != null)
            {
                //Cast to IEnumerable to be able to get children that I know should be there
                foreach (var childwave in (IEnumerable<KVObject>)rndwave)
                {
                    sounds.Add(ToSoundResource(childwave.Value));
                }
            }
            else
            {
                Console.WriteLine($"Warning: No wave found for '{Name}'.");
            }
        }

        private static string ToSoundResource(KVValue wave)
        {
            string wavePath = (string)wave;
            //Remove some chars, could do this better?
            wavePath = wavePath.Replace("#", "");
            wavePath = wavePath.Replace(")", "");
            wavePath = wavePath.Replace("(", "");
            wavePath = wavePath.Replace(Path.GetExtension(wavePath), ".vsnd");
            return $"resource:\"sounds/{wavePath.Replace("\\", "/")}\",";
        }

        public override string ToString()
        {
            return @$"<!-- kv3 encoding:text:version{{e21c7f3c-8a33-41c5-9977-a76d3a32aa0d}} format:generic:version{{7412167c-06e9-4698-aff2-e63eb59037e7}} -->
{{
	data = 
	{{
		UI = false
		Volume = {volume}
		VolumeRandom = {randomVolume}
		Pitch = {pitch}
		PitchRandom = {randomPitch}
		Decibels = 70
		Sounds = {ArrayOrNull(sounds)}
		SelectionMode = 3
		MaximumDistance = 1143.9335
		MinimumDistance = 57.196674
	}}
}}";
        }
    }
}
