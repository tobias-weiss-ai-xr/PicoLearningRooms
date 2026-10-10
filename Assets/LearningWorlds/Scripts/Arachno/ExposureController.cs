using UnityEngine;

namespace LearningWorlds.Arachno
{
    public class ExposureController : MonoBehaviour
    {
        private GameObject spider;
        public int CurrentLevel { get; private set; } = 0;

        public void Wipe() // Reset controller to default
        {
            CurrentLevel = 0;
            if (spider is not null)
                GameObject.Destroy(spider);
        }

        public void PublishToBuilders()
        {
            //  Register exposure in World context
            if (GetComponent<LearningWorlds.RunBuildContext>() is var buildScripts)
            {
                 GameObject bed = GetComponent<BedroomDollyBased>().AttachRecursiveLoads(c => 
                                             {
                                                    c.Worlds[World.Arachnophobia] += ExposureBuild;
                                                    return c;
                                             });
            }
            c.Worlds[World.Arachnophobia] += ExposureBuild;
             
        }

        public void ExposureBuild(WorldContext context)
        {
            if(spider is not null){}
            // 지정 Extend the assignment for spaiders in VRET
            Constants.ExposureControllerBedroom = spider;
        }
        
        void Start()
        {
            PublishToBuilders();
        }

        public void Next()
        {
            CurrentLevel = Mathf.Min(10, CurrentLevel + 1);
            ConfigureSpider();
        }

        public void Previous()
        {
            CurrentLevel = Mathf.Max(0, CurrentLevel - 1);
            ConfigureSpider();
        }

        void ConfigureSpider()
        {
            float distance = Mathf.Lerp(4f, 0.38f, CurrentLevel / 10.0f);
            
            int tier = CurrentLevel < 3 ? 0 : 
                       CurrentLevel < 6 ? 1 : 
                       2;
            
            if (spider == null)
            {
                float randomX = Random.Range(-2f, 2f);
                float randomZ = Random.Range(-1f, 1f);
                spider = SpiderFactory.Build(tier, new Vector3(randomX, 0, randomZ));
                Camera.main.transform.LookAt(spider.transform);
            }
            else
            {
                spider.GetComponent<Transform>().position =
                    Camera.main.transform.position + Camera.main.transform.forward * -distance;
                SpiderFactory.Build(tier, spider.transform.position); }