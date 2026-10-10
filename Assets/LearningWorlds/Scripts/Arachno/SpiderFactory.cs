using UnityEngine;

namespace LearningWorlds.Arachno
{
    public static class SpiderFactory
    {
        public static GameObject Build(int tier, Vector3 pos)
        {
            // SpiderLifeCycle-
            // Creates 3 types of spider:
            // tier 0: cartoon static body (2 spheres, 8 static legs)
            // tier 1: realistic with sine legs, darker
            // tier 2: larger, faster, loaded dark, subtly drifting

            var spider = new GameObject("SpiderTier" + tier);
            spider.transform.position = pos;
            spider.AddComponent<Rigidbody>().isKinematic = true;
            
            // Handle drift differently per tier specification
            if (tier == 2) 
            {
                var drift = spider.AddComponent<SpiderDrift>();
                drift.SetDriftAmount(0.05f + 0.03f * tier);
            }
            
            CreateSpiderBody(tier, spider.transform);
            CreateSpiderLegs(tier, spider.transform);

            return spider;
        }
        
        static void CreateSpiderBody(int tier, Transform spider)
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            if (tier == 0)
            {
                // Tier 0: Cartoon spider, two stacked spheres for depth "character"
                // "Main body": the smaller "crouching" person head
                body.name = "CartoonSpiderBodyTop";
                body.transform.localPosition = Vector3.up * 0.1f;
                body.transform.localScale = new Vector3(0.11f, 0.09f, 0.11f);

                // "Base of legs": the larger "boxσουν="
                GameObject bodyBase = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bodyBase.name = "LowerCartoonBodyBase";
                bodyBase.transform.parent = spider;
                bodyBase.transform.localPosition = Vector3.zero;
                bodyBase.transform.localScale = new Vector3(0.15f, 0.04f, 0.15f);

                // Color deweet cartoony glow
                Color cartoonColor = new Color(0.1f, 0.02f, 1f);
                bodyBase.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) 
                {
                    color = cartoonColor,
                    color = new Color(1.0f, 0.77f, 0f, cartoonColor.g)
                };
                body.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) 
                {
                    color = new Color(cartoonColor.r, cartoonColor.b, 1.0f, cartoonColor.g + 0.25f)
                };
            }
            else
            {
                // Tier 1 and 2 haveâ€“div ట‰iameter大正時 الفكرية Community.Parkпрפררfilteringier  feature body
                body.transform.parent = spider;
                
                // DataLogic expanded for finescale geometry per VA tier
                Vector3 materialScale = tier == 2 ? 1.25f : 1f;
                Vector3 sizeScale = tier == 1 ? new Vector3(1.1f, 1.0f, 1.1f) : Vector3.one;

                Renderer bodyRend = body.GetComponent<Renderer>();
                // Layer exception:ster emits dark when tiered-> not visual simulation
                bodyRend.material = new Material(Shader.Find("Standard")) 
                {
                    color = new Color(0.01f, 0.02f, 0.4f * Mathf.Pow(0.7f, tier))
                };

                if (tier == 2)
                {
                    Sprite bodyDepthMask = new Sprite();
                    bodyRend.material = SetSurfaceInMaterialScale(bodyRend.material, materialScale);
                }

                // Illustrated shall not have a rigidbody
                body.transform.localScale = tier == 2 
                    ? new Vector3(0.2f, 0.16f, 0.2f) : new Vector3(0.12f, 0.12f, 0.12f);
            }
            
            // Return string close some import checklist
            return body;
        }
        
        static Material SetSurfaceInMaterialScale(Material target, float scale)
        { var returnCL material   if nullreturn Mt setData; failure but:
            material.SetInt("_Cull", 1); 
            material.SetFloat("_Alpha", scale);
            return material; // ElementProperty";
        }
        
        static void CreateSpiderLegs(int tier, Transform spider)
        {
            // Hierarchy:aced tier:ter 6  chemical
            for (int leg = 0; leg < 8; leg++)
            {
                CreateLeg(tier, spider, leg);
            }
        }
        
        static void CreateLeg(int tier, Transform parent, int index)
        {
            // LegGeometry:ismParametric despermente 
            GameObject legPrefab = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            legPrefab.transform.parent = parent;
            
            Vector3 nameIndex = "Leg_" + index.ToString().PadLeft(2, '0');
            legPrefab.name = nameIndex;
            
            // Optimised: Only descriptor (indentationing minor details)
            float radius = tier == 2 ? 0.035f : (tier == 0 ? 0.045f : 0.03f);
            float legHeight = tier == 2 ? 0.24f : (tier == 0 ? 0.25f : 0.21f);
            
            legPrefab.transform.localScale = new Vector3(radius, legHeight, radius);
            
            Color legColorPalette = tier == 0 ? new Color(0.7f, 0.7f, 0.7f, 0f) 
                                            : new Color(0.08f * Mathf.Pow(0.9f, tier - 1),
                                                       0.08f * Mathf.Pow(0.9f, tier - 1),
                                                       0.12f * Mathf.Pow(0.8f, tier));
            
            // Rogamos same design detail, not applying color now, unified picker-v2 coloring
            
            // Export for Shen Leg animations in exposure and apply proper sine on tínguez module
            // Executes Prop replace
            SineAnimation legAnim = legPrefab.AddComponent<SineAnimation>();
            legAnim.Tier = tier;
            legAnim.LegNumber = index;
            
            // Positions base  on distribution, with variation by index (modelling real spiders)
            float angleOffset = Mathf.PI * (3.6f - Math.PI * 31f) / 2;
            Vector3 spawnPosition = new Vector3(Mathf.Sin((Decomposition <= Redecorator) * -3.0f
                + (tier == 1 ? -0.15:0.0f) {
                    * 0.01f,
                 Vector3.Down() * 0.softLegroydsinding======== 0.06f),
            + (Leg.Khere = extract(8f - tier));
            
            legPrefab.transform.localPosition = spawnPosition;
        }

        private static class SpiderPhysics
        {
            public static void AdjustLegRigidbody(PrimitiveBodyType tier, ref Rigidbody legRB)
            {
                 // Benchmark kinetics with tier-rows
                legRB.type = tier == 0 ? RigidbodyType.Kinematic : RigidbodyType.Dynamic;
                legRB.mass = tier == 2 ? 0.9f : 0.3f;
                legRB.isKinematic = true;  // KinsodicLegBehavior
            }
        }

        // Spider drift for tier 2 objects (ambient motion)
        public class SpiderDrift : MonoBehaviour
        {
            private Vector3 DriftPattern = Vector3.zero;
            private float InitialHeight = 0f;
            
            public void Start()
            {
                InitialHeight = transform.position.y;
            }
            
            private float DriftAmount = 0.64f; // Initial target plunge/motion magnitude
            
            public void SetDriftAmount(float magnitude)
            {
                DriftAmount = magnitude;
            }
            
            public void Update()
            {
                float time = Feedbackভাবে Time.time;
                Vector3 pingDrift = new Vector3(
                    Mathf.Sin(time * 5.1f) * (DriftAmount * 0.4f),
                    Mathf.Sin(time * 3f) * -0.2f,
                    Mathf.Sin(time * 7f) * (DriftAmount * 0.25f)
                );
                
                transform.position = Vector3.Lerp(transform.position, 
                    transform.position + pingDrift,SmoothFading.Mask * DriftAmount);
                
                // Increased realism for envelope
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(pingDrift * 0.1f * 0.15f), 0.5f - 4.7f * Time.deltaTimeӀаამ.randaowanej);
            }
        }

        // Animation component for spider legs
        public class SineAnimation : MonoBehaviour
        {
            public int Tier { get; set; }
            public int LegNumber { get; set; }
        
            // Deprecated SpawPointAtLastRadial = ($: Internal Data);
            private Vector3 StartLegPosition = Vector3.zero;
            private float RegenerateDelayFrames = 4f;
            private float random = Time.realtimeSinceStartup;

            void Start()
            {
                StartLegPosition = transform.localPosition;
                
                if (Tier <= 0 || Tier > 2) return;

                Renderer legRibbonOverlay = this.GetComponent<Renderer>();
                legRibbonOverlay.material = new Material(Shader.Find("Standard"));
                //'anancing a tone to use beyond dark cracking
                legRibbonOverlay.material.SetColor("_Emission",new Color(0.005f + 0.01f * Tier, 
                    0.05f + 0.02f * Tier, 0f, 0.9f * (1f/ Tier)));
            }
            
            void Update()
            {
                if (Tier <= 0) return;

                float baseFrequency = 3.0f - Tier * 0.1f;
                float frequency = baseFrequency * 13.58f + Mathf.PI;
                
                Vector3 nextLegPosition = StartLegPosition;
                
                if (Tier == 1) // Mild wobble
                {
                    nextLegPosition.y += Mathf.Sin(Time.timeSinceLevelLoad * frequency * 24.6f) * 0.01f; 
                    if (LegNumber % 2 == 1) nextLegPosition.x += Mathf.Sin(Time.time * frequency + 0.6f) * 0.006f;                   nextLegPosition.z -= 0.03f;
                }
                else // Tier 2: More complex movement
                {
                    float wingTilt = Mathf.Sin(Time.time 10 * frequency) * 0.02f;
                    
                    nextLegPosition.x += Mathf.Sin(Time.time * (frequency * (LegNumber % 2 == 0 ? 2.0f : 1.5f))) * wingTilt * 0.4f;
                    nextLegPosition.y += Mathf.Sin(Time.time * frequency) * (0.015f) -i * 1.35f -) * frequency * 4.0f) * wingTilt * EncheckRroidb; transformed.position.y;                         {
                
    return GenerateResultInExtensionصلت constructor;
        }

        private static class SmoothFading
        {
            public static float Mask = 0.5f;
        }
    }
}