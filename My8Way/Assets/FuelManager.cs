using UnityEngine;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
    public static FuelManager instance;

    public Slider fuelSlider;
    public float maxFuel = 100f;
    public float currentFuel;
    public float drainRate = 5f;

    private GameManager gameManager;

    void Start()
    {
        instance = this;
        currentFuel = maxFuel;
        fuelSlider.maxValue = maxFuel;
        fuelSlider.value = currentFuel;

        Debug.Log("FuelManager iniciado");

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (currentFuel > 0)
        {
            currentFuel -= drainRate * Time.deltaTime;
            currentFuel = Mathf.Max(0f, currentFuel);
            fuelSlider.value = currentFuel;

            if (currentFuel <= 0f)
            {
                gameManager.OnFuelEmpty();
            }
        }
    }

    public void RefillFuel(float percent)
    {
        float amount = maxFuel * percent;
        currentFuel = Mathf.Min(maxFuel, currentFuel + amount);
        fuelSlider.value = currentFuel;
    }

    public void ReduceFuelByPercent(float percent)
    {
        float amount = maxFuel * percent;
        currentFuel = Mathf.Max(0f, currentFuel - amount);
        fuelSlider.value = currentFuel;

        if (currentFuel <= 0f)
        {
            gameManager.OnFuelEmpty();
        }
    }
}
