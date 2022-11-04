using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffData buffData;
    public AudioSource buffSound;

    public BuffData GetBuffData()
    {
        return buffData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Player owner = collision.gameObject.GetComponent<PlayerRef>().owner;
            owner.AddBuff(buffData);
            //Debug.Log($"Buff {this.ToString()}");
            BuffManager.Instance.OnBuffDestroyed.Invoke();
            if (this.buffSound != null)
            {
                if (SoundManager.Instance.Enabled)
                {
                    this.buffSound.transform.SetParent(GameManager.Instance.transform);
                    this.buffSound.Play();
                }
            }
            this.gameObject.SetActive(false);
        }
    }
}
