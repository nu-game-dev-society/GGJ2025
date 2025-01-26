using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StartGrid : MonoBehaviour
{
    [SerializeField] GameObject[] fish;
    [SerializeField] TimerDisplay timerDisplay;

    List<Material> lightMaterials = new List<Material>();

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
            Material[] materials = skinnedMeshRenderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                Material mat = materials[i];

                // Just get the light material
                if (!mat.name.Contains("Light")) continue;

                var newMat = new Material(mat);
                newMat.SetColor("_Color", Color.red);
                materials[i] = newMat;

                lightMaterials.Add(newMat);
            }

            skinnedMeshRenderer.SetMaterials(materials.ToList());
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

    IEnumerator UpdateLight()
    {
        yield return new WaitForSeconds(1);

        if (curLight == lightMaterials.Count)
        {
            Debug.Log("GO!");

            foreach (var mat in lightMaterials)
            {
                mat.SetColor("_Color", Color.green);
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

        Debug.Log(lightMaterials.Count - curLight);

        lightMaterials[curLight].SetColor("_Color", Color.yellow);
        curLight++;

        StartCoroutine(UpdateLight());
    }
}
