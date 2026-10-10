using UnityEngine;

namespace LearningWorlds.Arachno
{
    public static class SpiderFactory
    {
        /// <summary>
        /// Creates a tiered spider with the given position.
        /// 
        /// Tiers:
        /// 0: Cartoon static body (2 spheres + 8 cylinders, no animation)
        /// 1: Legs animate with sine waves (more realistic)
        /// 2: Larger, darker, faster movement, and subtle drift
        /// Distance: Unit free spacing
        /// </summary>
        public static GameObject Build(int tier, Vector3 pos)
        {
            GameObject spider = new GameObject("SpiderTier" + tier);
            spider.transform.position = pos;
            spider.AddComponent<Rigidbody>().isKinematic = true;
            
            if (tier == 2)
            {
                // Tier 2: adds drift component
                spider.AddComponent<SpiderDrift>().SetDrift(0.05f);
            }
            
            CreateSpiderBody(tier, spider.transform);
            CreateSpiderLegs(tier, spider.transform);
            
            return spider;
        }
        
        static void CreateSpiderBody(int tier, Transform spider)
        {
            GameObject body;
            
            if (tier == 0)
            {
                // Cartoon spider (two stacked spheres)
                body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                body.name = "SpiderCartoonBody";
                body.transform.SetParent(spider, true);
                body.transform.localPosition = Vector3.up * 0.12f;
                body.transform.localScale = new Vector3(0.17f, 0.15f, 0.17f);
                
                // Dark red cartoon color
                Material cartoonMaterial = new Material(Shader.Find("Standard")) {
                    color = new Color(0.95f, 0.02f, 0.02f, 1f)
                };
                body.GetComponent<Renderer>().material = cartoonMaterial;
                
                // Additional base sphere (eight legs roots)
                GameObject baseObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                baseObj.name = "SpiderLegBase";
                baseObj.transform.SetParent(spider, true);
                baseObj.transform.localScale = new Vector3(0.2f, 0.08f, 0.2f);
                RuntimeUtils.SetMaterial(baseObj.GetComponent<Renderer>(), new Material(Shader.Find("Standard")) {
                    color = new Color(0.4f, 0.05f, 0.02f, 0.8f)
                });
            }
            else
            {
                // Tier 1 & 2: Realistic spider body
                body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                body.name = "SpiderBody";
                body.transform.SetParent(spider, true);
                
                float sizeScale = (tier == 2) ? 1.1f : 1.0f;
                body.transform.localScale = new Vector3(0.12f + sizeScale * 0.02f, sizeScale * 0.1f, 0.12f + sizeScale * 0.02f);
                
                // Darker shading for higher tiers
                float darknessFactor = tier == 2 ? 0.2f : 0.3f;
                Material bodyMaterial = new Material(Shader.Find("Standard")) {
                    color = new Color(0.01f, 0.03f + darknessFactor, 0.08f + darknessFactor * 0.02f, 1f),
                    enableInstancing = true
                };
                body.GetComponent<Renderer>().material = bodyMaterial;
            }
        }
        
        static void CreateSpiderLegs(int tier, Transform spider)
        {
            for (int i = 0; i < 8; i++)
            {
                CreateLeg(tier, spider, i);
            }
        }
        
        static void CreateLeg(int tier, Transform spider, int legIndex)
        {
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leg.name = "Leg_" + (legIndex + 1);
            leg.transform.SetParent(spider);
            
            // Scale legs based on tier
            float tierScaleFactor = tier == 2 ? 1.4f : (tier == 1 ? 1.2f : 1.0f);
            
            UnityEngine.Quaternion rotation = Quaternion.Euler(0, 0, legIndex * (225 / 8f));
            leg.transform.localRotation = rotation; // Radial positioning
            leg.transform.localEulerAngles = new Vector3(90f, leg.transform.localEulerAngles.y, leg.transform.localEulerAngles.z); // Stand upright
            
            // Dark yellowish legs with tier-dependent opacity
            Material legMaterial = new Material(Shader.Find("Standard")) {
                color = new Color(0.5f, 0.35f, 0f, (tier == 0) ? 1f : 0.8f)
            };
            leg.GetComponent<Renderer>().material = legMaterial;
            
            // Position leg offset slightly based on tier
            float legHeightFactor = tier == 2 ? 1.3f : (tier == 1 ? 1.15f : 1.0f);
            leg.transform.localPosition += Vector3.up * 0.07f * legHeightFactor;
            leg.transform.localPosition += Vector3.forward * 0.1f * tierScaleFactor;
            
            // Tier 0: just a cylinder
            // Tier 1 & 2: animate with sine
            if (tier >= 1)
            {
                leg.AddComponent<LegSineAnimator>().Initialize(tier, legIndex);
            }
            
            // Scale leg proportions
            Vector3 legScale = new Vector3(0.015f * tierScaleFactor, 0.15f * tierScaleFactor, 0.015f * tierScaleFactor);
            leg.transform.localScale = legScale;
        }
    }
    
    public class SpiderDrift : MonoBehaviour
    {
        private float DriftSpeed = 0.5f;
        private Vector3 InitialVelocity = Vector3.zero;
        
        public void Start()
        {
            GetComponent<Rigidbody>().velocity = InitialVelocity;
            InitialVelocity = Vector3.zero;
        }
        
        public void SetDrift(float value)
        {
            DriftSpeed = value;
        }
        
        void Update()
        {
            Rigidbody body = GetComponent<Rigidbody>();
            InitialVelocity = new Vector3(
                Mathf.Sin(Time.time * 0.01f) * 0.004f,
                0.0f,
                Mathf.Cos(Time.time * 0.01f) * 0.0025f
            ) * DriftSpeed;
            
            // Smooth drift
            body.velocity = Vector3.Lerp(body.velocity, InitialVelocity, 0.3f);
            body.position += body.velocity * Time.deltaTime;
        }
    }
    
    public class LegSineAnimator : MonoBehaviour
    {
        private bool ApplyXOffset = false;
        private bool ApplyZOffset = true;
        private float LegPeriod = 0f;
        private int LegIndex = 0;
        public void Initialize(int tier, int index)
        {
            LegIndex = index;
            ApplyXOffset = (tier == 1);
            LegPeriod = tier == 2 ? 2f : (tier == 1 ? 3f : 1f);
        }
        void Update()
        {
            Vector3 animatedLegPos = transform.localPosition;
            float sineFactor = 0.01f * Mathf.Sin(Time.time * LegPeriod * 2.0f);
            if (ApplyXOffset)
            {
                animatedLegPos.x += Mathf.Sin(Time.time * LegPeriod * 2f + Math.PI * LegIndex) * sineFactor;
            }
            if (ApplyZOffset)
            {
                animatedLegPos.z += Mathf.Sin(Time.time * LegPeriod * 3f + Math.PI * LegIndex) * sineFactor * 0.88f;
            }
            animatedLegPos.y += Mathf.Sin(Time.time * LegPeriod * 3.8f) * sineFactor * 0.25f;
            
            transform.localPosition = animatedLegPos;
        }
    }
    
    static class RuntimeUtils
    {
        public static void SetMaterial(Renderer renderer, Material newMaterial)
        {
            renderer.sharedMaterial = newMaterial;
        }
    }
}