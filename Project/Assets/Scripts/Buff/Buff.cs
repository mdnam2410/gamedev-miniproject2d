using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffData buffData;

    public BuffData GetBuffData()
    {
        return buffData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Player owner = collision.gameObject.GetComponent<Player>();
            if (owner == null)
            {
                owner = collision.gameObject.GetComponent<PlayerRef>().owner;
            }

            owner.AddBuff(buffData);
            BuffManager.Instance.OnBuffDestroyed.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
