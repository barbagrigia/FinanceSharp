# FinanceSharp
<p align="center">
  <img align="center" src="https://i.imgur.com/6601cb9.png" height=200  />
</p> 
High performance financial computation for technical analysis with a versatile and productive piping and consolidation techniques.

## Getting Started
Have a look at [/examples](https://github.com/Nucs/FinanceSharp/blob/master/examples/FinanceSharp.Examples/Program.cs) directory for plotted examples.

## Architecture
FinanceSharp favors high productivity and complitability while still maintaining performant computation alogrithms.
We use an 2D array to represent both scalar, indicator value, trade value and tick.

#### Time
We use `long` to represent time as *1970-epoch-milliseconds* as baseline. 
Unlike in QuantConnect/Lean, our time value is always passed seperatly from the values of our data type.

All time processing are always interpreted as UTC. local time is ignored completely and always swallen and interpreted as UTC.
Converting methods are available in static `Time` class.

#### Values - DoubleArray
DoubleArray is an abstract representation of a 2 dimensional array shaped `(Count, Properties)` and is used in all of our implemented algorithms (e.g. indicators, crunchers).<br>
  `Count` - how many items are in the DoubleArray<br>
  `Properties` - how many `double` fields for every item (count) in the DoubleArray

The class aims to support in as many ways as possible data is stored in C#:
  - `double` scalar via `DoubleArrayScalar`
  - `double[]`, `double[,]`, `double[][]` via `DoubleArrayManaged` and `DoubleArray2DManaged`
  - `double*` and `length` via `DoubleArrayUnmanaged`
  - Structs - `where TStruct : unmanaged, DataStruct, ICloneable` via `DoubleArrayStruct` and `DoubleArrayStructScalar`
    - Any structure can be used to represent data in `DoubleArray` as long as all of it's fields are double (verified staticly during runtime).

The classes derived from DoubleArray can be initialized via their constructor or via `DoubleArray.From`.

`struct BarValue, struct TradeBarValue` are ordered `CHLO` instead of `OHLC` to make `Close` (index 0) complitable with `IndicatorValue` which uses its `Value` at index 0 making it possible 
to pass `CHLO` values to a indicators that accept `IndicatorValue` that has only one property.

#### IIndicator : IUpdatable
Every indicator implements both IIndicator and IUpdatable. 
//TODO: elaborate

#### I Come From QuantConnect/Lean
We support all indicators and consolidators from QuantConnect/Lean. <br>
Key differences are:
  - IndicatorBase and DataConsolidator are non generic.
  - Time is passed separately from data.
  - Instead of IndicatorDataPoint/TradeBar/RenkoBar/Tick, DoubleArray is used.
  - decimal has been replaced with double to allow interopability with other languages without casting from decimal to other type.
  - DataConsolidator inheriets IIndicator
  - All original indicator tests from Lean are passing so any consolidation code from Lean can be used with FinanceSharp.

### Copyrights
The library is based on modified code from [QuantConnect](https://github.com/QuantConnect/Lean), incredible library for full-stack trading algorithm development.
