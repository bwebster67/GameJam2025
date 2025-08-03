using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public TMP_Text text;
    public int kills = 0;

    private void OnEnable()
    {
        Enemy.OnEnemyDied += AddKill;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= AddKill;
    }

    public void AddKill(Enemy enemy)
    {
        kills++;
        text.text = $"{kills}";
    }

}
