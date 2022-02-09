using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=8PXPyyVu_6I&t=49s&ab_channel=gamesplusjames
public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController instance;
    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;
    public float rotationMultiplier = 15f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;
            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;
            transform.position += new Vector3(xAmount, yAmount, 0f);
            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + (shakeRotation * Random.Range(-1f, -1f)));
    }
    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;
        shakeFadeTime = power / length;
        shakeRotation = power * rotationMultiplier;
    }
}
