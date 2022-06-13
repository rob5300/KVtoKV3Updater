using System.Text;
using ValveKeyValue;

namespace KVSurfaceUpdater
{
    internal class KV3Decal : KV3Object
    {
        public List<string> decals { get; private set; }

        public KV3Decal(KVObject sourceObject) : base(sourceObject)
        {
        }

        protected override void SetupWithKVObject(KVObject sourceObject)
        {
            Name = sourceObject.Name;
            decals = sourceObject.Children.Select(x => x.Name).ToList();
        }

        public override string ToString()
        {
            //Create the blocks for each decal entry
            StringBuilder decalSectionsBuilder = new StringBuilder();
            foreach(string decal in decals)
            {
                decalSectionsBuilder.Append(@$"
            {{
				material = resource:""materials/{decal}.vmat""
				depth = [15.0, 3.0, 0.0]
				keepaspect = true
				width = [8.0, 5.0, 0.0]
				height = [8.0, 5.0, 0.0]
			}},");
            }

            return @$"<!-- kv3 encoding:text:version{{e21c7f3c-8a33-41c5-9977-a76d3a32aa0d}} format:generic:version{{7412167c-06e9-4698-aff2-e63eb59037e7}} -->
{{
	data = 
	{{
		decals = 
		[{decalSectionsBuilder}
		]
	}}
}}";
        }
    }
}
