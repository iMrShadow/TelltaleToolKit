namespace TelltaleToolKit.T3Types.Events;

public class EventData
{
    public EventDataType DataType { get; set; } // mDataType

    // Union members – only one is meaningful at a time
    public Symbol DataSymbol { get; set; } = Symbol.Empty; // mDataSymbolMemory
    public long DataInt { get; set; } // mDataInt (u64 → long/ulong)
    public double DataDouble { get; set; } // mDataDouble (long double → double)

    public byte Severity { get; set; } // mSeverity (char → byte)
}

public enum EventDataType
{
    eEventData_Symbol = 0,
    eEventData_Int = 1,
    eEventData_Double = 2
}
