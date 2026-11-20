using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private static readonly int blendID = Shader.PropertyToID("_Blend");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RenderSettings.skybox.SetFloat(blendID, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
