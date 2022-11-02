using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text textDamageTemplate;
    [SerializeField] float destroyDuration;

    private void Start()
    {
        textDamageTemplate.gameObject.SetActive(false);
    }

    public void OnBehitCallback(Player player, int damage)
    {
        var textDamage = Instantiate(textDamageTemplate, transform);
        textDamage.gameObject.SetActive(true);

        // Position
        var displacement = (player.faceDirection == Player.FaceDirection.LeftRight ? 0.5f : -0.7f) + Random.Range(0, 0.4f);
        textDamage.gameObject.transform.position = player.heroHead.transform.position + (Vector3.up * 0.3f + Vector3.right * displacement);

        // Damage text
        textDamage.text = (-damage).ToString();

        // Effect
        textDamage.transform.localScale = new Vector3(1, 0, 1);
        textDamage.transform.DOScaleY(1, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            StartCoroutine(DestroyDamage(textDamage.gameObject));
        });
    }

    public IEnumerator DestroyDamage(GameObject gameObject)
    {
        yield return new WaitForSeconds(destroyDuration);

        gameObject.transform.DOScaleY(0, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
