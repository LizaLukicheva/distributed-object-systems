import java.net.MalformedURLException;
import java.rmi.*;
import java.rmi.server.UnicastRemoteObject;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class ClientChatImpl extends UnicastRemoteObject 
				implements ClientChat {
	private String name = "";
	private ServerChat server = null;

	public ClientChatImpl() throws RemoteException, MalformedURLException, NotBoundException {
		server = (ServerChat)Naming.lookup("rmi://localhost/ServerChat");
		
		name = readLine();
		//server.register(this);
		
		System.out.print("Enter port: ");
		int port = Integer.parseInt(readLine());
		
		// Register clients interface
		//String BINDING_NAME = "rmi://localhost:"+port+"/" + name;
		//Naming.rebind(BINDING_NAME, this);
	}
	
	private static String readLine() {
		String readed = null;
		try {
			InputStreamReader converter = new InputStreamReader(System.in);
			BufferedReader in = new BufferedReader(converter);
			readed = in.readLine();
		}
		catch (IOException e) {
			System.err.println("Error on reading line: " + e);
		}
		
		return readed;
	}
	
	public void chat() throws RemoteException {
		System.out.printf(name + "(you): ");
		String message = readLine();
		server.sendMessage(name, message);
	}
	
	@Override
	public void newMessage(String user, String message) throws RemoteException {
		System.out.println(name + ": " + message);
	}
	
	public static void main (String[] args) throws Exception {
		System.out.println("Enter your name: ");

		ClientChatImpl client = new ClientChatImpl();
		
		while (true) {
			client.chat();
		}
	}
}
