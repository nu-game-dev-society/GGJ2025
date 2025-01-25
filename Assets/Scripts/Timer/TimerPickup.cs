using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerPickup : MonoBehaviour
{
    [SerializeField] int RespawnDelay;
    [SerializeField] int IncreaseAmount;

    MeshRenderer Renderer;

    // Start is called before the first frame update
    void Start()
    {
        Renderer =  GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!Renderer.enabled) return;

        Renderer.enabled = false;

        GameTimer.Instance.Timer += IncreaseAmount * 100;

        StartCoroutine(ReEnable());
    }

    IEnumerator ReEnable()
    {
        yield return new WaitForSeconds(RespawnDelay);

        Renderer.enabled = true;
    }
}
