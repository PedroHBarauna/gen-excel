public sealed record CategoryDto(string Id, string Nome);
public sealed record FieldDto(string Id, string Label);

public sealed record SearchResultDto(string Id, string Texto);

public sealed record TextDetailsDto(
    string Id,
    string Texto,
    CategoryDto Categoria,
    List<FieldDto> Campos
);