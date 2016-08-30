package portfolio.managementsystem.ejb;

import java.util.List;

import javax.ejb.Local;

import portfolio.managementsystem.jpa.Transaction;

@Local
public interface AnalyzeBeanLocal {
    public List<Transaction> getTransaction();
    public Transaction getTransactionByTicker(String ticker);
    
}
