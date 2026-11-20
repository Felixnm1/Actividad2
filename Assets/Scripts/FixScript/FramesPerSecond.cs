using UnityEngine;

public class FramesPerSecond : MonoBehaviour
    

{
    public int FPS = 120;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = FPS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
