namespace PuppyLearn.Models.Dto
{
    public class WordDto
    {
        public Guid? Id { get; set; }
        public string WordName { get; set; } = null!;
        public Guid BookId { get; set; }
        public string BookNameCh { get; set; } = null!;
        public string? Ukphone { get; set; }
        public string? Usphone { get; set; }
        public string? Ukspeech { get; set; }
        public string? Usspeech { get; set; }
        public string? Phone { get; set; }
        public string? Speech { get; set; }
        public ICollection<CognatesDto> Cognates { get; set; } = null!;
        public ICollection<PhraseDto> Phrases { get; set; } = null!;
        public ICollection<RemMethodDto> RemMethods { get; set; } = null!;
        public ICollection<SentenceDto> Sentences { get; set; } = null!;
        public ICollection<SingleChoiceQuestionDto> SingleChoiceQuestions { get; set; } = null!;
        public ICollection<SynonymousDto> Synonymous { get; set; } = null!;
        public ICollection<TranDto> Trans { get; set; } = null!;
        public ICollection<ProgressDto>? Progress { get; set; }
    }

    /// <summary>
    /// Including a word to be updated and Fields that have been changed and to be updated, would only change those fiels rather than the whole word.
    /// <para>Fields has two patterns:
    /// "FieldName:GUID"-> indicates a field has been changed and need to be updated.
    /// "FieldName':new"-> indicates a new field has been added, thus it has no GUID, one need to be generated serverside.</para>
    /// </summary>
    public class WordNFieldsDto
    {
        public WordDto WordDto { get; set; } = null!;
        public List<string> Fields { get; set; } = null!;
    }

    public class CognatesDto
    {
        public Guid? Id { get; set; }
        public string? CognateEn { get; set; }
        public string? CognateCn { get; set; }
        public string? Pos { get; set; }
    }

    public class PhraseDto
    {
        public Guid? Id { get; set; }
        public string? PhraseEn { get; set; }
        public string? PhraseCn { get; set; }
    }

    public partial class RemMethodDto
    {
        public Guid? Id { get; set; }
        public string? Method { get; set; }
    }

    public partial class SentenceDto
    {
        public Guid? Id { get; set; }
        public string? SentenceEn { get; set; }

        public string? SentenceCn { get; set; }
    }

    public partial class SingleChoiceQuestionDto
    {
        public Guid? Id { get; set; }
        public string? QuestionEn { get; set; }

        public string? AnsExplainCn { get; set; }

        public int? AnswerIndex { get; set; }

        public string? Choice1 { get; set; }

        public string? Choice2 { get; set; }

        public string? Choice3 { get; set; }

        public string? Choice4 { get; set; }
    }

    public partial class SynonymousDto
    {
        public Guid? Id { get; set; }
        public string? Pos { get; set; }

        public string? TransCn { get; set; }

        public string? SynoEn { get; set; }
    }

    public partial class TranDto
    {
        public Guid? Id { get; set; }
        public Guid? WordId { get; set; }
        public string? TransCn { get; set; }
        public string? TransEn { get; set; }
        public string? Pos { get; set; }
    }

    public class ProgressDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public Guid WordId { get; set; }
        /// <summary>
        /// 1:1级；2:2级；3:3级（等级越高，表示本单词背错的次数越多，每次生成背诵List时，从3级选择较最多单词，2级选择中等个数单词，从1级选择最少的单词；如果背刺背错，则升级，反之降级；最高3级，当为0级时，从Progress表里删除本条目。）
        /// </summary>
        public int Status { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
