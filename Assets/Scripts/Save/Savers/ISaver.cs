namespace ConnectIt.Save.Savers
{
    public interface ISaver
    {
        void Save(string data, string saveKey);
        string Load(string loadKey);
        void Delete(string saveKey);
    }
}
