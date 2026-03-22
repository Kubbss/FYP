using UnityEngine;
using UnityEngine.UI;

public class PlayerSprint : MonoBehaviour
{
    private PlayerMotor motor;
    
    public float sprint;
    private float maxSprint = 5; //In Seconds
    private float sprintRegen = 0.5f; //0.5 = Every second regenerate 0.5 seconds of Sprint;
    private float minToStartSprint = 0.2f; //Minimum Percentage needed to start sprint (Between 0 and 1)
    private bool sprinting;

    public bool queSprint = false; 

    public bool IsSprinting => sprinting;

    public Image sprintBar;
    
    
    
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        
        sprint = maxSprint;
        sprinting = false;
    }

    void Update()
    {
        sprint = Mathf.Clamp(sprint, 0.0f, maxSprint);
        
        if (queSprint)
            TryStartSprint();
        
        UpdateSprint();
    }

    public void UpdateSprint()
    {
        if (sprint <= 0)
            SprintOff();
        
        if (sprinting)
        {
            sprint -= Time.deltaTime;
        }
        else if (sprint < maxSprint)
        {
            sprint += sprintRegen * Time.deltaTime;
        }
        
        float fraction = sprint / maxSprint;
        sprintBar.fillAmount = fraction;
    }

    public void TryStartSprint()
    {
        queSprint = true;
        if (sprint > (maxSprint * minToStartSprint))
            SprintOn();
    }

    public void TryEndSprint()
    {
        queSprint = false;
        SprintOff();
    }
    
    private void SprintOff()
    {
        sprinting = false;
        motor.EndSprint();
    }
    
    private void SprintOn()
    {
        if (!motor.CanMove)
            return;
        
        sprinting = true;
        motor.StartSprint();
    }
}
