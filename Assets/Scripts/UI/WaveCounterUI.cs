using TMPro;
using DesoliteTanks.EntitySpawner;

public class WaveCounterUI : Semaphore
{
    public TextMeshProUGUI waveText;
    public string wavePrefix = "Wave: ";
    public string waveStartPrefix = "Wave Starting in ";
    
    private int wave = 0;
    private bool isCountingdown;

    public PointShopWaveSpawner waveSpawner;

    protected override void SephamoreStart(Manager manager)
    {
        waveSpawner.onWaveStarted += IncrementWave;
        waveSpawner.onWaveStarting += WaveCountdown;
    }

    private void Update()
    {
        if (isCountingdown)
        {
            waveText.text = waveStartPrefix + waveSpawner.CurrentWaveInterval.ToString("0.0");
        }
    }

    public void WaveCountdown()
    {
        isCountingdown = true;
    }

    public void IncrementWave()
    {
        wave++;
        waveText.text = wavePrefix + wave.ToString();
        isCountingdown = false;
    }
}
