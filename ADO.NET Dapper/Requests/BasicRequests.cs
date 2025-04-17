namespace ADO.NET_Dapper.Requests;

public enum ExactRequest
{
    All,
    AllNames,
    AllColors,
    MaxCalories,
    MinCalories,
    AvgCalories,
    AllVegetablesCount,
    AllFruitsCount,
    AllItemsByColor,
    ItemsCountByEachColor,
    ItemsBelowCalories,
    ItemsAboveCalories,
    ItemsWithinCaloriesRange,
    YellowOrRedItems
}   
public class Requests
{
    public string GetRequest(ExactRequest request)
    {
        string sql = "";
        switch (request)
        {
            case ExactRequest.All:
                sql = "SELECT * FROM Vegetables";
                break;
            case ExactRequest.AllNames:
                sql = "SELECT Name FROM Vegetables";
                break;
            case ExactRequest.AllColors:
                sql = "SELECT Color FROM Vegetables";
                break;
            case ExactRequest.MaxCalories:
                sql = "SELECT MAX(Calories) FROM Vegetables";
                break;
            case ExactRequest.MinCalories:
                sql = "SELECT MIN(Calories) FROM Vegetables";
                break;
            case ExactRequest.AvgCalories:
                sql = "SELECT AVG(Calories) FROM Vegetables";
                break;
            case ExactRequest.AllVegetablesCount:
                sql = "SELECT COUNT(*) FROM Vegetables WHERE Type = 'Vegetable'";
                break;
            case ExactRequest.AllFruitsCount:
                sql = "SELECT COUNT(*) FROM Vegetables WHERE Type = 'Fruit'";
                break;
            case ExactRequest.AllItemsByColor:
                sql = "SELECT COUNT(*) FROM Vegetables WHERE Color = @color";
                break;
            case ExactRequest.ItemsCountByEachColor:
                sql = "SELECT Color, COUNT(*) AS ItemCount FROM Vegetables GROUP BY Color";
                break;
            case ExactRequest.ItemsBelowCalories:
                sql = "SELECT * FROM Vegetables WHERE Calories < {calories}";
                break;
            case ExactRequest.ItemsAboveCalories:
                sql = "SELECT * FROM Vegetables WHERE Calories > {calories}";
                break;
            case ExactRequest.ItemsWithinCaloriesRange:
                sql = "SELECT * FROM Vegetables WHERE Calories BETWEEN @minCalories AND @maxCalories";
                break;
            case ExactRequest.YellowOrRedItems:
                sql = "SELECT * FROM Vegetables WHERE Color = 'Yellow' OR Color = 'Red'";
                break;
        }
        return sql;
    }
}