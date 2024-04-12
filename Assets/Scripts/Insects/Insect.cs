using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic
}

public abstract class Insect : MonoBehaviour
{
    public int pollenP;
    public int honeyP;
    public Rarity rarity;

    [SerializeField] private float insectFlySpeed = 1f;
    [SerializeField] private float insectStayDuration = 6f;
    [SerializeField] private float flyAwayDuration = 2f;
    [SerializeField] private float RotationSpeed = 100f;

    public void Spawn(Vector3 target)
    {
        StartCoroutine(MoveInsectToPlant(target));
    }

    private IEnumerator MoveInsectToPlant(Vector3 targetPosition)
    {

        Vector3 initialPosition = transform.position;
        float journeyLength = Vector3.Distance(initialPosition, targetPosition);
        float startTime = Time.time;

        while ((Time.time - startTime) < insectStayDuration)
        {
            float distanceCovered = (Time.time - startTime) * insectFlySpeed;
            float journeyFraction = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, journeyFraction);

            Vector3 direction = (targetPosition - transform.position).normalized;

            Quaternion targetRotation;
            if (direction != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
            }

            yield return null;
        }

        Quaternion flyAwayRotation = Quaternion.LookRotation(initialPosition - targetPosition);
        float flyAwayStartTime = Time.time;

        ScoreManager.instance.UpdateScores(pollenP, honeyP);

        while ((Time.time - flyAwayStartTime) < flyAwayDuration)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, flyAwayRotation, Time.deltaTime * RotationSpeed);
            transform.Translate(Vector3.forward * insectFlySpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    public int GetRarityPercentage()
    {
        int percentage = 0;
        switch (rarity)
        {
            case Rarity.Common:
                percentage = 20;
                break;
            case Rarity.Uncommon:
                percentage = 10;
                break;
            case Rarity.Rare:
                percentage = 5;
                break;
            case Rarity.Epic:
                percentage = 1;
                break;
        }

        return percentage;
    }
}
