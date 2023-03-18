namespace ConnectIt.Input
{
    public class InputProvider
    {
        private MainInput _input;

        public InputProvider()
        {
            _input = new MainInput();
        }

        public void Enable()
        {
            _input.Enable();
        }

        public void Disable()
        {
            _input.Disable();
        }
    }
}
