public class Product{
	public int Id{get; set;}
	public string Name{get; set;}
	public decimal Price{get; set;}
	
	public Product( string name, decimal price){
		Name = name;
		if(price < 0){
		throw new ArgumentException("Price cannot be negative");
		}
		Price = price;
	}
}
