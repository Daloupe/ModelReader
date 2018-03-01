LINQ Tests:
                     Method |        Mean |        Error |     StdDev |   Gen 0 |   Gen 1 | Allocated |
--------------------------- |------------:|-------------:|-----------:|--------:|--------:|----------:|
         NumberDESC_Control |  1,504.3 us |    24.837 us |  16.428 us | 23.4375 |  5.8594 | 157.81 KB |
                 NumberDESC |  3,192.4 us |    52.398 us |  34.658 us | 66.4063 | 19.5313 | 431.28 KB | As we neeed to box value types, vannile lambdas on value types are faster, as expected.
 NameDESC_NumberASC_Control | 17,814.1 us |   436.310 us | 288.592 us |       - |       - | 236.06 KB |
         NameDESC_NumberASC | 16,542.1 us | 1,269.536 us | 839.720 us | 62.5000 |       - | 509.87 KB | Ordering by both a ref type and value type, we see a slight improvement in speed, despite the boxing.
            BoolASC_Control |    708.4 us |     5.604 us |   3.707 us | 20.5078 |  3.9063 | 128.43 KB |
                    BoolASC |  1,972.7 us |    75.764 us |  50.113 us | 68.3594 | 19.5313 | 431.24 KB | As before, single value types aren't a good fit.
          StringASC_Control | 11,163.6 us |   202.308 us | 133.815 us | 15.6250 |       - | 196.95 KB |
                  StringASC |  8,397.3 us |   148.581 us |  98.277 us | 15.6250 |       - | 196.95 KB | But we do see non-trivial speed% increase when dealing purely with Ref Types :D

Mongo Aggregate Tests:
                   Method |     Mean |      Error |     StdDev |  Gen 0 | Allocated |
------------------------- |---------:|-----------:|-----------:|-------:|----------:|
 IntASC_FindStart_Control | 443.9 us |  9.0488 us |  5.9852 us | 7.8125 |  49.58 KB | 
         IntASC_FindStart | 441.2 us |  6.6790 us |  4.4178 us | 7.8125 |  49.99 KB | No speed diff on the IQueryables.
   StringASC_Skip_Control | 389.1 us |  3.3437 us |  2.2117 us | 7.3242 |  46.74 KB |
           StringASC_Skip | 371.8 us |  0.8938 us |  0.5912 us | 7.3242 |  45.96 KB |
         BoolDESC_Control | 389.2 us |  7.2847 us |  4.8184 us | 7.3242 |  46.26 KB |
                 BoolDESC | 388.1 us |  9.9207 us |  6.5619 us | 7.3242 |   45.4 KB |
       StringDESC_Control | 434.8 us |  5.9374 us |  3.9272 us | 7.8125 |  49.48 KB |
               StringDESC | 427.0 us | 15.9523 us | 10.5515 us | 7.8125 |  49.35 KB |



Usage:
--------------------------
Main class:
    Readable<TestClass>.Readers["SomeProp"](instanceOfClass)
    Readable<TestClass>.Expressers["SomeProp"](instanceOfClass)

Quality Of Life string extensions:
    Getters:
    "SomeProp".GetFrom<T>()                         //Func<T, object>
    "SomeProp".QueryFrom<T>()                       //Expression<Func<T, object>> 

    Sorters:
    "SomeProp".Sort("ASC")                          //IOrder
    "SomeProp".Ascending()                          //IOrder
    "SomeProp".Descending()                         //IOrder
    "SomeProp".Descending().StartingAt(obj)         //PaginatedOrder



LINQ Examples:
--------------------------
    Vanilla:
    return list
        .OrderBy(n => n.Name)
        .ToList();
--------------------------

    return list
        .OrderBy(sorter.Property.Sort(sorter.Direction))
        .ToList();

    return list
        .OrderBy(sorter.Property
        .Ascending())
        .ToList();

    return list
        .OrderBy("Name"
        .Ascending())
        .ToList();

Mongo Examples:
--------------------------
    Vanilla:
    return aggregate
        .Match(TestClass.Filter.Lte("Number", 1074257077))
        .SortBy(n => n.Number)
        .Limit(_limit)
        .ToList();
--------------------------

    return aggregate
        .OrderBy(sorter.Property
        .Sort(sorter.Direction)
        .StartingAt(sorter.PreviousLastValue))
        .Limit(_limit)
        .ToList();

    return aggregate
        .OrderBy("Number"
        .Ascending()
        .StartingAt(1074257077))
        .Limit(_limit)
        .ToList();
