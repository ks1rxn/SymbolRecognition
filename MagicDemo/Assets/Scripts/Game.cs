
public class Game  {
	public static EventService EventService { get; set; }

	static Game() {
		EventService = new EventService();
	}

}