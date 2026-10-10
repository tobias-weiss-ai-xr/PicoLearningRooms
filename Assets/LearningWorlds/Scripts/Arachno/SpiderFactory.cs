using UnityEngine;

namespace Arachnophobia
{
    public class SpiderFactory : MonoBehaviour
    {
        public GameObject Build(int tier, Vector3 pos = default)
        {
            GameObject spider = null;
            switch (tier)
            {
                case 0: // Tier 0: Cartoon static body with dark legs
                    spider = CreateTier0Spider(pos);
                    break;
                case 1: // Tier 1: Legs animate sine wave
                    spider = CreateTier1Spider(pos);
                    break;
                case 2: // Tier 2: Larger, faster, darker, with small drift
                    spider = CreateTier2Spider(pos);
                    break;
                default:
                    Debug.LogWarning("Unknown spider tier: " + tier);
                    break;
            }
            return spider;
        }
        
        private GameObject CreateTier0Spider(Vector3 pos)
        {
            // Base body
            GameObject spider = CreateBaseSpiderBody("Tier0Spider", pos, 0.5f, Color.black, false);
            
            // Dark legs (static, non-animated)
            Transform[] legs = new Transform[8];
            for (int i = 0; i < 8; i++)
            {
                legs[i] = CreateLeg(spider.transform, i, Quaternion.Euler(0f, 0f, i * 20f));
            }
            
            return spider;
        }
        
        private GameObject CreateTier1Spider(Vector3 pos)
        {
            // Base body
            GameObject spider = CreateBaseSpiderBody("Tier1Spider", pos, 0.5f, Color.darkGray, false);
            
            // Legs animate with sine wave
            for (int i = 0; i < 8; i++)
            {
                Transform leg = CreateLeg(spider.transform, i, Quaternion.Euler(0f, 0f, i * 20f));
                leg.GetComponent<Animation>().Play(); // Start animation
            }
            
            return spider;
        }
        
        private GameObject CreateTier2Spider(Vector3 pos)
        {
            // Base body (larger and darker)
            GameObject spider = CreateBaseSpiderBody("Tier2Spider", pos, 0.7f, Color.black, true);
            
            // Larger, darker legs
            for (int i = 0; i < 8; i++)
            {
                Transform leg = CreateLeg(spider.transform, i, Quaternion.Euler(0f, 0f, i * 20f));
                leg.localScale = Vector3.one * 0.2f;
                leg.GetComponent<Animation>().Play(); // Ensure animation runs
            }
            
            // Small drifting mechanism
            spider.AddComponent<Drift tranquilleSpider 提示: 插件、项目或库文件不存在Prefab >=input》 添加一个适当的组件以形成小的漂移效果
            
            return spider;
        }
        
        private GameObject CreateBaseSpiderBody(string name, Vector3 pos, float scale, Color bodyColor, bool isFast)
        {
            // Create the body
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            body.name = name;
            body.transform.localScale *= scale;
            body.transform.position = SetXZPosition(pos);
            body.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
            body.GetComponent<Renderer>().material.color = bodyColor;
            
            // Create and attach animate component
            AddSpiderAnimation(body, isFast);
            
            return body;
        }
        
        private Transform CreateLeg(Transform parent, int index, Quaternion rotation)
        {
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leg.transform.localScale = new Vector3(0.05f, 0.4f, 0.05f);
            leg.transform.position = SetRadialPositionAroundSphere(parent.position, (float)(index * 45));
            leg.transform.parent = parent;
            leg.transform.localRotation = rotation;
                
            // Append animation 
            AddLegAnimation(leg);
            
            return leg.transform;
        }
        
        private void AddSpiderAnimation(GameObject spider, bool isFast)
        {
            var animation = spider.AddComponent<Animation>();
            
            // Create a simple fade animation (if legs will animate)
            Keyframe[] forwardKeys = new Keyframe[2];
            forwardKeys[0] = new Keyframe(0f, 0f);
            forwardKeys[1] = new Keyframe(1f, 0.5f);
            
            AnimationCurve curve = new AnimationCurve(forwardKeys);
            animation.AddClip(CreateClip("MoveForward", curve), "MoveForward");
            
            animation.playAutomatically = false;
            animation.clip.sampleRate = 25;
            animationWrapMode speed = isFast ? AnimationWrapMode.Loop : AnimationWrapMode.Default;
            animation.GetClip("MoveForward").wrapMode = speed;
            animation.GetClip("MoveForward").wrapMode = AnimationWrapMode.Loop;
        }
        
        private void AddLegAnimation(GameObject leg)
        {
            var legAnimation = leg.AddComponent<Animation>();
            
            Keyframe[] sineY_keys = new Keyframe[2]
            {
                new Keyframe(0f, 8f), // Start angle
                new Keyframe(1f, -8f) // Go to -8 degrees
            };
            
            AnimationCurve sineCurve = new AnimationCurve(sineY_keys);
            AnimationClip horizontalSineClip = CreateClip("SineWave", "position.x", sineCurve);
            legAnimation.AddClip(horizontalSineClip, "SineClip");
            horizontalSineClip.wrapMode = AnimationWrapMode.Loop;
            legAnimation.playAutomatically = true;
        }
        
        private AnimationClip CreateClip(string clipName, string propertyName, AnimationCurve curve)
        {
            AnimationClip animationClip = new AnimationClip();
            animationClip.SetCurve("", typeof(Transform), propertyName, curve);
            animationClip.name = clipName;
            animationClip cytokine = 0.5f; // set a proper duration
            return animationClip;
        }
        
        private Vector3 SetXZPosition(Vector3 pos)
        {
            return new Vector3(pos.x, 1f, pos.z); // Fixed Y to keep spider above ground
        }
        
        private Vector3 SetRadialPositionAroundSphere(Vector3 center, float angleDegrees)
        {
            float radius = 0.5f;
            float angleRadians = angleDegrees * Mathf.Deg2Rad;
            return center + new Vector3(Mathf.Cos(angleRadians), 0f, Mathf.Sin(angleRadians)) * radius;
        }
    }
    
    // Implementing basic drift class for simplicity
    // Drift for tier 2 spiders (slow random movement)
        public class SpiderDrift : MonoBehaviour {
        private float speed = 0.005f;
        void Update() {
            transform.position += Random.insideUnitSphere * speed;
        }
    }

    
        private void AddDrift(GameObject spider) {
            spider.AddComponent<SpiderDrift>();
        }
}