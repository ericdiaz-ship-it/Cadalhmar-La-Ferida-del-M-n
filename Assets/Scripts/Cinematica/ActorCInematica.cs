using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ActorCinematic : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public IEnumerator MoureA(Transform destí, float velocitat)
    {
        Vector2 target = destí.position;

        while (Vector2.Distance(rb.position, target) > 0.05f)
        {
            Vector2 nova = Vector2.MoveTowards(rb.position, target, velocitat * Time.fixedDeltaTime);
            rb.MovePosition(nova);

            if (anim != null)
            {
                Vector2 dir = (target - rb.position).normalized;
                anim.SetFloat("Horizontal", dir.x);
                anim.SetFloat("Vertical", dir.y);
                anim.SetBool("isMoving", true);
            }

            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(target);

        if (anim != null)
            anim.SetBool("isMoving", false);
    }
}