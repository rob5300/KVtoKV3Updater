using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValveKeyValue;

namespace KVSurfaceUpdater
{
    internal class KV3Sound : KV3Object
    {
        private static Dictionary<string, float> SoundLevels = new Dictionary<string, float>();
        
        static KV3Sound()
        {
            string[] soundLevelsSource = @"//	SNDLVL_NONE		= 0,
//	SNDLVL_25dB		= 25,
//	SNDLVL_30dB		= 30,
//	SNDLVL_35dB		= 35,
//	SNDLVL_40dB		= 40,
//	SNDLVL_45dB		= 45,
//	SNDLVL_50dB		= 50,	// 3.9
//	SNDLVL_55dB		= 55,	// 3.0
//	SNDLVL_IDLE		= 60,	// 2.0
//	SNDLVL_TALKING	= 60,	// 2.0
//	SNDLVL_60dB		= 60,	// 2.0
//	SNDLVL_65dB		= 65,	// 1.5
//	SNDLVL_STATIC	= 66,	// 1.25
//	SNDLVL_70dB		= 70,	// 1.0
//	SNDLVL_NORM		= 75,
//	SNDLVL_75dB		= 75,	// 0.8
//	SNDLVL_80dB		= 80,	// 0.7
//	SNDLVL_85dB		= 85,	// 0.6
//	SNDLVL_90dB		= 90,	// 0.5
//	SNDLVL_95dB		= 95,
//	SNDLVL_100dB	= 100,	// 0.4
//	SNDLVL_105dB	= 105,
//	SNDLVL_120dB	= 120,
//	SNDLVL_130dB	= 130,
//	SNDLVL_GUNFIRE	= 140,	// 0.27
//	SNDLVL_140dB	= 140,	// 0.2
//	SNDLVL_150dB	= 150,	// 0.2"
.Replace(" ", string.Empty).Replace("\t", "").Split("\n");

            foreach (string line in soundLevelsSource)
            {
                string[] split = line.Split('=');
                var name = split[0].Replace("/", "");
                var value = split[1];
                int commaIndex = value.IndexOf(',');
                if(commaIndex >= 0)
                {
                    value = value.Substring(0, commaIndex);
                }
                SoundLevels.Add(name, float.Parse(value));
            }
        }

        List<string> sounds = new List<string>();
        
        KVValue? channel;

        float pitch = 1f;
        float volume = 1f;
        float soundlevel;
        float randomVolume;
        float randomPitch;

        public KV3Sound(KVObject sourceObject) : base(sourceObject)
        {
        }

        protected override void SetupWithKVObject(KVObject sourceObject)
        {
            Name = sourceObject.Name;
            channel = sourceObject["channel"];
            var kvSoundlevel = sourceObject["soundlevel"];
            if(kvSoundlevel != null)
            {
                switch(kvSoundlevel.ValueType)
                {
                    case KVValueType.String:
                        string stringSoundLevel = (string)kvSoundlevel;
                        if (!SoundLevels.TryGetValue(stringSoundLevel, out soundlevel))
                        {
                            //If we failed to find a direct translation in the dictionary, pull value strait from string
                            var match = Regex.Match(stringSoundLevel, @"(?<=SNDLVL_)\d*(?=dB)");
                            if (match.Success)
                            {
                                soundlevel = float.Parse(match.Value);
                            }
                        }
                        break;
                    case KVValueType.Int32:
                        soundlevel = (int)kvSoundlevel;
                        break;
                    default:
                        soundlevel = (float)kvSoundlevel;
                        break;
                }
            }

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

        private static string[] SoundCharacters = new[]
        {
            // Full list available here:
            // https://developer.valvesoftware.com/wiki/Soundscripts#Sound_Characters

            "*", // CHAR_STREAM
            "#", // CHAR_DRYMIX
            "@", // CHAR_OMNI
            ">", // CHAR_DOPPLER
            "<", // CHAR_DIRECTIONAL
            "^", // CHAR_DISTVARIANT
            ")", // CHAR_SPATIALSTEREO
            "}", // CHAR_FAST_PITCH
            "$", // CHAR_CRITICAL 
            "!", // CHAR_SENTENCE 
            "?", // CHAR_USERVOX
            "&", // CHAR_HRTF_FORCE 
            "~", // CHAR_HRTF
            "`", // CHAR_HRTF_BLEND 
            "+", // CHAR_RADIO 
            "(", // CHAR_DIRSTEREO 
            "$", // CHAR_SUBTITLED 
            "%", // CHAR_MUSIC 
        };

        private static string ToSoundResource(KVValue wave)
        {
            string wavePath = (string)wave;

            // Remove Valve's Sound Characters
            foreach(var soundChar in SoundCharacters)
            {
                wavePath = wavePath.Replace(soundChar, "");
            }

            wavePath = wavePath.Replace(Path.GetExtension(wavePath), ".vsnd");
            return $"resource:\"sounds/{wavePath.Replace("\\", "/")}\",";
        }

        public override string ToString()
        {
            return @$"<!-- kv3 encoding:text:version{{e21c7f3c-8a33-41c5-9977-a76d3a32aa0d}} format:generic:version{{7412167c-06e9-4698-aff2-e63eb59037e7}} -->
{{
	data = 
	{{
		UI = {(soundlevel > 0f ? "false" : "true")}
		Volume = {volume}
		VolumeRandom = {randomVolume}
		Pitch = {pitch}
		PitchRandom = {randomPitch}
		Decibels = {soundlevel}
		Sounds = {ArrayOrNull(sounds)}
		SelectionMode = 3
	}}
}}";
        }
    }
}
