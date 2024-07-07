namespace PuppyLearn.Models.Dto
{
    /// <summary>
    /// Single Choice Question Card, the question can be a Word(choose the right translation out of 4), or a String type question, and String type answers;
    /// Can add more types of answers, like image latter.
    /// </summary>
    public class SingleChoiceQCardto
    {
        public Word? Word { get; set; }
        public string? Question { get; set; }
        public List<string>? OptionsStr { get; set; }
        public int Answer {  get; set; }
    }
}
