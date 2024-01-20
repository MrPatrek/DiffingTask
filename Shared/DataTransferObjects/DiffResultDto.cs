namespace Shared.DataTransferObjects
{
    public record DiffResultDto
    {
        public string? DiffResultType { get; init; }

        public IEnumerable<DiffDto>? Diffs { get; init; }
    }

    public record DiffDto
    {
        public int Offset { get; init; }
        public int Length { get; init; }
    }
}
