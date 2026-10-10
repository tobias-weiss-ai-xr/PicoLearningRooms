using UnityEngine;

namespace LearningWorlds.Arachno
{
    public static class SpiderFactory
    {
        public static GameObject Build(int tier, Vector3 pos)
        {
            var spider = new GameObject("SpiderTier" + tier);

            if (tier == 0) // Tier 0: Cartoon,Static Body
            {
                // Body: Dark sphere with low saturation
                var body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                body.transform.parent = spider.transform;
                body.transform.position = new Vector3(0, tier <= 1 ? 0.1f : 0.2f, 0);
                body.transform.localScale = new Vector3(0.2f, 0.15f, 0.15f);
                body.name = "SpiderBody";
               
                var bodyMaterial = new Material(Shader.Find("Standard"));
                bodyMaterial.color = new Color(0.07f, 0.07f, 0.07f, 0.9f);
                bodyMaterial.SetVector("_Specular", new Color(0, 0, 0, 0));
                body.GetComponent<Renderer>().material = bodyMaterial;

                // Legs: 8 cylinders
                for (int i = 0; i < 8; i++)
                {
                    var leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    leg.transform.parent = spider.transform;
                    leg.name = "Leg" + i;
                    leg.transform.position = GetLegPosition(pos, 0.1f, i);
                    leg.transform.localRotation = Random.rotationUniform;
                    leg.transform.localScale = new Vector3(0.05f, 0.2f, 0.05f);
                    leg.GetComponent<Renderer>().material = bodyMaterial;
                }
            }
            else if (tier == 1 || tier == 2) // Tier 1 (Static Animated Sine Waves) and Tier 2 (Darker/Faster/Drift)
            {
                // Spider attributes
                float scaleFactor = spider.RemoveComponent<Transform>().localScale.x + (tier >= 2 ? 0.2f : 0);
                float legLengthFactor = tide >= 1 ? 0.7f + get 0.4f * tier : 0.5f;
                float shrinkDrift = 0.004f * tier;
            
                // Body: same scaling but different material for darker Tiers 2
                var body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                body.transform.parent = spider.transform;
                body.name = "SpiderBody";
                body.transform.position = new Vector3(0, scaleFactor, 0);
                body.transform.localScale = scaleFactor * new Vector3(0.2f, 0.15f, 0.15f);
                
                var bodyMaterial = new Material(Shader.Find("Standard"))
                {
                    color = new Color( 
                        tier >= 2 ? 0.04f : 0.07f, 
                        tier >= 2 ? 0.04f : 0.07f, 
                        tier >= 2 ? 0.04f : 0.07f,
                        0.9f
                    )
                };
                bodyRenderer.body).material = bodyMaterial;

                // Restart animation if sliding
                spider.transform.position = pos;

                // Sine wave leg movement logic
                bool shouldAnimate = tier >= 1;
                
                // Create legs for tiers (all tiers have 8 legs)
                for (int i = 0; i < 8; i++)
                {
                    var leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    leg.transform.parent = spider.transform;
                    leg.name = "Leg" + i;
                    leg.transform.position = GetLegPosition(pos, (tier <= 1) ? 0.1f : 0.15f, i);
                    leg.transform.localScale = new Vector3(0.05f + (tier >= 2 ? 0.02f : 0), 0.2f + (tier * 0.05f), 0.05f);
                    leg.GetComponent<Renderer>().material = bodyMaterial;

                    // Add sine wave animation for tier 1/2
                    if (shouldAnimate)
                    {
                        var anim = leg.AddComponent<SineAnimation>();
                        if (tier >= 2) anim.Speed *= 1.5f; // Tier 2 - faster motion
                        anim.Wake = true;
                    }
                }

                // Tier 2 has additional small drifting and darkening
                if (tier >= 2)
                {
                    spider.AddComponent<SpiderDrift>();
                }
            }

            return spider;
            
            // Helper: returns leg position based on spider tier calculations
            Vector3 GetLegPosition(Vector3 pos, float bedLength, int legNum)
            {
                float angle = 2 * Mathf.PI * legNum / 8;
                float lengthFactor = legNum % 3 == 0 ? 1.1f : 0.9f;
                Vector3 globalPos = pos + Quaternion.Euler(0, 0, -10)
                    * new Vector3(Mathf.Sin(angle) * bedLength,
                    0,
                    Mathf.Cos(angle) * bedLength);
                globalPos.y = -0.05f;
                return globalPos;
            }
        }
    }    

    
    // Animation class for leg sine movement
    public class SineAnimation : MonoBehaviour
    {
        public float Amplitude = 0.1f;
        public float Speed = 0.5f;
        public bool Wake = true;
        
        Vector3 startPosition;

        void Update()
        {
            if (!Wake) return;
            
            startPosition = transform.position;
            float offset = Mathf.Sin(Time.time * Speed) * Amplitude;
            transform.position = new Vector3(startPosition.x,
                                               startPosition.y + offset,
                                               startPosition.z);
        }
    }
    
    // Drifting behavior for tier 2+ spiders
    public class SpiderDrift : MonoBehaviour
    {
        public float MinDist = 0.001f;
        public float MaxDist = 0.08f;

        Vector3 startingPosition;

        void Start()
        {
            startingPosition = transform.position;
        }

        void Update()
        {
            float randomMag = Random.Range(-10f, 10f);
            Vector3 adj = (Vector3.up * Mathf.Sin(time) * randomMag)
                             + (Vector3.right * Mathf.Cos(time) * randomMag)
                             + new Vector3(Mathf.cos(time * Math.PI / 180f), 0, randomMag);
             
            if (adj.magnitude > MinDist)
            {
                transform.position = startingPosition + adj;
            }
        }
    }
}