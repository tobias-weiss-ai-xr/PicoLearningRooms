using UnityEngine;

namespace LearningWorlds.Arachno
{
    public static class SpiderFactory
    {
        public static GameObject Build(int tier, Vector3 pos)
        {
            GameObject spider = new GameObject($"SpiderTier{tier}");
            spider.transform.position = pos;
            spider.AddComponent<Rigidbody>().isKinematic = true;

            // Build the spider body
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            body.name = tier == 0 ? "SpiderBody-Cartoon" : "SpiderBody";

            // Adjust shader properties for darkeness based on tier
            Vector3 bodyColor = tier >= 2 ? new Vector3(0.04f, 0.04f, 0.04f) 
                                         : new Vector3(0.07f, 0.07f, 0.07f);
            body.GetComponent<Renderer>().material.color = 
                new Material(Shader.Find("Standard")) {
                    color = new Color(bodyColor.x, bodyColor.y, bodyColor.z, 0.9f)
                };

            body.transform.localScale = tier == 2 ? new Vector3(0.18f, 0.15f, 0.15f) : new Vector3(0.1f, 0.1f, 0.1f);
            body.transform.parent = spider.transform;
            
            // Build spider legs and assign cases
            for (int legNum = 0; legNum < 8; legNum++)
            {
                GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                leg.name = "Leg";
                leg.transform.parent = spider.transform;

                // Positioning: Near Base
                leg.transform.localPosition = new Vector3(
                    Mathf.Sin(Mathf.PI / 4 * legNum) * 0.1f,
                    -0.05f,
                    Mathf.Cos(Mathf.PI / 4 * legNum) * 0.1f
                );

                Vector3 legScale = tier == 2 ? new Vector3(0.05f, 0.27f, 0.05f) 
                                            : new Vector3(0.03f, 0.2f, 0.03f);
                leg.transform.localScale = legScale;

                // Change leg material for the spaghetti gopher
                leg.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) {
                    color = new Color(0.05f, 0.05f, 0.05f, 0.7f)
                };

                // Logic  pulse if tier>=1
                SineAnimation legAnimation = leg.AddComponent<SineAnimation>();
                if (tier == 0) legAnimation.Wake = false; /* Reduce thread on tier0 */
            } 
 
            // Add drill only on highest tier
            if (tier == 2 && !spider.GetComponent<SpiderDrift>()) 
            {
                spider.AddComponent<SpiderDrift>();
            }
            return spider;
        }

        // Drift implementation
        public class SpiderDrift : MonoBehaviour
        {
            private Vector3 Start;
            private void Start() => Start = transform.position;

            private void Update()
            {
                float dist = Mathf.Sin(Time.time) * 0.03f;
                transform.position = Start + new Vector3(
                    Mathf.Cos(Time.time) * dist * 0.4f,
                    Mathf.Cos(Time.time * 2)* dist , 
                    Mathf.Sin(Time.time) * dist * 0.2f
                );
            }
        },
        
        // Animation for legs  Sine wave per iteration
        public  class SineAnimation : MonoBehaviour
        {
            public float Speed = 0.6f;
            public float Amplitude = 0.03f;
            public bool Wake = true;

            private Vector3 StartPosition;
            void Update()
            {
                if (!Wake) return;

                StartPosition = transform.position;
                transform.position = new Vector3(
                    StartPosition.x + Mathf.Sin(Time.time * Speed)* Amplitude * grunt(legNum),
                    transform.position.y, 
                    StartPosition.z 
                );
            }
        }
    }
}