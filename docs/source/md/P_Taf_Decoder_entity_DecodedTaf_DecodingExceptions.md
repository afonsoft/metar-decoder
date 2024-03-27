# DecodingExceptions Property


If the decoded taf is invalid, get all the exceptions that occurred during decoding Note that in strict mode, only the first encountered exception will be reported as parsing stops on error Else return null;.



## Definition
**Namespace:** <a href="N_Taf_Decoder_entity.md">Taf.Decoder.entity</a>  
**Assembly:** Taf.Decoder (in Taf.Decoder.dll) Version: 1.0.2+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public ReadOnlyCollection<TafChunkDecoderException> DecodingExceptions { get; }
```



#### Property Value
<a href="https://learn.microsoft.com/dotnet/api/system.collections.objectmodel.readonlycollection-1" target="_blank" rel="noopener noreferrer">ReadOnlyCollection</a>(<a href="T_Taf_Decoder_TafChunkDecoderException.md">TafChunkDecoderException</a>)

## See Also


#### Reference
<a href="T_Taf_Decoder_entity_DecodedTaf.md">DecodedTaf Class</a>  
<a href="N_Taf_Decoder_entity.md">Taf.Decoder.entity Namespace</a>  
