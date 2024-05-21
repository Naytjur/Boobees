using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Insect : MonoBehaviour
{
    [SerializeField]
    private InsectSO insectSO;

    public void Spawn(Vector3 target)
    {
        StartCoroutine(MoveInsectToPlant(target));

        if(!insectSO.unlocked)
        {
            insectSO.unlocked = true;
        }
    }

    private IEnumerator MoveInsectToPlant(Vector3 targetPosition)
    {

        Vector3 initialPosition = transform.position;
        float journeyLength = Vector3.Distance(initialPosition, targetPosition);
        float startTime = Time.time;

        while ((Time.time - startTime) < insectSO.insectStayDuration)
        {
            float distanceCovered = (Time.time - startTime) * insectSO.insectFlySpeed;
            float journeyFraction = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, journeyFraction);

            Vector3 direction = (targetPosition - transform.position).normalized;

            Quaternion targetRotation;
            if (direction != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * insectSO.rotationSpeed);
            }

            yield return null;
        }

        Quaternion flyAwayRotation = Quaternion.LookRotation(initialPosition - targetPosition);
        float flyAwayStartTime = Time.time;

        ScoreManager.instance.UpdateScores(insectSO.pollenProduction, insectSO.honeyProduction);

        while ((Time.time - flyAwayStartTime) < insectSO.flyAwayDuration)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, flyAwayRotation, Time.deltaTime * insectSO.rotationSpeed);
            transform.Translate(Vector3.forward * insectSO.insectFlySpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
