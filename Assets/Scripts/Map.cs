using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    [SerializeField] private GameObject blackFloor;

    private void Start()
    {
        for (int x = 0; x < this.mapWidth; x++)
        {
            for (int y = 0; y < this.mapHeight; y++)
            {
                Instantiate(blackFloor, new Vector3(x * 6 - (this.mapWidth * 6 / 2), y * 4 - (this.mapHeight * 4 / 2), this.transform.position.z), Quaternion.identity, this.transform);
            }
        }
    }
}
