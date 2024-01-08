using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MovementController;

public class Stamina : MonoBehaviour
{
    public Image staminaBar;
    public Image gripBar;

    public float stamina = 100;
    public float grip = 100;

    public float recoveryRate = 5f;

    public static Stamina staminaInstance;

    private void Awake() => staminaInstance = this;

    public bool isResting = false;

    private float savesCounter = 0;

    public bool ok = true;
    public AudioSource restingSound;
    public AudioSource onLadderSound;
    public AudioSource lowGripSound;

    void Update()
    {
        if (stamina < 0)
        {
            grip += stamina;
            stamina = 0;
        }

        if (stamina > 100)
            stamina = 100;
        staminaBar.fillAmount = Mathf.Clamp(stamina / 100f, 0, 1);
        gripBar.fillAmount = Mathf.Clamp(grip / 100f, 0, 1);
        if (stamina <= 100)
            if (movementInstance.isOnLadder)
            {
                if (!onLadderSound.isPlaying)
                    onLadderSound.Play();
                if (grip < 25)
                {
                    onLadderSound.Pause();
                    if (!lowGripSound.isPlaying)
                        lowGripSound.Play();
                }
                else
                    lowGripSound.Pause();
                if (Input.GetKeyDown(KeyCode.R))
                {
                    isResting = !isResting;
                    movementInstance.canMove = !movementInstance.canMove;
                }

                if (isResting && stamina < 100)
                {
                    stamina += recoveryRate * Time.deltaTime;
                    if (stamina >= 100)
                    {
                        isResting = !isResting;
                        movementInstance.canMove = !movementInstance.canMove; 
                    }

                    onLadderSound.Pause();
                    lowGripSound.Pause();
                    if (!restingSound.isPlaying)
                    {
                        restingSound.Play();
                    }
                }
                else restingSound.Pause();
            }
            else
            {
                lowGripSound.Pause();
                onLadderSound.Pause();
                stamina += recoveryRate * Time.deltaTime;
            }

        if (grip < 0.1f)
        {
            if (ok)
                if (savesCounter < 3)
                {
                    ok = false;
                    StartCoroutine(QuickTimeEvent());
                }
                else
                {
                    recoveryRate = 0f;
                    movementInstance.isOnLadder = false;
                }
        }
    }

    IEnumerator QuickTimeEvent()
    {
        Debug.Log("S-a intrat in curutina");
        bool isSaved = false;
        float timePassed = 0;
        recoveryRate = 0f;
        movementInstance.canMove = false;
        while (timePassed < 2f)
        {
            if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.C))
            {
                ok = true;
                isSaved = true;
                savesCounter += 1;
                break;
            }

            timePassed += Time.deltaTime;
            yield return null;
        }

        movementInstance.canMove = true;
        if (isSaved)
        {
            grip = 33f;
            recoveryRate = 5f;
        }
        else
        {
            movementInstance.isOnLadder = false;
        }
    }
}