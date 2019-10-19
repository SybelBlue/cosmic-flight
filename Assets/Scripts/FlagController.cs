using UnityEngine;

public class FlagController : MonoBehaviour
{

    public bool isRaised;

    // Start is called before the first frame update
    void Start()
    {
        isRaised = false;
        transform.localScale = new Vector3(0.05f, 0.05f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRaised)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1), 0.2f);
        }
        else
        {
            if (!gameObject.activeSelf) return;

            if (transform.localScale.sqrMagnitude < 0.1f)
            {
                gameObject.SetActive(false);
                return;
            }

            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0), 0.2f);
        }
    }

    internal void SetRaised(bool raised)
    {
        isRaised = raised;
        gameObject.SetActive(true);
    }
}
