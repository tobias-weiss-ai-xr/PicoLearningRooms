using System.Collections;
using UnityEngine;

public class SpiderFactory : MonoBehaviour {
    public GameObject Build(int tier, Vector3 pos) {
        Vector3 spiderPos = new Vector3(pos.x, 1f, pos.z); 

        GameObject spider = new GameObject("Spider");

        if (tier >= 0 && tier <= 2) {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            body.name = "Body";
            body.transform.parent = spider.transform;
            body.transform.localScale = tier == 2 ? new Vector3(0.7f, 0.7f, 0.7f) : new Vector3(0.5f, 0.5f, 0.5f);
            body.transform.position = spiderPos;
            body.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = tier == 2 ? Color.black : Color.darkGray };

            // Create legs
            GameObject[] legs = new GameObject[8]; 
            for (int i = 0; i < 8; i++) {
                legs[i] = CreateSpideLeg(tier, spiderPos, i);
                legs[i].transform.parent = spider.transform;
            }
            if (tier == 1) {
                // Animate some sine waves for tier 1
                AddSineMotion(spider, 0, 40);
            } else if (tier == 2) {
                spider.AddComponent<SpiderDrift>();
            }
            spider.transform.position = spiderPos;
        }

        return spider;
    }

    GameObject CreateSpideLeg(int tier, Vector3 spiderPos, int i) {
        GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        leg.name = "Leg" + i;
        leg.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) { color = Color.black };
        Quaternion rotation = Quaternion.Euler(0f, 0f, i * 20f);

        leg.transform.rotation = rotation;
        Vector3 position = spiderPos;
        position.x += Mathf.Cos((float)((i * 45) * Mathf.Deg2Rad)) * 0.5f;
        position.z += Mathf.Sin((float)((i * 45) * Mathf.Deg2Rad)) * 0.5f;
        leg.transform.position = position;
        leg.transform.localScale = new Vector3(0.05f, tier == 2 ? 0.7f : tier == 0 ? 0.4f : 0.5f, 0.05f);

        if (tier != 0) {
            StartCoroutine(MoveLegContinuously(leg.transform, tier == 2 ? 7f : 4f));
        }

        return leg;
    }

    // Optional sine wave animation for tier if needed
    void AddSineMotion(GameObject baseObject, float startPosition, float amplitude)
    {
        StartCoroutine(PulseBase(baseObject.transform, amplitude));
        
    }

    IEnumerator MoveLegContinuously(Transform leg, float timeFactor) {
        float yAngle = 0f;
        while (true) {
            float sineWave = Mathf.Sin(yAngle * 0.05f) * 0.3f;
            leg.localPosition = new Vector3(0f,
                sineWave, 
                0f);
            yAngle += timeFactor;
            yield return null;
        }
        
    }

    IEnumerator PulseBase(Transform baseTransform, float amplitude) {
        float pulseTime = 0f;
        while (true) {
            float pulseFactor = amplitude * Mathf.PingPong(pulseTime * 0.5f, 1f);
            baseTransform.localScale = new Vector3(1f + pulseFactor, 1f + pulseFactor, 1f + pulseFactor);
            pulseTime += 1f;
            yield return null;
        }
    }
}
    
// Essential Drift for tier 2 spiders to mimic wandering
    public class SpiderDrift : MonoBehaviour {
        private float speed = 0.005f;
        
        void Update() {
            transform.position += Random.insideUnitSphere * speed;
        }
    }
