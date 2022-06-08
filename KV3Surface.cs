using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace KVSurfaceUpdater
{
    internal class KV3Surface
    {
		public static string Prefix = "";

		public string Name { get; set; }

		public KVValue? Base;
		public KVValue? density;
		public KVValue? elasticity;
		public KVValue? friction;
		public KVValue? dampening;
		public KVValue? stepleft;
		public KVValue? stepright;
		public KVValue? impacthard;
		public KVValue? impactsoft;
		public KVValue? bulletimpact;
		public KVValue? Break;
		public KVValue? scraperough;
		public KVValue? scrapesmooth;
		public KVValue? thickness;

		public KVValue? gamematerial;
		public KV3Decal? gamematerial_decal = null;

		public KV3Surface(KVObject sourceObject)
        {
			Name = sourceObject.Name;
            
			Base = sourceObject["base"];
			density = sourceObject["density"];
            elasticity = sourceObject["elasticity"];
			friction = sourceObject["friction"];
			dampening = sourceObject["dampening"];
            stepleft = sourceObject["stepleft"];
			stepright = sourceObject["stepright"];
			impacthard = sourceObject["impacthard"];
			impactsoft = sourceObject["impactsoft"];
			bulletimpact = sourceObject["bulletimpact"];
			Break = sourceObject["break"];
			gamematerial = sourceObject["gamematerial"];
			scraperough = sourceObject["scraperough"];
			scrapesmooth = sourceObject["scrapesmooth"];
			thickness = sourceObject["thickness"];
		}

        public override string ToString()
        {
            return @$"<!-- kv3 encoding:text:version{{e21c7f3c-8a33-41c5-9977-a76d3a32aa0d}} format:generic:version{{7412167c-06e9-4698-aff2-e63eb59037e7}} -->
{{
	data = 
	{{
		BaseSurface = ""{(Base == null ? "data/surface/default.surface" : $"data/surface/{Prefix + (string)Base}.surface")}""
		AudioMaterial = null
		Description = ""{Name}""
		Friction = {FloatOrDefault(friction)}
		Elasticity = {FloatOrDefault(elasticity)}
		Density = {FloatOrDefault(density)}
		Thickness = {FloatOrDefault(thickness)}
		Dampening = {FloatOrDefault(dampening)}
		BounceThreshold = 50
		ImpactEffects = 
		{{
			Regular = 
			[
				resource:"""",
			]
			Bullet =
			[
				resource:"""",
			]
			BulletDecal =
			[
				resource:""{(gamematerial_decal == null ? "" : $"data/decal/{gamematerial_decal.Name.ToLower()}.decal")}"",
			]
		}}
		Sounds = 
		{{
			FootLeft = ""{StringOrDefault(stepleft)}""
			FootRight = ""{StringOrDefault(stepright)}""
			FootLaunch = """"
			FootLand = """"
			Bullet = ""{StringOrDefault(bulletimpact)}""
			SmoothScrape = ""{StringOrDefault(scrapesmooth)}""
			RoughScrape = ""{StringOrDefault(scraperough)}""
			ImpactHard = ""{StringOrDefault(impacthard)}""
			ImpactSoft = ""{StringOrDefault(impactsoft)}""
		}}
		Breakables = 
		{{
			BreakSound = """"
			GenericGibs = null
		}}
	}}
}}";
        }

		private static float FloatOrDefault(KVValue? value)
        {
			return value == null ? 0f : (float)value;
        }

		private static string StringOrDefault(KVValue? value)
        {
			return value == null ? "" : (string)value;
		}
    }
}
