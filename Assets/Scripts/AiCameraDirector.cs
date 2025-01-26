using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AiCameraDirector : MonoBehaviour
{
    public CinemachineVirtualCamera Camera;
    public Position Position;
    public TextMeshProUGUI SpectatorText;
    void Start()
    {
        var r = GetOrderedTransforms();
        StartCoroutine(SwitchCam(r?.Length > 0 ? r[^1] : null));// followLastPlace
    }
    private void OnEnable()
    {
        SpectatorText.gameObject.SetActive(true);
        SpectatorText.text = "Spectating";
    }
    private void OnDisable()
    {
        SpectatorText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    IEnumerator SwitchCam(Transform target)
    {
        if (target != null)
        {
            Camera.Follow = target;
            Camera.LookAt = target;
            Camera.CancelDamping();
            Position.player = target;
            SpectatorText.text = $"Spectating {target.name}";
        }
        yield return new WaitForSeconds(6.0f);
        StartCoroutine(SwitchCam(GetRandom()));
    }

    public Transform[] GetOrderedTransforms()
    {
        return Position.carPct.Where(x => x.Item1.gameObject.activeSelf).OrderBy(e => e.Item2).Reverse().Select(x => x.Item1).ToArray();

    }
    public Transform GetRandom()
    {
        var v = GetOrderedTransforms();
        return v[Random.Range(0, v.Length)];
    }
}
