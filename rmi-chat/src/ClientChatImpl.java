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
		this.server = (ServerChat)Naming.lookup("rmi://localhost/ServerChat");

		System.out.println("Enter your name: ");
		this.name = readLine();
		
		server.register(this);
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
		while (true) {
			String message = readLine();
			server.sendMessage(this.name, message);
		}
	}
	
	@Override
	public void newMessage(String user, String message) throws RemoteException {		
		if (!user.equals(this.name)) {
			System.out.println(user + ": " + message);
		}
	}
	
	public static void main (String[] args) throws Exception {
		ClientChatImpl client = new ClientChatImpl();	
		client.chat();
	}
}
