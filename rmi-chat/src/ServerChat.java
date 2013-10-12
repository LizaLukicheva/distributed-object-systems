import java.rmi.*;

public interface ServerChat extends Remote {
	public void register (ClientChat newClient) throws RemoteException;
	public void sendMessage(String name, String message) throws RemoteException;
}