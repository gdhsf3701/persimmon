using csiimnida.CSILib.SoundManager.RunTime;
using UnityEngine;

public class DataBase : MonoSingleton<DataBase>
{
    private int Point = 0;

    public int GetPoint()
    {
        return Point;
    }

    public void SetPoint(int point)
    {
        Point = point;
    }
}
