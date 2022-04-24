# Data

Data is divided into **Data Sets**.  Each **Data Set** has it's own classes that represent the data and management of that **Data Set**.

In this demo application there are two data sets:

1. Weather Forecasts
2. Weather Locations

Each data set has a set of data classes.  The number depend on the application requirements for the data set.

1. Dbo record - this is the record that maps directly to the database table.  It's used by EF to retrieve data from the table.

2. Deo class - this is a class object that is used to manage editingh of records in the data set.

3. Dvo record - this is a class that is used to view the data set from views.  It's normally a composite record with joins mapping foreign keys to user prsentable values.

4. Fk classes - these are simple Key/Value objects used in edit forms to populate select lists.  There may be more that one per data set.

## Weather Location Classes

`DboWeatherLocation` maps to database table.  

Note:
1. The object is a `record` and uses `{get; init;}` to make all the mapped properties immutable.
2. The `Key` attribute is used to mark the unique reference field.
3. The Id field is a Guid.
4. The record mirrors the structure of the database table it represents.  Property nullability matches the column nullability.

```csharp
public record DboWeatherLocation
{
    [Key]
    public Guid WeatherLocationId { get; init; }

    public string? Name { get; init; }
}
```
