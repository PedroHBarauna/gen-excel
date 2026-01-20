public sealed record CategoryDto(string Id, string Name);
public sealed record FieldDto(string Id, string Label);

public sealed record SearchResultDto(string Id, string Text);

public sealed record TextDetailsDto(
    string Id,
    string Text,
    CategoryDto Category,
    List<FieldDto> Fields
);