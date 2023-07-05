
public static class Utills
{
    public static int Ceil(float target)
    {
        if (target % 1 > 0.001f)
        {
            return (int)target + 1;
        }

        return (int)target;
    }
}
