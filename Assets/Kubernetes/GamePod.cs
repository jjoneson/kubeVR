using UnityEngine;
using k8s.Models;

namespace Unity.Template.VR.Kubernetes
{
    public class GamePod : GameResourceBase<V1Pod>
    {
        public override Color _color => Color.blue;

        public GamePod(V1Pod kubernetesObject, GameObject gameObject) : base(kubernetesObject, gameObject)
        {
        }

        public override void UpdateGameObject()
        {
        }
    }
}