using UnityEngine;

public class CastGodsHandMessage : IMessage {
	private CustomSymbol symbol;

	public CastGodsHandMessage(CustomSymbol symbol) {
		this.symbol = symbol;
	}

	public CustomSymbol Symbol {
		get {
			return symbol;
		}
	}

}