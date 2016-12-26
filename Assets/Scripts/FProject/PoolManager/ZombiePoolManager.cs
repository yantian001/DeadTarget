using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FProject
{
    public class ZombiePoolManager : SingletonMono<ZombiePoolManager>
    {

        //  private Transform[] zombiePrefabs = new Transform[21];
        private Dictionary<ZombieModel, Transform> zombiePrefabs = new Dictionary<ZombieModel, Transform>();
        private Dictionary<ZombieModel, List<Material>> zombieMatrials = new Dictionary<ZombieModel, List<Material>>();
        // Use this for initialization
        void Start()
        {
            // zombiePrefabs.Add(ZombieModel.FatDaddy)

            AddPrefab(ZombieModel.FatDaddy, "Prefabs/FProject/Zombie/FatZombie");
            AddMaterial(ZombieModel.FatDaddy, "Materials/ZN_Fat1");
            AddMaterial(ZombieModel.FatDaddy, "Materials/ZN_Fat2");
            AddMaterial(ZombieModel.FatDaddy, "Materials/ZN_Fat3");
            AddPrefab(ZombieModel.Female, "Prefabs/FProject/Zombie/FemaleZombie");
            AddMaterial(ZombieModel.Female, "Materials/ZN_Female1");
            AddMaterial(ZombieModel.Female, "Materials/ZN_Female2");
            AddMaterial(ZombieModel.Female, "Materials/ZN_Female4");
            AddPrefab(ZombieModel.Male, "Prefabs/FProject/Zombie/MaleZombie");
            AddMaterial(ZombieModel.Male, "Materials/ZN_Male1");
            AddMaterial(ZombieModel.Male, "Materials/ZN_Male2");
            AddMaterial(ZombieModel.Male, "Materials/ZN_Male3");
            AddMaterial(ZombieModel.Male, "Materials/ZN_Male4");
        }


        void AddPrefab(ZombieModel model, string path)
        {
            GameObject ob = Resources.Load(path) as GameObject;
            if (ob)
            {
                var t = (Transform)Transform.Instantiate(ob.transform, Vector3.zero, Quaternion.identity);
                zombiePrefabs.Add(model, t);
            }
        }


        void AddMaterial(ZombieModel model, string path)
        {
            var m = Resources.Load<Material>(path);

            if (m)
            {
                if (zombieMatrials.ContainsKey(model))
                {
                    zombieMatrials[model].Add(m);
                }
                else
                {
                    zombieMatrials.Add(model, new List<Material>() { m });
                }
            }

        }

        public Transform SpwanZombieType(ZombieModel zombieType, Vector3 position, bool needInit = true, float biasHealth = 0.0f, bool randomMaterial = true)
        {
            if (!zombiePrefabs.ContainsKey(zombieType))
            {
                return null;
            }
            Transform t = (Transform)vp_Utility.Instantiate(zombiePrefabs[zombieType], position, Quaternion.identity);
            if (randomMaterial)
            {
                if (zombieMatrials.ContainsKey(zombieType) && zombieMatrials[zombieType].Count > 0)
                {
                    int index = Random.Range(0, zombieMatrials[zombieType].Count);
                    var skineRenderer = t.GetComponentInChildren<SkinnedMeshRenderer>();
                    skineRenderer.sharedMaterial = zombieMatrials[zombieType][index];
                }
            }
            if (needInit)
            {
                ZombineBase component = t.GetComponent<ZombineBase>();
                component.Init((int)zombieType, biasHealth);

            }
            return t;
        }
    }
}