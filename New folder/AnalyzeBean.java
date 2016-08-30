package portfolio.managementsystem.ejb;

import java.util.List;

import javax.ejb.Stateless;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.persistence.TypedQuery;

import portfolio.managementsystem.jpa.Investment;
import portfolio.managementsystem.jpa.Transaction;

/**
 * Session Bean implementation class AnalyzeBean
 */
@Stateless
public class AnalyzeBean implements AnalyzeBeanRemote, AnalyzeBeanLocal {

    /**
     * Default constructor. 
     */
    public AnalyzeBean() {
        // TODO Auto-generated constructor stub
    }

    @PersistenceContext(unitName = "PortfolioManagementSystemJPA-PU")
    EntityManager em;
    
    @Override
    public List<Transaction> getTransaction(){
    	String sql = "SELECT t FROM Transaction AS t";
    	TypedQuery<Transaction> query = em.createQuery(sql,Transaction.class);
    	return query.getResultList();
    }
    
    @Override
    public Transaction getTransactionByTicker(String ticker){
    	String sql = "SELECT t from Transaction AS t where ticker="+ticker;
    	TypedQuery<Transaction> query = em.createQuery(sql,Transaction.class);
    	return query.getSingleResult();
    }
}
