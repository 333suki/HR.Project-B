public class Dish
{
    public long ID { get; }
    public string Name { get; }
    public string Price { get; }
    public bool IsVegan { get; }
    public bool IsVegetarian { get; }
    public bool IsHalal { get; }
    public bool IsGlutenFree { get; }

    public Dish(long id, string name, string price, bool isvegan, bool isvegetarian, bool ishalal, bool isglutenfree)
    {
        ID = id;
        Name = name;
        Price = price;
        IsVegan = isvegan;
        IsVegetarian = isvegetarian;
        IsHalal = ishalal;
        IsGlutenFree = isglutenfree;
    }

    public override string ToString()
    {
        return $"ID: {ID}\nDish Name: {Name}\nDish Price: {Price}\nVegan: {(IsVegan ? "Yes" : "No")}\nVegetarian: {(IsVegetarian ? "Yes" : "No")}\nHalal: {(IsHalal ? "Yes" : "No")}\nGluten free: {(IsGlutenFree ? "Yes" : "No")}";
    }
}
