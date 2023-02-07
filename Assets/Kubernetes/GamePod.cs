using System.Linq;
using UnityEngine;
using k8s.Models;
using TMPro;

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
            TMP_Text resourceLabel = GameObject.GetComponentsInChildren<TMP_Text>()
                .FirstOrDefault(t => t.name == "ResourceLabel");

            if (resourceLabel is not null)
            {
                resourceLabel.text = $"{KubernetesObject.Namespace()}/{KubernetesObject.Name()}";
            }
        }
    }
}