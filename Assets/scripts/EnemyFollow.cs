 using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed;
    public float range;
    private Transform target;
    private Rigidbody rigi;
    private RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(this.transform.position,target.position - this.transform.position,
         out hit, range))
         {
            if (hit.transform.CompareTag("Player"))
            {
                Vector3 direction = target.position - this.transform.position;
                Vector3 velo = direction.normalized * speed * Time.deltaTime;
                rigi.MovePosition(rigi.position + velo);
            }
         }
    }
}
