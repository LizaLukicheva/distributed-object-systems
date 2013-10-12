import java.rmi.*;

public interface ClientChat extends Remote {
	public void newMessage(String user, String message) throws RemoteException;
}
