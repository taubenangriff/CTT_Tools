using CTT_Tools.src;
using FileDBSerializing;

public class DevPlayground
{
    public static void Main(string[] args)
    {
        cttFile file = new cttFile("res/0x1.ctt");
        file.Downsampled.Save("downsampled.bmp");
        for (int i = 0; i < file.Deltas.Length; i++)
        {
            file.Deltas[i].Save("" + i + ".bmp");
        }
    }
}