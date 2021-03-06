using UnityEngine;

public class RadioDial : MonoBehaviour
{
    public float rotateSpd = 120f;
    public float channelWidth = 0.8f; //How close to the channel value you have to be for no whitenoise to appear
    public float whiteNoiseFadeWidth = 1f; //Distance from no whitenoise to full whitenoise outside each channel
    public float[] channelHertzValues;

    [HideInInspector] public float hertz = 88f;
    
    private float minHertz = 88f;
    private float maxHertz = 108f;
    private float dialMinAngle = -140f;
    private float dialMaxAngle = 140f;

    private float yRot;

    [SerializeField] private PlayerInput _Input;
    [SerializeField] private Music _Music;
    
    // Update is called once per frame
    void Update()
    {
        var rot = transform.localEulerAngles;
        var add = rotateSpd * Time.deltaTime;

        if (_Input.rotateDir > 0f)
        {
            if (yRot + add <= dialMaxAngle) yRot += add;
            else yRot = dialMaxAngle;
        }
        if (_Input.rotateDir < 0f)
        {
            if (yRot - add >= dialMinAngle) yRot -= add;
            else yRot = dialMinAngle;
        }
        
        transform.localRotation = Quaternion.Euler(rot.x, yRot, rot.z);

        //Calculate Hertz
        var angle = -yRot - dialMinAngle;
        hertz = minHertz + (angle / (dialMaxAngle - dialMinAngle) * (maxHertz - minHertz));
        
        //Use Hertz to select channel & whitenoise
        if (hertz <= 95f) _Music.SetChannel(0); //Channel at 91
        else if (hertz <= 101.5f) _Music.SetChannel(1); //Channel at 98
        else _Music.SetChannel(2); //Channel at 105

        var fullWhitenoise = true;
        for (var i = 0; i < channelHertzValues.Length; i++)
        {
            var dist = Mathf.Abs(hertz - channelHertzValues[i]);
            if (dist <= channelWidth)
            {
                _Music.SetParameter("Whitenoise", 0);
                fullWhitenoise = false;
            }
            else if (dist <= channelWidth + whiteNoiseFadeWidth)
            {
                _Music.SetParameter("Whitenoise", (dist - channelWidth)/whiteNoiseFadeWidth);
                fullWhitenoise = false;
                print((dist - channelWidth)/whiteNoiseFadeWidth);
            }
        }
        if (fullWhitenoise) _Music.SetParameter("Whitenoise", 1);
    }
}
