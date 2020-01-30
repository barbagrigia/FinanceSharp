# FinanceSharp
<p align="center">
  <img align="center" src="https://i.imgur.com/6601cb9.png" height=200  />
</p> 
High performance financial computation for technical analysis with a versatile and productive piping and consolidation techniques.


## Architecture
FinanceSharp favors high productivity while still maintaining performant computation alogrithms.
We use an 2D array to represent both scalar, indicator value, trade value and tick.
#### Time
We use `long` to represent time as 1970-epoch-milliseconds as baseline. Converts are available in static `Time` class.
Unlike in QuantConnect/Lean, our time value is always passed seperatly from the values of our data type.
All time processing are always interpreted as UTC. local time is ignored completely and always swallen and interpreted as UTC.

#### Values
- DoubleArray - memory array with two dimensions, Count and Properties. A candle may have 4 properties (OHLC) but 5 candles therefore making it shaped (count: 5, properties: 4).
- TradeValue, IndicatorValue, TickValue - are the unmanaged struct representation of a set of properties in a DoubleArray - meaning TradeValue has 4 fields: OHLC and it represents a DoubleArray shaped (n, 4).
  All Value structures must implement `interface DataStruct : ICloneable`

#### Its not a bug, its a feature
- TradeValue data are ordered `CHLO` instead of `OHLC` to make `Close` (index 0) complitable with `IndicatorValue` which uses its `Value` at index 0.

### Copyrights
The library is based on modified code from [QuantConnect](https://github.com/QuantConnect/Lean), incredible library for full-stack trading algorithm development.
