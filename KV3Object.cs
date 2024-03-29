﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace KVSurfaceUpdater
{
    internal class KV3Object
    {
        public static string Prefix = "";

        public string Name { get; set; }

        public KV3Object(KVObject sourceObject)
        {
            SetupWithKVObject(sourceObject);
        }

        protected virtual void SetupWithKVObject(KVObject sourceObject)
        {
            
        }

        protected static float TryParseFloatOrDefault(KVValue? value)
        {
            if(value != null)
            {
                if (float.TryParse((string)value, out float val))
                {
                    return val;
                }
            }

            return 0f;
        }

        protected static float FloatOrDefault(KVValue? value)
        {
            return value == null ? 0f : (float)value;
        }

        protected static string StringOrDefault(KVValue? value)
        {
            return value == null ? "" : (string)value;
        }

        protected static float[] GetFloatArray(string values)
        {
            var splitValues = values.Split(",");
            float[] floats = new float[splitValues.Length];
            for (int i = 0; i < splitValues.Length; i++)
            {
                floats[i] = float.Parse(splitValues[i]);
            }
            
            return floats;
        }

        protected static string ArrayOrNull(IEnumerable<string> input)
        {
            if(input == null || input.Count() == 0)
            {
                return "null";
            }
            else
            {
return @$"
[
{string.Join("\n", input)}
]";

            }
        }
    }
}
