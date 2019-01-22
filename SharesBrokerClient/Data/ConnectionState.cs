namespace SharesBrokerClient.Data
{
    public enum ConnectionState
    {
        NotConnected,
        Connected,
        Unauthorized,
        Forbidden,
        Unreachable,
        Error
    }
}
