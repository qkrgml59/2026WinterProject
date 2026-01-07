using UnityEngine;

[System.Serializable]
public class Horse
{
    public string name;
    public float speed;
    public float positionX;
    public bool finished;

    public Horse(string name, float speed)
    {
        this.name = name;
        this.speed = speed;
        positionX = 0f;
        finished = false;
    }

    public void Move(float deltaTIme)
    {
        if (finished) return;

        positionX += speed * deltaTIme;
    }
}
