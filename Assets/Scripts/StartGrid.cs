using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StartGrid : MonoBehaviour
{
    [SerializeField] GameObject[] fish;
    [SerializeField] TimerDisplay timerDisplay;

    List<SkinnedMeshRenderer> lightRenderer = new List<SkinnedMeshRenderer>();

    InputController[] inputControllers;

    int curLight;
    bool shouldMove;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        inputControllers = FindObjectsOfType<InputController>();

        // Disable all input
        foreach (var controller in inputControllers)
        {
            controller.enabled = false;
        }

        // Find a timer display if its missing
        if (timerDisplay == null)
        {
            timerDisplay = FindObjectOfType<TimerDisplay>();
        }

        // Setup light materials
        foreach (var fishItem in fish)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = fishItem.transform.Find("Light").GetComponent<SkinnedMeshRenderer>();
            UpdateLightColor(skinnedMeshRenderer, Color.red, 0);
            lightRenderer.Add(skinnedMeshRenderer);
        }

        // Final bits
        targetPos = transform.position + (Vector3.up * 10);
        timerDisplay.ShouldTick = false;

        // Start the timer
        StartCoroutine(UpdateLight());
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void UpdateLightColor(SkinnedMeshRenderer skinnedMeshRenderer, Color color, int emissive = -1)
    {
        var lightMaterialBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(lightMaterialBlock, 1);
        lightMaterialBlock.SetColor("_Color", color);
        if (emissive != -1) lightMaterialBlock.SetInt("_Emissive", emissive);
        skinnedMeshRenderer.SetPropertyBlock(lightMaterialBlock, 1);
    }

    IEnumerator UpdateLight()
    {
        yield return new WaitForSeconds(1);

        if (curLight == lightRenderer.Count)
        {
            Debug.Log("GO!");

            foreach (var rend in lightRenderer)
            {
                UpdateLightColor(rend, Color.green);
            }

            foreach (var controller in inputControllers)
            {
                controller.enabled = true;
            }

            timerDisplay.ShouldTick = true;

            yield return new WaitForSeconds(0.2f);

            shouldMove = true;
            yield break;
        }

        Debug.Log(lightRenderer.Count - curLight);

        UpdateLightColor(lightRenderer[curLight], Color.red, 1);
        curLight++;

        StartCoroutine(UpdateLight());
    }
}
