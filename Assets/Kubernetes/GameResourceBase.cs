﻿using k8s;
using k8s.Models;
using UnityEngine;

namespace Unity.Template.VR.Kubernetes
{
    public abstract class GameResourceBase<T> where T :
        IKubernetesObject,
        IKubernetesObject<V1ObjectMeta>,
        IMetadata<V1ObjectMeta>,
        IValidate
    {
        public T KubernetesObject { get; set; }
        public GameObject GameObject { get; set; }
        public bool Moveable { get; set; }

        public abstract Color _color { get; }
        public GameResourceBase(T kubernetesObject, GameObject gameObject)
        {
            KubernetesObject = kubernetesObject;
            GameObject = gameObject;
        }

        public abstract void UpdateGameObject();
    }
}