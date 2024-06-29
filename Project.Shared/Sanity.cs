public class Sanity
{
    // This is a shared method that can be called from both the client and server
    // You can change the project configuration in the top left corner of the editor
    // to switch between the client and server projects

    public void SharedMethod()
    {
        // This is a shared method that can be called from both the client and server
    }

#if SERVER

    // This is a server-only method
    public void ServerMethod()
    {
        // This is a server-only method
    }

#endif

#if CLIENT

    // This is a client-only method
    public void ClientMethod()
    {
        // This is a client-only method
    }

#endif
}