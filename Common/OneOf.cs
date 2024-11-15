namespace HsManCommonLibrary.Common;

public class OneOf<TOption1, TOption2>
{
    private TOption1? _option1;
    private TOption2? _option2;
    public OneOfValueState ValueState { get; private set; }
    public OneOf()
    {
    }
    
    public OneOf(TOption1? option1)
    {
        if (option1 == null)
        {
            return;
        }
        
        _option1 = option1;
        ValueState = OneOfValueState.Option1;
    }
    
    public OneOf(TOption2? option2)
    {
        if (option2 == null)
        {
            return;
        }
        
        _option2 = option2;
        ValueState = OneOfValueState.Option2;
    }

    public TOption1 Option1
    {
        get
        {
            if (ValueState != OneOfValueState.Option1 || _option1 == null)
            {
                throw new InvalidOperationException("Can not get option1.");
            }

            return _option1;
        }

        set
        {
            if (ValueState != OneOfValueState.Option1)
            {
                throw new InvalidOperationException("Can not set option1.");
            }
            
            _option1 = value;
        }
    }
    
    public TOption2 Option2
    {
        get
        {
            if (ValueState != OneOfValueState.Option2 || _option2 == null)
            {
                throw new InvalidOperationException("Can not get option2.");
            }

            return _option2;
        }

        set
        {
            if (ValueState != OneOfValueState.Option2)
            {
                throw new InvalidOperationException("Can not set option2.");
            }
            
            _option2 = value;
        }
    }

    public object GetCurrentValue()
    {
        return ValueState switch
        {
            OneOfValueState.Option1 when _option1 != null => _option1,
            OneOfValueState.Option2 when _option2 != null => _option2,
            OneOfValueState.None => throw new InvalidOperationException(),
            _ => throw new InvalidOperationException()
        };
    }
    
}   