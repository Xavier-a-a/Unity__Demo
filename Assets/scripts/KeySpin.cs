using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
   
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
