namespace TeamsLauncher
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                TeamsHelper.Start();
            }
            else
            {
                TeamsHelper.Start(args[0]);
            }
        }
    }
}
