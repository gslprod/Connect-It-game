namespace ConnectIt.DI.Installers
{
    public static class ExecutionOrderConstants
    {
        public static class Initializable
        {
            public const int FileSaver = -20;
            public const int GameSaveProvider = -15;
            public const int PortViewFromModelSpawner = -10;
            public const int ConnectionLineViewFromModelSpawner = -10;
            public const int CreatedPortsRegistrator = -10;
        }
    }
}
