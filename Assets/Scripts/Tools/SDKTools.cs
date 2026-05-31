using UnityEngine;

public class SDKTools
{
    public static double NormalECPM(double revenue)
    {
        double ecpm = revenue > 0 ? revenue * 1000 : 0;
        return ecpm;
    }

    public static double NormalRevenue(double revenue)
    {
        double _revenue = revenue > 0 ? revenue : 0;
        return _revenue;
    }

    public static string NormalEcpm(double revenue)
    {
        double ecpm = revenue > 0 ? revenue * 1000 : 0;
        return $"{ecpm}";
    }
}
