package portfolio.managementsystem.response;

import java.util.Date;

public class TransactionResponse {
	private int transactionId;
	private String ticker;
	private Date transactionDate;
	private double transactionPrice;
	private int buySell;
	private int units;
	private String username;
	
	public TransactionResponse(){
		super();
	}

	public int getTransactionId() {
		return transactionId;
	}

	public void setTransactionId(int transactionId) {
		this.transactionId = transactionId;
	}

	public String getTicker() {
		return ticker;
	}

	public void setTicker(String ticker) {
		this.ticker = ticker;
	}

	public Date getTransactionDate() {
		return transactionDate;
	}

	public void setTransactionDate(Date transactionDate) {
		this.transactionDate = transactionDate;
	}

	public double getTransactionPrice() {
		return transactionPrice;
	}

	public void setTransactionPrice(double price) {
		this.transactionPrice = price;
	}

	public int getBuySell() {
		return buySell;
	}

	public void setBuySell(int buySell) {
		this.buySell = buySell;
	}

	public int getUnits() {
		return units;
	}

	public void setUnits(int units) {
		this.units = units;
	}

	public String getUsername() {
		return username;
	}

	public void setUsername(String username) {
		this.username = username;
	}

	
}
