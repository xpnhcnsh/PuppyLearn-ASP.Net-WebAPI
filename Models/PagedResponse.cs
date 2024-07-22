namespace PuppyLearn.Models
{
    public record PagedResponseDto<T>
    {
        public int Skip { get; set; }
        public int Take {  get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Data { get; set; }
        public PagedResponseDto(int skip, int take, int totalRecords, List<T> data)
        {
            Skip = skip;
            Take = take;
            TotalRecords = totalRecords;
            TotalPages = take==0?take:(int)Math.Ceiling((decimal)totalRecords / (decimal)take);
            Data = data;
        }
    }
}
