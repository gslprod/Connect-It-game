using ConnectIt.Utilities;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace ConnectIt.Localization
{
    public class TextKey
    {
        public string Key => _key;
        public IEnumerable<object> Args => _args;

        private readonly string _key;
        private readonly ILocalizationProvider _localizationProvider;
        private object[] _args;

        public TextKey(ILocalizationProvider localizationProvider,
            string textKey,
            IEnumerable<object> args)
        {
            _localizationProvider = localizationProvider;
            _key = textKey;

            if (args != null)
                SetArgs(args.ToArray());
        }

        public void SetArgs(object[] args)
        {
            Assert.ArgIsNotNull(args);

            _args = args;
        }

        public void ClearArgs()
        {
            _args = null;
        }

        public override string ToString()
        {
            return _args == null ?
                _localizationProvider.Localize(Key) :
                _localizationProvider.LocalizeFormat(Key, _args);
        }

        public class Factory : PlaceholderFactory<string, IEnumerable<object>, TextKey> { }
    }
}