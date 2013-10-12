import java.rmi.*;
import java.rmi.server.*;
import java.rmi.registry.*;
import java.util.ArrayList;
import java.net.*;

public class ServerChatImpl extends UnicastRemoteObject 
		implements ServerChat {
	
	ArrayList<ClientChat> clients = new ArrayList<ClientChat>();	
	
	public ServerChatImpl() throws RemoteException {
		
	}

	@Override
	public void sendMessage (String userName, String message) throws RemoteException {
		for (ClientChat client : clients) {
			client.newMessage(userName, message);
		}
		System.out.println(userName + ": " + message);
	}

	@Override
	public void register(ClientChat newClient) throws RemoteException {
		clients.add(newClient);
	}
	
	public static void main(String[] args) throws Exception {
		System.out.println("Initializing chat server...");
		
		ServerChat server = new ServerChatImpl();
		String BINDING_NAME = "rmi://localhost/ServerChat";
		Naming.rebind(BINDING_NAME, server);
		
		System.out.println("Server started.");
	}

}
