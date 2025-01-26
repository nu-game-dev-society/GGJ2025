using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiCameraDirector : MonoBehaviour
{
    public CinemachineVirtualCamera Camera;
    public Position Position;
    void Start()
    {
        var r = GetOrderedTransforms();
        StartCoroutine(SwitchCam(r?.Length > 0 ? r[^1] : null));// followLastPlace
    }

    // Update is called once per frame
    IEnumerator SwitchCam(Transform target)
    {
        if (target != null)
        {
            Camera.Follow = target;
            Camera.LookAt = target;
            Position.player = target;
        }
        yield return new WaitForSeconds(6.0f);
        StartCoroutine(SwitchCam(GetRandom()));
    }

    public Transform[] GetOrderedTransforms()
    {
        return Position.carPct.OrderBy(e => e.Item2).Reverse().Select(x => x.Item1).ToArray();

    }
    public Transform GetRandom()
    {
        var v = GetOrderedTransforms();
        return v[Random.Range(0, v.Length)];
    }
}
