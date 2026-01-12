using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rigi;
    public float speed;
    public float lifeSpan;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeSpan);
        rigi.AddForce(this.transform.forward * speed, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
        {
            if (other.transform.tag == "Enemy")
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }

}
