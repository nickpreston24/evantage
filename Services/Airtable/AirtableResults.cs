using System.Collections.Generic;

namespace CodeMechanic.Airtable;

public class AirtableResults<TResult>
{
    public List<TResult> Data = new();
}
