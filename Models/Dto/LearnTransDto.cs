namespace PuppyLearn.Models.Dto
{
    /// <summary>
    /// Options: 翻译四选一
    /// </summary>
    public class LearnTransDto
    {
        public WordDto WordDto { get; set; } = null!;
        public List<string> Options { get; set; } = null!;
        public int RightAnIdx { get; set; }
        public int? WordStatus { get; set; }
        /// <summary>
        /// null for not been learned;1 for wrong answer, status plus one; -1 for right answer, status minus one;
        /// </summary>
        public int? WordStatusOffset { get; set; }
    }
}
