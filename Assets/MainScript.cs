using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using k8s;
using k8s.Models;
using Unity.Template.VR.Kubernetes;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    private static ConcurrentDictionary<String, GamePod> _pods;

    public GameObject k8sResourcePrefab;
    public ClusterState clusterState;

    public static MainScript Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        clusterState ??= new ClusterState();
        Instance.clusterState.startWatches();
    }

    void Update()
    {
        Instance.SyncPods();
    }

    private void createKube<TGame, TKube>(String key, ConcurrentDictionary<String, TGame> dictionary, TGame value)
        where TKube :
        IKubernetesObject,
        IKubernetesObject<V1ObjectMeta>,
        IMetadata<V1ObjectMeta>,
        IValidate
        where TGame : GameResourceBase<TKube>
    {
        if (value.GameObject is null)
        {
            print($"Creating {value.GetType().Name}: {key}");
            GameObject cube = Instantiate(k8sResourcePrefab);
            value.GameObject = cube;
            value.UpdateGameObject();
            dictionary.AddOrUpdate(key, value, (k, v) => v);
        }
    }

    private void destroyKube<TGame, TKube>(String key, ConcurrentDictionary<String, TGame> dictionary)
        where TKube :
        IKubernetesObject,
        IKubernetesObject<V1ObjectMeta>,
        IMetadata<V1ObjectMeta>,
        IValidate
        where TGame : GameResourceBase<TKube>
    {
        print($"Destroying Kube: {key}");
        GameObject cube = dictionary[key].GameObject;
        Destroy(cube);
        dictionary.TryRemove(key, out var v);
    }

    void Sync<TKube, TGame>(
        ConcurrentDictionary<String, TGame> gameDict,
        ConcurrentDictionary<String, TKube> kubeDict,
        TGame defaultGameValue,
        float verticalOffset) where TKube :
        IKubernetesObject,
        IKubernetesObject<V1ObjectMeta>,
        IMetadata<V1ObjectMeta>,
        IValidate
        where TGame : GameResourceBase<TKube>
    {
        var i = 0;
        foreach (var key in kubeDict.Keys)
        {
            var gameValue = gameDict.GetValueOrDefault(key, defaultGameValue);
            var kubeValue = kubeDict.GetValueOrDefault(key, defaultGameValue.KubernetesObject);
            gameValue.KubernetesObject = kubeValue;
            createKube<TGame, TKube>(key, gameDict, gameValue);
            if (!gameValue.Moveable)
            {
                gameValue.GameObject.transform.position = new Vector3(i * 2, verticalOffset, 2f);
            }

            gameValue.Moveable = true;
            i++;
        }

        foreach (var key in gameDict.Keys)
        {
            if (!kubeDict.ContainsKey(key))
            {
                destroyKube<TGame, TKube>(key, gameDict);
            }
        }
    }

    void SyncPods()
    {
        _pods ??= new ConcurrentDictionary<string, GamePod>();
        Sync(_pods, clusterState.Pods, new GamePod(new V1Pod(), null), 2f);
    }
}