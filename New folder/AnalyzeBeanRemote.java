package portfolio.managementsystem.ejb;

import java.util.List;

import javax.ejb.Remote;

import portfolio.managementsystem.jpa.Transaction;

@Remote
public interface AnalyzeBeanRemote {
    public List<Transaction> getTransaction();
    public Transaction getTransactionByTicker(String ticker);

}
