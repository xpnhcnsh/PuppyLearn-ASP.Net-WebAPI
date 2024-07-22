using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;
using System.Net;
using System.Threading;

namespace PuppyLearn.Services
{
    public class BookService : IBookService
    {
        private readonly PuppyLearnContext _context;
        private readonly IMapper _mapper;

        public BookService(PuppyLearnContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ReturnValue> AddbyFolderUrl(string url)
        {

            if (Directory.Exists(url))
            {
                var filesFullPath = Directory.GetFiles(url,"*.json").ToList();
                foreach(var fileFullPath in filesFullPath)
                {
                    using StreamReader sr = new StreamReader(fileFullPath);
                    var jsonBook = await sr.ReadToEndAsync();
                    dynamic jsonBookObj = JsonConvert.DeserializeObject(jsonBook);
                    var wordsCount = jsonBookObj.Count;

                    #region get bookName from table Books_en and check existence first.
                    #region table Books_en
                    var bookName = (string)jsonBookObj.First.bookId.Value;
                    await Console.Out.WriteLineAsync(bookName);
                    bool isBookExist = await _context.BooksEns.Where(a => a.BookName==bookName).FirstOrDefaultAsync() !=null;
                    Guid bookId;
                    string bookNameCh;
                    string catalog;
                    if (isBookExist)
                    {
                        bookId = await _context.BooksEns.Where(a=>a.BookName== bookName).Select(a=>a.Id).SingleOrDefaultAsync();
                    }
                    else
                    {
                        bookId = Guid.NewGuid();
                    }
                    bookNameCh = Global.GetBookNameCh(bookName);
                    catalog = Global.GetCatalog(bookNameCh);
                    var book = new BooksEn
                    {
                        Id = bookId,
                        BookName = bookName,
                        WordsCount = wordsCount,
                        BookNameCh = bookNameCh,
                        Catalog = catalog,
                    };
                    if (!isBookExist)
                    {
                        await _context.BooksEns.AddAsync(book);
                    }
                    #endregion
                    #endregion

                    #region insert into all other tables for each word inserating
                    foreach (var wordObj in jsonBookObj)
                    {
                        await Console.Out.WriteLineAsync((string)wordObj.headWord.Value);


                        string wordName = (string)wordObj.headWord.Value;
                        //bool isWordExist = await _context.Words.Where(a => a.WordName == wordName).FirstOrDefaultAsync() != null;
                        Guid wordId;
                        Word wordEntry;
                        string ukphone;
                        string ukspeech;
                        string usspeech;
                        string phone;
                        string speech;
                        string usphone;

                        wordId = Guid.NewGuid();
                        ukphone = (wordObj.content.word.content["ukphone"] != null) ? (string)wordObj.content.word.content.ukphone.Value : "";
                        usphone = (wordObj.content.word.content["usphone"] != null) ? (string)wordObj.content.word.content.usphone.Value : "";
                        ukspeech = (wordObj.content.word.content["ukspeech"] != null) ? (string)wordObj.content.word.content.ukspeech.Value : "";
                        usspeech = (wordObj.content.word.content["usspeech"] != null) ? (string)wordObj.content.word.content.usspeech.Value : "";
                        phone = (wordObj.content.word.content["phone"] != null) ? (string)wordObj.content.word.content.phone.Value : "";
                        speech = (wordObj.content.word.content["speech"] != null) ? (string)wordObj.content.word.content.speech.Value : "";

                        wordEntry = new Word
                        {
                            Id = wordId,
                            WordName = wordName,
                            BookId = bookId,
                            Ukphone = ukphone,
                            Usphone = usphone,
                            Ukspeech = ukspeech,
                            Usspeech = usspeech,
                            Speech = speech,
                            Phone = phone
                        };

                        #region table sentences
                        if (wordObj.content.word.content["sentence"] != null)
                        {
                            var setences = wordObj.content.word.content.sentence.sentences;
                            foreach (var sentence in setences)
                            {
                                string sentenceCn = (sentence["sCn"] != null) ? (string)sentence.sCn.Value : "";
                                string sentenceEn = (sentence["sContent"] != null) ? (string)sentence.sContent.Value : "";

                                var sentencesEntry = new Sentence
                                {
                                    Id = Guid.NewGuid(),
                                    WordId = wordEntry.Id,
                                    BookId = book.Id,
                                    SentenceCn = sentenceCn,
                                    SentenceEn = sentenceEn,
                                };
                                await _context.Sentences.AddAsync(sentencesEntry);
                            }
                        }
                        #endregion

                        #region table Synonymous
                        if (wordObj.content.word.content["syno"] != null)
                        {
                            var synos = wordObj.content.word.content.syno.synos;
                            foreach (var syno in synos)
                            {
                                var hwds = syno.hwds;
                                foreach (var hwd in hwds)
                                {
                                    string pos = (syno["pos"] != null) ? (string)syno.pos.Value : "";
                                    string transCn = (syno["tran"] != null) ? (string)syno.tran.Value : "";
                                    string synoEn = (hwd["w"] != null) ? (string)hwd.w.Value : "";

                                    var synosEntry = new Synonymou
                                    {
                                        Id = Guid.NewGuid(),
                                        WordId = wordEntry.Id,
                                        BookId = book.Id,
                                        Pos = pos,
                                        TransCn = transCn,
                                        SynoEn = synoEn
                                    };
                                    await _context.Synonymous.AddAsync(synosEntry);
                                }
                            }
                        }
                        #endregion

                        #region table Phrases
                        if (wordObj.content.word.content["phrase"] != null)
                        {
                            var phrases = wordObj.content.word.content.phrase.phrases;
                            foreach (var phrase in phrases)
                            {
                                string phraseEn = (phrase["pContent"] != null) ? (string)phrase.pContent.Value : "";
                                string phraseCn = (phrase["pCn"] != null) ? (string)phrase.pCn.Value : "";
                                var phrasesEntry = new Phrase
                                {
                                    Id = Guid.NewGuid(),
                                    WordId = wordEntry.Id,
                                    BookId = book.Id,
                                    PhraseEn = phraseEn,
                                    PhraseCn = phraseCn
                                };
                                await _context.Phrases.AddAsync(phrasesEntry);
                            }
                        }
                        #endregion

                        #region table Cognate
                        if (wordObj.content.word.content["relWord"] != null)
                        {
                            var cognates = wordObj.content.word.content.relWord.rels;
                            foreach (var cognate in cognates)
                            {
                                var words = cognate.words;
                                foreach (var word in words)
                                {
                                    string cognateCn = (word["tran"] != null) ? (string)word.tran.Value : "";
                                    string cognateEn = (word["hwd"] != null) ? (string)word.hwd.Value : "";
                                    string pos = (cognate["pos"] != null) ? (string)cognate.pos.Value : "";

                                    var cognatesEntry = new Cognate
                                    {
                                        Id = Guid.NewGuid(),
                                        WordId = wordEntry.Id,
                                        BookId = book.Id,
                                        CognateCn = cognateCn,
                                        CognateEn = cognateEn,
                                        Pos = pos
                                    };
                                    await _context.Cognates.AddAsync(cognatesEntry);
                                }
                            }
                        }
                        #endregion

                        #region table Trans
                        if (wordObj.content.word.content["trans"] != null)
                        {
                            var trans = wordObj.content.word.content.trans;
                            foreach (var tran in trans)
                            {
                                string pos;
                                string transCn;
                                string transEn;

                                pos = (tran["pos"] != null) ? (string)tran.pos.Value : "";
                                transCn = (tran["tranCn"] != null) ? (string)tran.tranCn.Value : "";
                                transEn = (tran["tranOther"] != null) ? (string)tran.tranOther.Value : "";

                                var transEntry = new Tran
                                {
                                    Id = Guid.NewGuid(),
                                    WordId = wordEntry.Id,
                                    BookId = book.Id,
                                    TransCn = transCn,
                                    TransEn = transEn,
                                    Pos = pos,
                                };
                                await _context.Trans.AddAsync(transEntry);
                            }
                        }

                        #endregion

                        #region table SingleChoiceQuestions
                        if (wordObj.content.word.content["exam"] != null)
                        {
                            var questions = wordObj.content.word.content.exam;
                            foreach (var q in questions)
                            {
                                string questionEn = (q["question"] != null) ? (string)q.question.Value : "";
                                string ansExplainCn = (q["answer"]["explain"] != null) ? (string)q.answer.explain.Value : "";
                                int answerIndex = (q["answer"]["rightIndex"] != null) ? (int)q.answer.rightIndex.Value : -1;
                                string choice1 = (q["choices"][0] != null) ? (string)q.choices[0].choice.Value : "";
                                string choice2 = (q["choices"][1] != null) ? (string)q.choices[1].choice.Value : "";
                                string choice3 = (q["choices"][2] != null) ? (string)q.choices[2].choice.Value : "";
                                string choice4 = (q["choices"][3] != null) ? (string)q.choices[3].choice.Value : "";

                                var singleChoiceQuestionsEntry = new SingleChoiceQuestion
                                {
                                    Id = Guid.NewGuid(),
                                    WordId = wordEntry.Id,
                                    BookId = book.Id,
                                    QuestionEn = questionEn,
                                    AnsExplainCn = ansExplainCn,
                                    AnswerIndex = answerIndex,
                                    Choice1 = choice1,
                                    Choice2 = choice2,
                                    Choice3 = choice3,
                                    Choice4 = choice4,
                                };
                                await _context.SingleChoiceQuestions.AddAsync(singleChoiceQuestionsEntry);
                            }
                        }
                        #endregion

                        #region table RemMethod
                        if (wordObj.content.word.content["remMethod"] != null)
                        {
                            var remMethod = new RemMethod 
                            { 
                                Id = Guid.NewGuid(),
                                WordId = wordEntry.Id,
                                BookId = book.Id,
                                Method = (string)wordObj.content.word.content.remMethod.val.Value
                            };
                            await _context.RemMethods.AddAsync(remMethod);
                        }
                        #endregion

                        #region table Words
                        await _context.Words.AddAsync(wordEntry);
                        #endregion
                    }
                    #endregion
                    await _context.SaveChangesAsync();  
                }

                return new ReturnValue
                {
                    Value = url,
                    HttpCode = System.Net.HttpStatusCode.OK,
                    Msg = "迁移完毕"
                };

            }
            else
            {
                return new ReturnValue
                {
                    Value = null,
                    HttpCode = System.Net.HttpStatusCode.NotFound,
                    Msg = "文件夹不存在"
                };
            }
        }
        public async Task<ReturnValue> GetBookList(CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var res =await _context.BooksEns.AsNoTracking().ToListAsync();
                    return new ReturnValue
                    {
                        Value = _mapper.Map<List<BookDto>>(res),
                        Msg = "查询成功，返回结果",
                        HttpCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = null,
                        Msg = "用户取消GetBookList",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "用户取消GetBookList",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch(Exception ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "error",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            
        }
        public async Task<ReturnValue> GetUserBookById(Guid bookId,Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var res = await _context.UserBooks.Where(x=>x.BookId==bookId && x.UserId==userId).ToListAsync();
                    return new ReturnValue
                    {
                        Value = res,
                        Msg = "查询成功，返回结果",
                        HttpCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = null,
                        Msg = "用户取消GetUserBookById",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "用户取消GetUserBookById",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "error",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }
        public async Task<ReturnValue> GetWordReports(int skip, int take, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    if(skip < 0|| take <= 0)
                    {
                        return new ReturnValue
                        {
                            Msg = " Take or Skip is smaller than 0!",
                            Value = $"{nameof(take)}:{take}; {nameof(skip)}:{skip}.",
                            HttpCode = HttpStatusCode.BadRequest
                        };
                    }
                    var reportList = await _context.WordReports.AsNoTracking()
                        .OrderByDescending(x => x.SubmitTime)
                        .Skip(skip)
                        .Take(take)
                        .Select(x => new WordReportGetDto
                        {
                            Id = x.Id,
                            WordName = x.Word.WordName,
                            Fields = x.Fields,
                            Comments = x.Comment,
                            ReportDate = x.SubmitTime,
                            UserEmail = x.User.Email,
                            Status = x.Status == false ? "未处理" : "已处理",
                            BookNameCh = x.Word.Book.BookNameCh!,
                        })
                        .ToListAsync(cancellationToken);
                    var res = new PagedResponseDto<WordReportGetDto>(skip, take, reportList.Count, reportList);
                    return new ReturnValue
                    {
                        Value = res,
                        Msg = $"Return {take} records, skip: {skip}",
                        HttpCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = null,
                        Msg = "用户取消GetWordReports",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "用户取消GetWordReports",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "error",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }
        public async Task<ReturnValue> UpdateAWordReport(Guid reportId, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var res = await _context.WordReports.Where(x=>x.Id == reportId).SingleOrDefaultAsync();
                    if (res != null) 
                    {
                        res.Status = true;
                        res.ProcessTime = DateTime.Now;
                    }
                    await _context.SaveChangesAsync();
                    return new ReturnValue
                    {
                        Value = res,
                        Msg = "修改成功",
                        HttpCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = null,
                        Msg = "用户取消UpdateAWordReport",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "用户取消UpdateAWordReport",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "error",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }
    
    }
}
